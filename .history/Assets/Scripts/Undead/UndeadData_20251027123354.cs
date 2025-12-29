using UnityEngine;
using System.Collections.Generic;

namespace NecromancersRising.Undead
{
    /// <summary>
    /// ScriptableObject defining an undead creature's base stats and data.
    /// MINIMAL CHANGE VERSION - Preserves serialization from original script
    /// </summary>
    [CreateAssetMenu(fileName = "New Undead", menuName = "Necromancer's Rising/Undead Data")]
    public class UndeadData : ScriptableObject
    {
        [Header("Basic Info")]
        public string undeadName = "Unnamed Undead";
        public Combat.UndeadType type = Combat.UndeadType.Bone;
        
        [TextArea(3, 5)]
        public string description = "A reanimated creature.";

        [Header("Base Stats")]
        [Range(1, 500)]
        public int baseHP = 100;
        
        [Range(1, 100)]
        public int baseAttack = 10;
        
        [Range(1, 100)]
        public int baseDefense = 10;
        
        [Range(1, 100)]
        public int baseSpeed = 10;
        
        [Range(1, 100)]
        public int baseAccuracy = 95;

        [Header("Moves")]
        public List<Combat.MoveData> moves = new List<Combat.MoveData>();

        [Header("Evolution/Crafting (Future)")]
        public bool canEvolve = false;
        public int evolveLevel = 0;

        [Header("Visual (Future)")]
        public Sprite sprite;

        /// <summary>
        /// Create runtime combat stats from this data
        /// </summary>
        public UndeadStats CreateStats()
        {
            return new UndeadStats(this);
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

            // Initialize moves list if null (compatibility fix)
            if (moves == null)
            {
                moves = new List<Combat.MoveData>();
            }

            // Limit to 4 moves
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