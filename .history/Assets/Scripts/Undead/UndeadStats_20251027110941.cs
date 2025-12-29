using System.Collections.Generic;
using UnityEngine;

namespace NecromancersRising.Undead
{
    /// <summary>
    /// Runtime combat statistics for an undead creature.
    /// Created from UndeadData at the start of each battle.
    /// Tracks current HP, stat stages, and move AP usage.
    /// </summary>
    public class UndeadStats
    {
        // Base stats (from UndeadData)
        public string name;
        public Combat.UndeadType type;
        public int maxHP;
        public int attack;
        public int defense;
        public int speed;
        public int accuracy;

        // Current battle state
        public int currentHP;
        public bool isFainted;

        // Stat stages (buffs/debuffs from -6 to +6)
        public int attackStage;
        public int defenseStage;
        public int speedStage;
        public int accuracyStage;

        // Move data and AP tracking
        public List<Combat.MoveData> moves;
        private Dictionary<Combat.MoveData, int> currentAP;

        /// <summary>
        /// Create runtime stats from UndeadData
        /// </summary>
        public UndeadStats(UndeadData data)
        {
            // Copy base stats
            name = data.undeadName;
            type = data.type;
            maxHP = data.baseHP;
            attack = data.baseAttack;
            defense = data.baseDefense;
            speed = data.baseSpeed;
            accuracy = data.baseAccuracy;

            // Initialize battle state
            currentHP = maxHP;
            isFainted = false;

            // Reset stat stages
            attackStage = 0;
            defenseStage = 0;
            speedStage = 0;
            accuracyStage = 0;

            // Initialize moves and AP
            moves = new List<Combat.MoveData>(data.moves);
            currentAP = new Dictionary<Combat.MoveData, int>();
            
            foreach (var move in moves)
            {
                if (move != null)
                {
                    currentAP[move] = move.maxActionPoints;
                }
            }
        }

        /// <summary>
        /// Take damage and check for fainting
        /// </summary>
        public void TakeDamage(int damage)
        {
            currentHP -= damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
                isFainted = true;
            }
        }

        /// <summary>
        /// Heal HP (cannot exceed max)
        /// </summary>
        public void Heal(int amount)
        {
            if (isFainted) return;
            
            currentHP += amount;
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
        }

        /// <summary>
        /// Get current HP as a percentage (0-1)
        /// </summary>
        public float GetHPPercent()
        {
            return (float)currentHP / maxHP;
        }

        /// <summary>
        /// Get modified attack stat based on stat stages
        /// </summary>
        public int GetModifiedAttack()
        {
            return ApplyStatStage(attack, attackStage);
        }

        /// <summary>
        /// Get modified defense stat based on stat stages
        /// </summary>
        public int GetModifiedDefense()
        {
            return ApplyStatStage(defense, defenseStage);
        }

        /// <summary>
        /// Get modified speed stat based on stat stages
        /// </summary>
        public int GetModifiedSpeed()
        {
            return ApplyStatStage(speed, speedStage);
        }

        /// <summary>
        /// Get modified accuracy based on stat stages
        /// </summary>
        public int GetModifiedAccuracy()
        {
            return ApplyStatStage(accuracy, accuracyStage);
        }

        /// <summary>
        /// Apply stat stage multiplier to a base stat
        /// Stages: -6 to +6, each stage = ~50% change
        /// </summary>
        private int ApplyStatStage(int baseStat, int stage)
        {
            float multiplier = 1.0f + (stage * 0.5f);
            return Mathf.RoundToInt(baseStat * multiplier);
        }

        /// <summary>
        /// Modify a stat stage (clamped between -6 and +6)
        /// </summary>
        public void ModifyStatStage(StatType stat, int change)
        {
            switch (stat)
            {
                case StatType.Attack:
                    attackStage = Mathf.Clamp(attackStage + change, -6, 6);
                    break;
                case StatType.Defense:
                    defenseStage = Mathf.Clamp(defenseStage + change, -6, 6);
                    break;
                case StatType.Speed:
                    speedStage = Mathf.Clamp(speedStage + change, -6, 6);
                    break;
                case StatType.Accuracy:
                    accuracyStage = Mathf.Clamp(accuracyStage + change, -6, 6);
                    break;
            }
        }

        /// <summary>
        /// Get current AP remaining for a move
        /// </summary>
        public int GetCurrentAP(Combat.MoveData move)
        {
            if (currentAP.ContainsKey(move))
            {
                return currentAP[move];
            }
            return 0;
        }

        /// <summary>
        /// Check if a move has AP remaining to be used
        /// </summary>
        public bool CanUseMove(Combat.MoveData move)
        {
            return GetCurrentAP(move) > 0;
        }

        /// <summary>
        /// Use a move (decreases AP by 1)
        /// </summary>
        public bool UseMove(Combat.MoveData move)
        {
            if (!CanUseMove(move))
            {
                Debug.LogWarning($"{name} tried to use {move.moveName} but has no AP remaining!");
                return false;
            }

            currentAP[move]--;
            return true;
        }

        /// <summary>
        /// Restore AP to a move (for items/rest)
        /// </summary>
        public void RestoreAP(Combat.MoveData move, int amount)
        {
            if (currentAP.ContainsKey(move))
            {
                currentAP[move] = Mathf.Min(currentAP[move] + amount, move.maxActionPoints);
            }
        }

        /// <summary>
        /// Restore all AP to max (for resting/items)
        /// </summary>
        public void RestoreAllAP()
        {
            foreach (var move in moves)
            {
                if (move != null && currentAP.ContainsKey(move))
                {
                    currentAP[move] = move.maxActionPoints;
                }
            }
        }

        /// <summary>
        /// Reset all battle state (for new battle)
        /// </summary>
        public void ResetBattleState()
        {
            currentHP = maxHP;
            isFainted = false;
            attackStage = 0;
            defenseStage = 0;
            speedStage = 0;
            accuracyStage = 0;
            RestoreAllAP();
        }
    }

    /// <summary>
    /// Enum for stat modification moves
    /// </summary>
    public enum StatType
    {
        Attack,
        Defense,
        Speed,
        Accuracy
    }
}