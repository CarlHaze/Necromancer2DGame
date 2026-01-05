using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NecromancersRising.UI;

namespace NecromancersRising.Battle
{
    public class BattleManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BattleUIController _uiController;
        
        [Header("Battle State")]
        private List<BattleEntity> _playerParty = new List<BattleEntity>();
        private List<BattleEntity> _enemyParty = new List<BattleEntity>();
        private List<BattleEntity> _turnOrder = new List<BattleEntity>();
        
        private BattleEntity _currentActor;
        private BattleState _currentState = BattleState.WaitingForInput;
        
        private IMove _selectedMove;
        private BattleEntity _selectedTarget;

        private void Start()
        {
            InitializeBattle();
        }

        private void InitializeBattle()
        {
            // Find all battle entities in the scene
            var allEntities = FindObjectsByType<BattleEntity>(FindObjectsSortMode.None);
            
            foreach (var entity in allEntities)
            {
                // Categorize by tag or layer - for now, assume enemies have "Enemy" tag
                if (entity.CompareTag("Enemy"))
                {
                    _enemyParty.Add(entity);
                }
                else if (entity.CompareTag("Player"))
                {
                    _playerParty.Add(entity);
                }
            }
            
            Debug.Log($"Battle initialized: {_playerParty.Count} player units, {_enemyParty.Count} enemies");
            
            // Calculate turn order
            CalculateTurnOrder();
            
            // Start first turn
            StartNextTurn();
        }

        private void CalculateTurnOrder()
        {
            _turnOrder.Clear();
            
            // Combine all entities
            var allEntities = new List<BattleEntity>();
            allEntities.AddRange(_playerParty);
            allEntities.AddRange(_enemyParty);
            
            // Sort by speed (descending)
            _turnOrder = allEntities
                .Where(e => !e.IsDead)
                .OrderByDescending(e => e.Data.baseSpeed)
                .ToList();
            
            Debug.Log($"Turn order: {string.Join(", ", _turnOrder.Select(e => e.EntityName))}");
        }

        private void StartNextTurn()
        {
            // Remove dead entities from turn order
            _turnOrder.RemoveAll(e => e.IsDead);
            
            // Check for battle end
            if (CheckBattleEnd())
            {
                return;
            }
            
            // Get next actor
            if (_turnOrder.Count > 0)
            {
                _currentActor = _turnOrder[0];
                _turnOrder.RemoveAt(0);
                _turnOrder.Add(_currentActor); // Add to end for next round
                
                Debug.Log($"=== {_currentActor.EntityName}'s turn ===");
                
                // Process turn start effects
                _currentActor.OnTurnStart();
                
                // Decide action based on whether it's player or enemy
                if (_playerParty.Contains(_currentActor))
                {
                    // Player unit - wait for player input
                    _currentState = BattleState.WaitingForInput;
                    ShowPlayerMoves(_currentActor);
                }
                else
                {
                    // Enemy unit - AI decides
                    _currentState = BattleState.ExecutingAction;
                    ExecuteEnemyTurn(_currentActor);
                }
            }
        }

        private void ShowPlayerMoves(BattleEntity actor)
        {
            Debug.Log($"Player can choose a move for {actor.EntityName}");
            
            // Get moves and populate UI
            var moves = actor.GetAvailableMoves();
            _uiController.PopulateAttackPage(moves);
        }

        public void OnSkillSelected(string skillName)
        {
            if (_currentState != BattleState.WaitingForInput) return;
            
            var moves = _currentActor.GetAvailableMoves();
            _selectedMove = moves.FirstOrDefault(m => m.MoveName == skillName);
            
            if (_selectedMove == null)
            {
                Debug.LogWarning($"Move {skillName} not found!");
                return;
            }
            
            if (!_selectedMove.CanExecute(_currentActor))
            {
                Debug.LogWarning($"Cannot execute {skillName} - not enough SP!");
                return;
            }
            
            Debug.Log($"Selected move: {_selectedMove.MoveName}");
            
            // Now wait for target selection
            _currentState = BattleState.SelectingTarget;
            
            // For now, auto-select first living enemy
            AutoSelectTarget();
        }

        private void AutoSelectTarget()
        {
            // Simple AI: select first living enemy
            _selectedTarget = _enemyParty.FirstOrDefault(e => !e.IsDead);
            
            if (_selectedTarget != null)
            {
                ExecutePlayerMove();
            }
        }

        private void ExecutePlayerMove()
        {
            if (_selectedMove == null || _selectedTarget == null) return;
            
            _currentState = BattleState.ExecutingAction;
            
            // Execute the move
            _selectedMove.Execute(_currentActor, _selectedTarget);
            
            // Process turn end effects
            _currentActor.OnTurnEnd();
            
            // Clear selections
            _selectedMove = null;
            _selectedTarget = null;
            
            // Wait a moment then continue
            Invoke(nameof(StartNextTurn), 1.5f);
        }

        private void ExecuteEnemyTurn(BattleEntity enemy)
        {
            // Simple AI: pick random move, target random player unit
            var moves = enemy.GetAvailableMoves();
            var usableMoves = moves.Where(m => m.CanExecute(enemy)).ToList();
            
            if (usableMoves.Count == 0)
            {
                Debug.Log($"{enemy.EntityName} has no usable moves!");
                StartNextTurn();
                return;
            }
            
            var randomMove = usableMoves[Random.Range(0, usableMoves.Count)];
            var target = _playerParty.Where(p => !p.IsDead).ToList();
            
            if (target.Count > 0)
            {
                var randomTarget = target[Random.Range(0, target.Count)];
                
                // Execute move
                randomMove.Execute(enemy, randomTarget);
            }
            
            // Process turn end effects
            enemy.OnTurnEnd();
            
            // Continue to next turn
            Invoke(nameof(StartNextTurn), 1.5f);
        }

        private bool CheckBattleEnd()
        {
            bool allPlayersDead = _playerParty.All(p => p.IsDead);
            bool allEnemiesDead = _enemyParty.All(e => e.IsDead);
            
            if (allPlayersDead)
            {
                Debug.Log("=== DEFEAT ===");
                _currentState = BattleState.BattleEnd;
                return true;
            }
            
            if (allEnemiesDead)
            {
                Debug.Log("=== VICTORY ===");
                _currentState = BattleState.BattleEnd;
                return true;
            }
            
            return false;
        }
    }

    public enum BattleState
    {
        WaitingForInput,
        SelectingTarget,
        ExecutingAction,
        BattleEnd
    }
}