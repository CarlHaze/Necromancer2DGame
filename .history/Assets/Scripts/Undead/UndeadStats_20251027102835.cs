using UnityEngine;
using NecromancersRising.Combat;

namespace NecromancersRising.Undead
{
    /// <summary>
    /// Runtime instance of undead stats
    /// Created from UndeadData and modified during gameplay
    /// </summary>
    [System.Serializable]
    public class UndeadStats
    {
        // Identity
        public string undeadName;
        public UndeadType type;

        // Core Combat Stats
        public int currentHP;
        public int maxHP;
        public int attack;
        public int defense;
        public int speed;
        public int accuracy;

        // Stat Stage Modifiers (changed by buff/debuff moves)
        public int attackStage = 0;    // -6 to +6
        public int defenseStage = 0;   // -6 to +6
        public int speedStage = 0;     // -6 to +6
        public int accuracyStage = 0;  // -6 to +6

        // Derived Properties
        /// <summary>
        /// Is this undead still alive (has HP remaining)?
        /// </summary>
        public bool IsAlive => currentHP > 0;

        /// <summary>
        /// Current HP as a percentage (0.0 to 1.0)
        /// </summary>
        public float HPPercent => maxHP > 0 ? (float)currentHP / maxHP : 0f;

        /// <summary>
        /// Constructor - creates runtime stats from UndeadData
        /// </summary>
        public UndeadStats(UndeadData data)
        {
            this.undeadName = data.undeadName;
            this.type = data.type;
            this.maxHP = data.baseHP;
            this.currentHP = data.baseHP;
            this.attack = data.baseAttack;
            this.defense = data.baseDefense;
            this.speed = data.baseSpeed;
            this.accuracy = data.baseAccuracy;
        }

        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public UndeadStats()
        {
            undeadName = "Unknown";
            type = UndeadType.Bone;
            maxHP = 100;
            currentHP = 100;
            attack = 10;
            defense = 10;
            speed = 10;
            accuracy = 95;
        }

        /// <summary>
        /// Get the actual attack stat after applying stage modifiers
        /// </summary>
        public int GetModifiedAttack()
        {
            return Mathf.RoundToInt(attack * GetStageMultiplier(attackStage));
        }

        /// <summary>
        /// Get the actual defense stat after applying stage modifiers
        /// </summary>
        public int GetModifiedDefense()
        {
            return Mathf.RoundToInt(defense * GetStageMultiplier(defenseStage));
        }

        /// <summary>
        /// Get the actual speed stat after applying stage modifiers
        /// </summary>
        public int GetModifiedSpeed()
        {
            return Mathf.RoundToInt(speed * GetStageMultiplier(speedStage));
        }

        /// <summary>
        /// Get the actual accuracy stat after applying stage modifiers
        /// </summary>
        public int GetModifiedAccuracy()
        {
            return Mathf.Clamp(Mathf.RoundToInt(accuracy * GetStageMultiplier(accuracyStage)), 0, 100);
        }

        /// <summary>
        /// Converts stat stage (-6 to +6) to multiplier
        /// Pokemon-style: -6 = 0.25x, 0 = 1.0x, +6 = 4.0x
        /// </summary>
        private float GetStageMultiplier(int stage)
        {
            stage = Mathf.Clamp(stage, -6, 6);
            
            if (stage >= 0)
            {
                return (2f + stage) / 2f;
            }
            else
            {
                return 2f / (2f - stage);
            }
        }

        /// <summary>
        /// Modify a stat stage by amount (buffs/debuffs)
        /// </summary>
        public void ModifyStatStage(StatType stat, int amount)
        {
            switch (stat)
            {
                case StatType.Attack:
                    attackStage = Mathf.Clamp(attackStage + amount, -6, 6);
                    break;
                case StatType.Defense:
                    defenseStage = Mathf.Clamp(defenseStage + amount, -6, 6);
                    break;
                case StatType.Speed:
                    speedStage = Mathf.Clamp(speedStage + amount, -6, 6);
                    break;
                case StatType.Accuracy:
                    accuracyStage = Mathf.Clamp(accuracyStage + amount, -6, 6);
                    break;
            }
        }

        /// <summary>
        /// Take damage and reduce HP
        /// </summary>
        /// <param name="damage">Amount of damage to take</param>
        /// <returns>Actual damage dealt after being clamped</returns>
        public int TakeDamage(int damage)
        {
            int actualDamage = Mathf.Max(0, damage);
            currentHP = Mathf.Max(0, currentHP - actualDamage);
            return actualDamage;
        }

        /// <summary>
        /// Heal HP (cannot exceed maxHP)
        /// </summary>
        /// <param name="amount">Amount to heal</param>
        /// <returns>Actual healing done</returns>
        public int Heal(int amount)
        {
            int oldHP = currentHP;
            currentHP = Mathf.Min(maxHP, currentHP + amount);
            return currentHP - oldHP;
        }

        /// <summary>
        /// Restore to full HP
        /// </summary>
        public void FullHeal()
        {
            currentHP = maxHP;
        }

        /// <summary>
        /// Returns a formatted string of all stats for debugging
        /// </summary>
        public override string ToString()
        {
            return $"{undeadName} ({type})\n" +
                   $"HP: {currentHP}/{maxHP}\n" +
                   $"ATK: {attack} | DEF: {defense} | SPD: {speed} | ACC: {accuracy}%";
        }
    }
}