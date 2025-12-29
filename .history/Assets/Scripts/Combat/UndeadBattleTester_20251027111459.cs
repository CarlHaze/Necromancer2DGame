using UnityEngine;
using NecromancersRising.Undead;
using NecromancersRising.Combat;

namespace NecromancersRising.Combat
{
    /// <summary>
    /// Test script to run simulated battles between undead creatures.
    /// Now uses the move system with Action Points (AP) and type effectiveness.
    /// </summary>
    public class UndeadBattleTester : MonoBehaviour
    {
        [Header("Test Subjects")]
        [SerializeField] private UndeadData skeleton;
        [SerializeField] private UndeadData zombie;
        [SerializeField] private UndeadData ghoul;

        [Header("Test Settings")]
        [SerializeField] private int battlesPerMatchup = 1000;
        [SerializeField] private bool showDetailedLogs = false;

        private void Start()
        {
            Debug.Log("=== STARTING UNDEAD BATTLE SIMULATIONS ===");
            Debug.Log($"Running {battlesPerMatchup} battles per matchup");
            Debug.Log("NOW USING MOVE SYSTEM WITH TYPE EFFECTIVENESS!\n");

            RunAllMatchups();

            Debug.Log("=== BATTLE SIMULATIONS COMPLETE ===");
        }

        /// <summary>
        /// Run all possible starter matchups
        /// </summary>
        private void RunAllMatchups()
        {
            // Skeleton vs Zombie
            RunMatchup(skeleton, zombie);

            // Skeleton vs Ghoul
            RunMatchup(skeleton, ghoul);

            // Zombie vs Ghoul
            RunMatchup(zombie, ghoul);
        }

        /// <summary>
        /// Run multiple battles between two undead and report statistics
        /// </summary>
        private void RunMatchup(UndeadData undead1, UndeadData undead2)
        {
            int wins1 = 0;
            int wins2 = 0;

            Debug.Log($"--- {undead1.undeadName} vs {undead2.undeadName} ---");

            for (int i = 0; i < battlesPerMatchup; i++)
            {
                string winner = SimulateBattle(undead1, undead2);
                
                if (winner == undead1.undeadName)
                    wins1++;
                else if (winner == undead2.undeadName)
                    wins2++;
                // Ties are ignored (shouldn't happen with AP system)
            }

            // Calculate win percentages
            float winRate1 = (wins1 / (float)battlesPerMatchup) * 100f;
            float winRate2 = (wins2 / (float)battlesPerMatchup) * 100f;

            Debug.Log($"{undead1.undeadName} wins: {wins1}/{battlesPerMatchup} ({winRate1:F1}%)");
            Debug.Log($"{undead2.undeadName} wins: {wins2}/{battlesPerMatchup} ({winRate2:F1}%)\n");
        }

        /// <summary>
        /// Simulate a single battle between two undead using the new move system
        /// </summary>
        private string SimulateBattle(UndeadData data1, UndeadData data2)
        {
            // Create runtime stats for both combatants
            UndeadStats fighter1 = data1.CreateStats();
            UndeadStats fighter2 = data2.CreateStats();

            int turnCount = 0;
            int maxTurns = 100; // Prevent infinite loops

            if (showDetailedLogs)
            {
                Debug.Log($"\n=== BATTLE START: {fighter1.name} vs {fighter2.name} ===");
            }

            // Battle loop
            while (!fighter1.isFainted && !fighter2.isFainted && turnCount < maxTurns)
            {
                turnCount++;

                // Determine turn order based on speed
                UndeadStats firstAttacker, secondAttacker;
                
                if (fighter1.GetModifiedSpeed() > fighter2.GetModifiedSpeed())
                {
                    firstAttacker = fighter1;
                    secondAttacker = fighter2;
                }
                else if (fighter1.GetModifiedSpeed() < fighter2.GetModifiedSpeed())
                {
                    firstAttacker = fighter2;
                    secondAttacker = fighter1;
                }
                else
                {
                    // Speed tie - 50/50 random
                    if (Random.Range(0, 2) == 0)
                    {
                        firstAttacker = fighter1;
                        secondAttacker = fighter2;
                    }
                    else
                    {
                        firstAttacker = fighter2;
                        secondAttacker = fighter1;
                    }
                }

                // First attacker's turn
                if (!firstAttacker.isFainted && !secondAttacker.isFainted)
                {
                    ExecuteTurn(firstAttacker, secondAttacker);
                }

                // Second attacker's turn (if still alive)
                if (!firstAttacker.isFainted && !secondAttacker.isFainted)
                {
                    ExecuteTurn(secondAttacker, firstAttacker);
                }

                // Check for simultaneous KO (rare but possible)
                if (fighter1.isFainted && fighter2.isFainted)
                {
                    if (showDetailedLogs)
                    {
                        Debug.Log("=== DOUBLE KO! Choosing random winner... ===");
                    }
                    return Random.Range(0, 2) == 0 ? fighter1.name : fighter2.name;
                }
            }

            // Determine winner
            if (fighter1.isFainted)
            {
                if (showDetailedLogs)
                {
                    Debug.Log($"=== {fighter2.name} WINS! ({turnCount} turns) ===\n");
                }
                return fighter2.name;
            }
            else if (fighter2.isFainted)
            {
                if (showDetailedLogs)
                {
                    Debug.Log($"=== {fighter1.name} WINS! ({turnCount} turns) ===\n");
                }
                return fighter1.name;
            }
            else
            {
                // Max turns reached (shouldn't happen)
                Debug.LogWarning($"Battle exceeded {maxTurns} turns! Calling it a draw.");
                return "";
            }
        }

        /// <summary>
        /// Execute a single turn (one undead attacks another using a move)
        /// </summary>
        private void ExecuteTurn(UndeadStats attacker, UndeadStats defender)
        {
            // Select a move (prioritize first available move with AP)
            MoveData selectedMove = SelectMove(attacker);

            if (selectedMove == null)
            {
                if (showDetailedLogs)
                {
                    Debug.Log($"{attacker.name} has no moves available! (Shouldn't happen with 35 AP)");
                }
                return;
            }

            // Use the move (deduct AP)
            attacker.UseMove(selectedMove);

            // Check accuracy
            int hitRoll = Random.Range(1, 101);
            bool hits = hitRoll <= attacker.GetModifiedAccuracy();

            if (!hits && !selectedMove.neverMisses)
            {
                if (showDetailedLogs)
                {
                    Debug.Log($"{attacker.name}'s {selectedMove.moveName} missed!");
                }
                return;
            }

            // Calculate damage
            int damage = CalculateDamage(attacker, defender, selectedMove);

            // Apply damage
            defender.TakeDamage(damage);

            if (showDetailedLogs)
            {
                float effectiveness = TypeChart.Instance.GetEffectiveness(selectedMove.moveType, defender.type);
                string effectivenessText = TypeChart.Instance.GetEffectivenessText(effectiveness);
                
                Debug.Log($"{attacker.name} used {selectedMove.moveName}! ({selectedMove.moveType} type)");
                if (effectivenessText != "")
                {
                    Debug.Log(effectivenessText);
                }
                Debug.Log($"Dealt {damage} damage! {defender.name}: {defender.currentHP}/{defender.maxHP} HP");
                
                if (defender.isFainted)
                {
                    Debug.Log($"{defender.name} fainted!");
                }
            }
        }

        /// <summary>
        /// Select a move for the attacker to use (simple AI: use first available move)
        /// </summary>
        private MoveData SelectMove(UndeadStats attacker)
        {
            // Simple AI: Use first move with AP remaining
            foreach (var move in attacker.moves)
            {
                if (move != null && attacker.CanUseMove(move))
                {
                    return move;
                }
            }

            // No moves available (shouldn't happen with 35 AP)
            return null;
        }

        /// <summary>
        /// Calculate damage for an attack using the new formula with type effectiveness
        /// Formula: Base Damage = (Attacker.Attack - Defender.Defense/2) * Move.Power / 10 * Type Multiplier
        /// </summary>
        private int CalculateDamage(UndeadStats attacker, UndeadStats defender, MoveData move)
        {
            // Get type effectiveness multiplier
            float typeMultiplier = TypeChart.Instance.GetEffectiveness(move.moveType, defender.type);

            // Check for immunity
            if (typeMultiplier == 0f)
            {
                if (showDetailedLogs)
                {
                    Debug.Log(TypeChart.Instance.GetEffectivenessText(0f));
                }
                return 0;
            }

            // Base damage calculation (simplified)
            // Formula: ((Attack - Defense/2) * MovePower / 10) * TypeMultiplier
            int attackPower = attacker.GetModifiedAttack();
            int defensePower = defender.GetModifiedDefense();

            float baseDamage = (attackPower - (defensePower / 2f)) * (move.power / 10f);
            float finalDamage = baseDamage * typeMultiplier;

            // Ensure minimum 1 damage if move can hit
            int damage = Mathf.Max(1, Mathf.RoundToInt(finalDamage));

            return damage;
        }

        /// <summary>
        /// Validate test setup in the Unity Inspector
        /// </summary>
        private void OnValidate()
        {
            battlesPerMatchup = Mathf.Max(1, battlesPerMatchup);
        }
    }
}