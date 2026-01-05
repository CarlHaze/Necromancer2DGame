using UnityEngine;
using System.Collections.Generic;
using NecromancersRising.Undead; // Fixed namespace
using NecromancersRising.Combat;  // Fixed namespace
using NecromancersRising.UI;

namespace NecromancersRising.Battle
{
    public class BattleEntity : MonoBehaviour, IBattleEntity
    {
        [SerializeField] private UndeadData _undeadData;
        [SerializeField] private MonsterHPBar _hpBar;

        private BattleUIController _uiController;
        private int _currentHP;
        private int _currentSP;
        private Dictionary<string, IStatusEffect> _activeStatusEffects = new Dictionary<string, IStatusEffect>();
        private List<IMove> _availableMoves = new List<IMove>();

        public string EntityName => _undeadData.undeadName;
        public int CurrentHP => _currentHP;
        public int MaxHP => _undeadData.baseHP;
        public int CurrentSP { get; private set; }
        public int MaxSP => 100;
        public bool IsDead => _currentHP <= 0;
        public UndeadData Data => _undeadData;

        private void Start()
        {
            InitializeEntity();
        }

        public void InitializeEntity()
        {
            _currentHP = MaxHP;
            CurrentSP = MaxSP;
            
            LoadMoves();
            
            if (_hpBar != null)
            {
                _hpBar.SetHP(_currentHP, MaxHP);
                _hpBar.SetMonsterName(_undeadData.undeadName);
            }
        }

        private void LoadMoves()
        {
            _availableMoves.Clear();
            
            if (_undeadData.moves != null)
            {
                foreach (var moveData in _undeadData.moves)
                {
                    _availableMoves.Add(new MoveExecutor(moveData));
                }
            }
        }

        public List<IMove> GetAvailableMoves()
        {
            return new List<IMove>(_availableMoves);
        }

        public void TakeDamage(int amount, DamageType damageType)
        {
            if (IsDead) return;

            int actualDamage = amount;
            if (damageType == DamageType.Physical)
            {
                actualDamage = Mathf.Max(1, amount - (_undeadData.baseDefense / 2));
            }
            else if (damageType != DamageType.True)
            {
                actualDamage = Mathf.Max(1, amount - (_undeadData.baseDefense / 3));
            }

            _currentHP = Mathf.Max(0, _currentHP - actualDamage);
            
            UpdateHPBar();
            
            // Update party UI if this is a player unit
            if (_uiController != null)
            {
                _uiController.UpdatePartyMemberDisplay(this);
            }
            
            if (IsDead)
            {
                OnDeath();
            }
        }

        public void Heal(int amount)
        {
            if (IsDead) return;
            _currentHP = Mathf.Min(MaxHP, _currentHP + amount);
            UpdateHPBar();
        }

        public void RestoreSP(int amount)
        {
            CurrentSP = Mathf.Min(MaxSP, CurrentSP + amount);
        }

        public void ConsumeSP(int amount)
        {
            CurrentSP = Mathf.Max(0, CurrentSP - amount);
        }

        public void ApplyStatusEffect(IStatusEffect statusEffect)
        {
            if (_activeStatusEffects.ContainsKey(statusEffect.StatusName))
            {
                _activeStatusEffects[statusEffect.StatusName].Remove(this);
            }

            _activeStatusEffects[statusEffect.StatusName] = statusEffect;
            statusEffect.Apply(this);
            
            if (_hpBar != null && statusEffect.Icon != null)
            {
                _hpBar.AddStatusEffectIcon(statusEffect.StatusName, statusEffect.Icon);
            }
        }

        public void RemoveStatusEffect(string effectName)
        {
            if (_activeStatusEffects.ContainsKey(effectName))
            {
                _activeStatusEffects[effectName].Remove(this);
                _activeStatusEffects.Remove(effectName);
                
                if (_hpBar != null)
                {
                    _hpBar.RemoveStatusEffect(effectName);
                }
            }
        }

        public void OnTurnStart()
        {
            foreach (var effect in _activeStatusEffects.Values)
            {
                effect.OnTurnStart(this);
            }
        }

        public void OnTurnEnd()
        {
            foreach (var effect in _activeStatusEffects.Values)
            {
                effect.OnTurnEnd(this);
            }
        }

        private void UpdateHPBar()
        {
            if (_hpBar != null)
            {
                _hpBar.SetHP(_currentHP, MaxHP);
            }
        }

        private void OnDeath()
        {
            Debug.Log($"{EntityName} has been defeated!");
        }

        public void SetUIController(BattleUIController uiController)
        {
            _uiController = uiController;
        }




    }
}