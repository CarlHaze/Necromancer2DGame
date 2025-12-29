using UnityEngine;
using System.Collections.Generic;

namespace NecromancersRising.Undead
{
    [CreateAssetMenu(fileName = "New Undead", menuName = "Necromancer/Undead Data")]
    public class UndeadData : ScriptableObject
    {
        // Basic Info
        public string undeadName = "Unnamed Undead";
        public Combat.UndeadType type = Combat.UndeadType.Bone;
        public string description = "";

        // Stats
        public int baseHP = 100;
        public int baseAttack = 10;
        public int baseDefense = 10;
        public int baseSpeed = 10;
        public int baseAccuracy = 95;

        // MOVES - NO FANCY STUFF
        public List<Combat.MoveData> moves;

        // Evolution
        public bool canEvolve = false;
        public int evolveLevel = 0;

        // Visual
        public Sprite sprite;

        // Create stats
        public UndeadStats CreateStats()
        {
            return new UndeadStats(this);
        }

        // Force initialize moves on enable
        void OnEnable()
        {
            if (moves == null)
            {
                moves = new List<Combat.MoveData>();
                Debug.Log($"Initialized moves list for {name}");
            }
        }

        // Force initialize moves on validate
        void OnValidate()
        {
            if (moves == null)
            {
                moves = new List<Combat.MoveData>();
                Debug.Log($"Validated moves list for {name}");
            }
        }
    }
}