using UnityEngine;
using NecromancersRising.Combat;

namespace NecromancersRising.Undead
{
    /// <summary>
    /// ScriptableObject that defines base stats and properties for an undead type
    /// Create instances in Unity Editor via: Create > Necromancer > Undead Data
    /// </summary>
    [CreateAssetMenu(fileName = "New Undead", menuName = "Necromancer/Undead Data")]
    public class UndeadData : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("The display name of this undead")]
        public string undeadName = "Unknown Undead";

        [Tooltip("The type determines combat effectiveness")]
        public UndeadType type = UndeadType.Bone;

        [TextArea(3, 5)]
        [Tooltip("Description shown to player")]
        public string description = "";

        [Header("Base Stats")]
        [Tooltip("Base maximum health points")]
        public int baseHP = 100;

        [Tooltip("Base physical attack power")]
        public int baseAttack = 10;

        [Tooltip("Base physical defense")]
        public int baseDefense = 10;

        [Tooltip("Base turn order priority (higher = faster)")]
        public int baseSpeed = 10;

        [Tooltip("Base hit chance percentage (0-100, default 95)")]
        [Range(0, 100)]
        public int baseAccuracy = 95;

        [Header("Visual (Future)")]
        [Tooltip("Sprite to display for this undead")]
        public Sprite sprite;

        /// <summary>
        /// Creates a runtime UndeadStats instance from this data
        /// </summary>
        public UndeadStats CreateInstance()
        {
            return new UndeadStats(this);
        }

        /// <summary>
        /// Validation - called when values change in Inspector
        /// </summary>
        private void OnValidate()
        {
            // Ensure stats are never negative
            baseHP = Mathf.Max(1, baseHP);
            baseAttack = Mathf.Max(0, baseAttack);
            baseDefense = Mathf.Max(0, baseDefense);
            baseSpeed = Mathf.Max(0, baseSpeed);
        }
    }
}