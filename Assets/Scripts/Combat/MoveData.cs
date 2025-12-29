using UnityEngine;

namespace NecromancersRising.Combat
{
    /// <summary>
    /// Defines the type of move effect
    /// </summary>
    public enum MoveEffectType
    {
        Damage,        // Deals damage to target
        BuffStat,      // Increases a stat
        DebuffStat     // Decreases a stat
    }

    /// <summary>
    /// Which stat can be modified by moves
    /// </summary>
    public enum StatType
    {
        Attack,
        Defense,
        Speed,
        Accuracy
    }

    /// <summary>
    /// Who the move targets
    /// </summary>
    public enum MoveTarget
    {
        Enemy,   // Targets opponent
        Self     // Targets user
    }

    /// <summary>
    /// ScriptableObject that defines a move/ability
    /// UPDATED: Now includes Action Points (AP) system
    /// </summary>
    [CreateAssetMenu(fileName = "New Move", menuName = "Necromancer/Move Data")]
    public class MoveData : ScriptableObject
    {
        [Header("Basic Info")]
        [Tooltip("The name of this move")]
        public string moveName = "Unknown Move";

        [Tooltip("The type of this move (determines effectiveness)")]
        public UndeadType moveType = UndeadType.Bone;

        [TextArea(2, 4)]
        [Tooltip("Description of what the move does")]
        public string description = "";

        [Header("Combat Properties")]
        [Tooltip("Base power of the move (0 for non-damaging moves)")]
        [Range(0, 200)]
        public int basePower = 40;

        [Tooltip("Hit chance percentage")]
        [Range(0, 100)]
        public int accuracy = 100;

        [Tooltip("Who this move targets")]
        public MoveTarget target = MoveTarget.Enemy;

        [Header("Action Points (NEW)")]
        [Tooltip("Maximum Action Points - how many times this move can be used")]
        [Range(1, 50)]
        public int maxActionPoints = 35;

        [Header("Special Properties")]
        [Tooltip("Does this move always hit (ignores accuracy)?")]
        public bool neverMisses = false;

        [Tooltip("Does this move bypass type immunities?")]
        public bool ignoresImmunity = false;

        [Header("Move Effect")]
        [Tooltip("What does this move do?")]
        public MoveEffectType effectType = MoveEffectType.Damage;

        [Tooltip("Which stat is modified (only for buff/debuff moves)")]
        public StatType statModified = StatType.Attack;

        [Tooltip("Amount to modify stat by (e.g., -1 for debuff, +1 for buff)")]
        [Range(-3, 3)]
        public int statChangeAmount = 0;

        // Compatibility properties (so both old and new code works)
        /// <summary>
        /// Alias for basePower (for new code that expects "power")
        /// </summary>
        public int power => basePower;

        /// <summary>
        /// Is this a damaging move?
        /// </summary>
        public bool IsDamaging => effectType == MoveEffectType.Damage;

        /// <summary>
        /// Is this a stat-modifying move?
        /// </summary>
        public bool IsStatMove => effectType == MoveEffectType.BuffStat || effectType == MoveEffectType.DebuffStat;

        /// <summary>
        /// Validation - called when values change in Inspector
        /// </summary>
        private void OnValidate()
        {
            // Ensure stats are in valid ranges
            basePower = Mathf.Clamp(basePower, 0, 200);
            accuracy = Mathf.Clamp(accuracy, 0, 100);
            maxActionPoints = Mathf.Max(1, maxActionPoints);

            // If it's a damage move, ensure base power > 0
            if (effectType == MoveEffectType.Damage && basePower == 0)
            {
                basePower = 40;
            }

            // If it's a stat move, ensure stat change is not 0
            if (IsStatMove && statChangeAmount == 0)
            {
                statChangeAmount = effectType == MoveEffectType.BuffStat ? 1 : -1;
            }
        }
    }
}