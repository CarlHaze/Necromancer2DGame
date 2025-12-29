using UnityEngine;
using System.Collections.Generic;

namespace NecromancersRising.Undead
{
    /// <summary>
    /// ScriptableObject defining an undead creature's base stats and data.
    /// This is the template - actual combat stats are created in UndeadStats.
    /// </summary>
    [CreateAssetMenu(fileName = "New Undead", menuName = "Necromancer's Rising/Undead Data")]
    public class UndeadData : ScriptableObject
    {
        [Header("Basic Info")]
        [Tooltip("Display name of the undead")]
        public string undeadName = "Unnamed Undead";

        [Tooltip("Type determines defensive resistances/weaknesses")]
        public Combat.UndeadType type = Combat.UndeadType.Bone;

        [Tooltip("Description and lore")]
        [TextArea(3, 5)]
        public string description = "A reanimated creature.";

        [Header("Base Stats")]
        [Tooltip("Maximum hit points")]
        [Range(1, 500)]
        public int baseHP = 100;

        [Tooltip("Physical attack power")]
        [Range(1, 100)]
        public int baseAttack = 10;

        [Tooltip("Physical defense (reduces incoming damage)")]
        [Range(1, 100)]
        public int baseDefense = 10;

        [Tooltip("Speed (determines turn order, higher goes first)")]
        [Range(1, 100)]
        public int baseSpeed = 10;

        [Tooltip("Accuracy percentage (chance to hit)")]
        [Range(1, 100)]
        public int baseAccuracy = 95;

        [Header("Moves")]
        [Tooltip("List of moves this undead knows (max 4)")]
        public List<Combat.MoveData> moves = new List<Combat.MoveData>();

        [Header("Evolution/Crafting (Future)")]
        [Tooltip("Can this undead evolve?")]
        public bool canEvolve = false;

        [Tooltip("Level required to evolve")]
        public int evolveLevel = 0;

        // TODO: Add evolution paths, part requirements, etc.

        [Header("Visual (Future)")]
        [Tooltip("Sprite for battle and overworld")]
        public Sprite sprite;

        /// <summary>
        /// Create runtime combat stats from this data
        /// </summary>
        public UndeadStats CreateStats()
        {
            return new UndeadStats(this);
        }

        /// <summary>
        /// Get a summary of this undead's stats
        /// </summary>
        public string GetStatSummary()
        {
            string summary = $"{undeadName} ({type})\n";
            summary += $"HP: {baseHP} | ATK: {baseAttack} | DEF: {baseDefense}\n";
            summary += $"SPD: {baseSpeed} | ACC: {baseAccuracy}%\n";
            summary += $"Moves: {moves.Count}/4";
            return summary;
        }

        /// <summary>
        /// Validate data in the Unity Inspector
        /// </summary>
        private void OnValidate()
        {
            // Ensure stats are positive
            baseHP = Mathf.Max(1, baseHP);
            baseAttack = Mathf.Max(1, baseAttack);
            baseDefense = Mathf.Max(1, baseDefense);
            baseSpeed = Mathf.Max(1, baseSpeed);
            baseAccuracy = Mathf.Clamp(baseAccuracy, 1, 100);

            // Limit to 4 moves (Pokemon standard)
            if (moves.Count > 4)
            {
                Debug.LogWarning($"{undeadName} has more than 4 moves! Limiting to first 4.");
                moves = moves.GetRange(0, 4);
            }

            // Remove null moves
            moves.RemoveAll(m => m == null);
        }
    }
}