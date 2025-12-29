using UnityEngine;
using NecromancersRising.Combat;

namespace NecromancersRising.Undead
{
    /// <summary>
    /// Defines the core stats for an undead creature
    /// Used for combat calculations and progression
    /// </summary>
    [System.Serializable]
    public class UndeadStats
    {
        // Identity
        [Header("Identity")]
        [Tooltip("The name of this undead")]
        public string undeadName;

        [Tooltip("The type determines combat effectiveness")]
        public UndeadType type;

        // Core Combat Stats
        [Header("Core Stats")]
        [Tooltip("Current health points")]
        public int currentHP;

        [Tooltip("Maximum health points")]
        public int maxHP;

        [Tooltip("Physical attack power")]
        public int attack;

        [Tooltip("Physical defense")]
        public int defense;

        [Tooltip("Turn order priority (higher = faster)")]
        public int speed;

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
        /// Constructor for creating new undead with stats
        /// </summary>
        public UndeadStats(string name, UndeadType type, int hp, int atk, int def, int spd)
        {
            this.undeadName = name;
            this.type = type;
            this.maxHP = hp;
            this.currentHP = hp;
            this.attack = atk;
            this.defense = def;
            this.speed = spd;
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
                   $"ATK: {attack} | DEF: {defense} | SPD: {speed}";
        }
    }
}