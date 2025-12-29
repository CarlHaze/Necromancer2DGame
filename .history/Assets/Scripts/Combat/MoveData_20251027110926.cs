using UnityEngine;

namespace NecromancersRising.Combat
{
    /// <summary>
    /// ScriptableObject that defines a move/ability for undead creatures.
    /// Moves have their own type (independent of the creature using them).
    /// </summary>
    [CreateAssetMenu(fileName = "New Move", menuName = "Necromancer's Rising/Move Data")]
    public class MoveData : ScriptableObject
    {
        [Header("Basic Info")]
        [Tooltip("Display name of the move")]
        public string moveName = "Unnamed Move";

        [Tooltip("Description shown to the player")]
        [TextArea(2, 4)]
        public string description = "A basic attack.";

        [Header("Combat Stats")]
        [Tooltip("Type of this move (determines effectiveness vs defender type)")]
        public UndeadType moveType = UndeadType.Bone;

        [Tooltip("Base power of the move (before type effectiveness)")]
        [Range(0, 200)]
        public int power = 10;

        [Tooltip("Hit chance percentage (1-100)")]
        [Range(1, 100)]
        public int accuracy = 100;

        [Tooltip("Maximum Action Points (AP) - how many times this move can be used")]
        [Range(1, 50)]
        public int maxActionPoints = 35;

        [Header("Special Effects")]
        [Tooltip("Does this move have a guaranteed hit (ignores accuracy)?")]
        public bool neverMisses = false;

        [Tooltip("Does this move bypass type immunities?")]
        public bool ignoresImmunity = false;

        [Header("Status Effects (Future)")]
        [Tooltip("Chance to inflict status effect (0-100)")]
        [Range(0, 100)]
        public int statusEffectChance = 0;

        // TODO: Add status effect enum when implemented
        // public StatusEffect statusEffect = StatusEffect.None;

        [Header("Stat Modifications (Future)")]
        [Tooltip("Does this move modify stats instead of dealing damage?")]
        public bool isStatMove = false;

        // TODO: Add stat modification data when implemented

        /// <summary>
        /// Get a formatted description of the move's properties
        /// </summary>
        public string GetFullDescription()
        {
            string desc = $"{moveName}\n";
            desc += $"Type: {moveType}\n";
            desc += $"Power: {power} | Accuracy: {accuracy}%\n";
            desc += $"AP: {maxActionPoints}\n";
            desc += $"\n{description}";
            return desc;
        }

        /// <summary>
        /// Validate move data in the Unity Inspector
        /// </summary>
        private void OnValidate()
        {
            // Ensure values stay within reasonable ranges
            power = Mathf.Max(0, power);
            accuracy = Mathf.Clamp(accuracy, 1, 100);
            maxActionPoints = Mathf.Max(1, maxActionPoints);
            statusEffectChance = Mathf.Clamp(statusEffectChance, 0, 100);
        }
    }
}