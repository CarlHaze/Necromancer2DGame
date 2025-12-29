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