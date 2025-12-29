using System.Collections.Generic;
using UnityEngine;

namespace NecromancersRising.Combat
{
    /// <summary>
    /// Manages type effectiveness calculations for the combat system.
    /// Bone type = "Normal" type (neutral against most, cannot hit Spirit)
    /// Uses 1.5x for super effective, 0.67x for not very effective, 0x for immunity
    /// </summary>
    public class TypeChart : MonoBehaviour
    {
        public static TypeChart Instance { get; private set; }

        // Type effectiveness multipliers
        private const float SUPER_EFFECTIVE = 1.5f;
        private const float NOT_VERY_EFFECTIVE = 0.67f;
        private const float IMMUNE = 0f;
        private const float NEUTRAL = 1f;

        // Type effectiveness lookup table
        // Dictionary<AttackingType, Dictionary<DefendingType, Multiplier>>
        private Dictionary<UndeadType, Dictionary<UndeadType, float>> typeChart;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeTypeChart();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Initialize the type effectiveness chart
        /// </summary>
        private void InitializeTypeChart()
        {
            typeChart = new Dictionary<UndeadType, Dictionary<UndeadType, float>>();

            // BONE TYPE - The "Normal" type
            // Neutral against everything except:
            // - Cannot hit Spirit (0x)
            // - Weak when defending vs Crushing (takes 1.5x)
            typeChart[UndeadType.Bone] = new Dictionary<UndeadType, float>
            {
                { UndeadType.Bone, NEUTRAL },
                { UndeadType.Plague, NEUTRAL },
                { UndeadType.Feral, NEUTRAL },
                { UndeadType.Spirit, IMMUNE },      // Cannot hit ghosts
                { UndeadType.Dark, NEUTRAL },
                { UndeadType.Fire, NEUTRAL },
                { UndeadType.Living, NEUTRAL },
                { UndeadType.Holy, NEUTRAL },
                { UndeadType.Crushing, NEUTRAL }
            };

            // PLAGUE TYPE - Disease/Corruption
            // Strong vs: Bone (infects physical), Living (spreads disease)
            // Weak vs: Fire (burns away disease), Holy (purifies)
            typeChart[UndeadType.Plague] = new Dictionary<UndeadType, float>
            {
                { UndeadType.Bone, SUPER_EFFECTIVE },      // Infects skeleton
                { UndeadType.Plague, NOT_VERY_EFFECTIVE }, // Plague immune to plague
                { UndeadType.Feral, NEUTRAL },
                { UndeadType.Spirit, NEUTRAL },
                { UndeadType.Dark, NEUTRAL },
                { UndeadType.Fire, NOT_VERY_EFFECTIVE },   // Fire purifies
                { UndeadType.Living, SUPER_EFFECTIVE },    // Disease spreads
                { UndeadType.Holy, NOT_VERY_EFFECTIVE },   // Holy purifies
                { UndeadType.Crushing, NEUTRAL }
            };

            // FERAL TYPE - Beast/Predator
            // Strong vs: Plague (predators resist disease), Living (hunt prey)
            // Weak vs: Fire (animals fear fire), Crushing (beaten by armor)
            typeChart[UndeadType.Feral] = new Dictionary<UndeadType, float>
            {
                { UndeadType.Bone, NEUTRAL },
                { UndeadType.Plague, SUPER_EFFECTIVE },    // Predators resist disease
                { UndeadType.Feral, NEUTRAL },
                { UndeadType.Spirit, NEUTRAL },
                { UndeadType.Dark, NEUTRAL },
                { UndeadType.Fire, NOT_VERY_EFFECTIVE },   // Fear of fire
                { UndeadType.Living, SUPER_EFFECTIVE },    // Hunt living prey
                { UndeadType.Holy, NEUTRAL },
                { UndeadType.Crushing, NOT_VERY_EFFECTIVE } // Weak to heavy blows
            };

            // SPIRIT TYPE - Intangible Ghost
            // Strong vs: Crushing (ghosts dodge physical), Dark (counter shadow magic)
            // Weak vs: Dark (shadow magic), Holy (light banishes)
            // Immune to: Bone (physical cannot hit)
            typeChart[UndeadType.Spirit] = new Dictionary<UndeadType, float>
            {
                { UndeadType.Bone, IMMUNE },               // Physical can't hit
                { UndeadType.Plague, NEUTRAL },
                { UndeadType.Feral, NEUTRAL },
                { UndeadType.Spirit, NEUTRAL },
                { UndeadType.Dark, SUPER_EFFECTIVE },      // Light vs shadow
                { UndeadType.Fire, NEUTRAL },
                { UndeadType.Living, NEUTRAL },
                { UndeadType.Holy, NOT_VERY_EFFECTIVE },   // Holy banishes
                { UndeadType.Crushing, SUPER_EFFECTIVE }   // Ghosts dodge physical
            };

            // DARK TYPE - Shadow Magic
            // Strong vs: Spirit (shadow magic hits ghosts), Living (corrupts mortals)
            // Weak vs: Holy (light purifies darkness)
            typeChart[UndeadType.Dark] = new Dictionary<UndeadType, float>
            {
                { UndeadType.Bone, NEUTRAL },
                { UndeadType.Plague, NEUTRAL },
                { UndeadType.Feral, NEUTRAL },
                { UndeadType.Spirit, SUPER_EFFECTIVE },    // Shadow hits ghosts
                { UndeadType.Dark, NOT_VERY_EFFECTIVE },   // Dark resists dark
                { UndeadType.Fire, NEUTRAL },
                { UndeadType.Living, SUPER_EFFECTIVE },    // Corrupts mortals
                { UndeadType.Holy, NOT_VERY_EFFECTIVE },   // Light beats shadow
                { UndeadType.Crushing, NEUTRAL }
            };

            // FIRE TYPE - Elemental Flame
            // Strong vs: Plague (burns disease), Feral (animals fear fire)
            // Weak vs: Bone (no flesh to burn)
            typeChart[UndeadType.Fire] = new Dictionary<UndeadType, float>
            {
                { UndeadType.Bone, NOT_VERY_EFFECTIVE },   // Bones don't burn well
                { UndeadType.Plague, SUPER_EFFECTIVE },    // Burns away disease
                { UndeadType.Feral, SUPER_EFFECTIVE },     // Flesh burns
                { UndeadType.Spirit, NEUTRAL },
                { UndeadType.Dark, NEUTRAL },
                { UndeadType.Fire, NOT_VERY_EFFECTIVE },   // Fire resists fire
                { UndeadType.Living, NEUTRAL },
                { UndeadType.Holy, NEUTRAL },
                { UndeadType.Crushing, NEUTRAL }
            };

            // LIVING TYPE - Humans/Animals (Generic enemy type)
            // Weak vs: All undead types (baseline difficulty)
            typeChart[UndeadType.Living] = new Dictionary<UndeadType, float>
            {
                { UndeadType.Bone, NEUTRAL },
                { UndeadType.Plague, NEUTRAL },
                { UndeadType.Feral, NEUTRAL },
                { UndeadType.Spirit, NEUTRAL },
                { UndeadType.Dark, NEUTRAL },
                { UndeadType.Fire, NEUTRAL },
                { UndeadType.Living, NEUTRAL },
                { UndeadType.Holy, NEUTRAL },
                { UndeadType.Crushing, NEUTRAL }
            };

            // HOLY TYPE - Priests/Paladins
            // Strong vs: Dark (light banishes shadow), Plague (purifies disease)
            // Weak vs: Bone (physical skeletons resist holy), Feral (beasts resist)
            typeChart[UndeadType.Holy] = new Dictionary<UndeadType, float>
            {
                { UndeadType.Bone, NOT_VERY_EFFECTIVE },   // Skeletons resist holy
                { UndeadType.Plague, SUPER_EFFECTIVE },    // Purifies disease
                { UndeadType.Feral, NOT_VERY_EFFECTIVE },  // Beasts resist
                { UndeadType.Spirit, SUPER_EFFECTIVE },    // Light banishes ghosts
                { UndeadType.Dark, SUPER_EFFECTIVE },      // Holy vs unholy
                { UndeadType.Fire, NEUTRAL },
                { UndeadType.Living, NEUTRAL },
                { UndeadType.Holy, NOT_VERY_EFFECTIVE },   // Holy resists holy
                { UndeadType.Crushing, NEUTRAL }
            };

            // CRUSHING TYPE - Knights/Martial fighters
            // Strong vs: Bone (smashes skeletons)
            // Weak vs: Spirit (can't hit ghosts), Feral (fast dodgers), Dark (magic)
            typeChart[UndeadType.Crushing] = new Dictionary<UndeadType, float>
            {
                { UndeadType.Bone, SUPER_EFFECTIVE },      // Smashes bones
                { UndeadType.Plague, NEUTRAL },
                { UndeadType.Feral, NOT_VERY_EFFECTIVE },  // Fast targets dodge
                { UndeadType.Spirit, IMMUNE },             // Can't hit ghosts
                { UndeadType.Dark, NOT_VERY_EFFECTIVE },   // Magic resists physical
                { UndeadType.Fire, NEUTRAL },
                { UndeadType.Living, NEUTRAL },
                { UndeadType.Holy, NEUTRAL },
                { UndeadType.Crushing, NEUTRAL }
            };
        }

        /// <summary>
        /// Get the type effectiveness multiplier for an attack
        /// </summary>
        /// <param name="attackingType">The type of the move being used</param>
        /// <param name="defendingType">The type of the defending undead</param>
        /// <returns>Damage multiplier (0x, 0.67x, 1x, or 1.5x)</returns>
        public float GetEffectiveness(UndeadType attackingType, UndeadType defendingType)
        {
            if (typeChart.ContainsKey(attackingType) && typeChart[attackingType].ContainsKey(defendingType))
            {
                return typeChart[attackingType][defendingType];
            }

            // Default to neutral if matchup not found
            Debug.LogWarning($"Type matchup not found: {attackingType} vs {defendingType}. Defaulting to neutral.");
            return NEUTRAL;
        }

        /// <summary>
        /// Get a text description of the effectiveness
        /// </summary>
        public string GetEffectivenessText(float multiplier)
        {
            if (multiplier == IMMUNE) return "It has no effect!";
            if (multiplier >= SUPER_EFFECTIVE) return "It's super effective!";
            if (multiplier <= NOT_VERY_EFFECTIVE) return "It's not very effective...";
            return "";
        }

        /// <summary>
        /// Check if an attack will hit (not immune)
        /// </summary>
        public bool CanHit(UndeadType attackingType, UndeadType defendingType)
        {
            return GetEffectiveness(attackingType, defendingType) > 0f;
        }
    }

    /// <summary>
    /// All types in the game (undead + enemy types)
    /// </summary>
    public enum UndeadType
    {
        // Core Undead Types (playable)
        Bone,      // Normal type - neutral, can't hit Spirit
        Plague,    // Disease type - strong vs Bone/Living
        Feral,     // Beast type - strong vs Plague/Living
        Spirit,    // Ghost type - immune to Bone/Crushing
        Dark,      // Shadow magic - strong vs Spirit/Living
        Fire,      // Elemental - strong vs Plague/Feral

        // Enemy/Living Types
        Living,    // Generic humans/animals
        Holy,      // Priests/paladins - strong vs Dark/Plague
        Crushing   // Knights/martial - strong vs Bone
    }
}