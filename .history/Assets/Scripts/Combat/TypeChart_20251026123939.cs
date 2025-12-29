using UnityEngine;

namespace NecromancersRising.Combat
{
    /// <summary>
    /// The 10 undead types in the game
    /// </summary>
    public enum UndeadType
    {
        Bone,      // Basic undead, physically durable
        Plague,    // Disease carriers
        Feral,     // Fast, hunger-driven predators
        Spirit,    // Intangible soul energy
        Dark,      // Forbidden magic users
        Hex,       // Curse-bound undead
        Infernal,  // Demonic fire undead
        Frost,     // Ice-preserved undead
        Blight,    // Nature's decay incarnate
        Soul       // Psychic energy manipulators
    }

    /// <summary>
    /// Handles type effectiveness calculations for combat
    /// </summary>
    public class TypeChart : MonoBehaviour
    {
        // Singleton instance for easy access
        public static TypeChart Instance { get; private set; }

        // Effectiveness multipliers
        private const float SUPER_EFFECTIVE = 2.0f;
        private const float NOT_EFFECTIVE = 0.5f;
        private const float NEUTRAL = 1.0f;
        private const float IMMUNE = 0.0f;

        private void Awake()
        {
            // Singleton pattern - only one TypeChart should exist
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Gets the damage multiplier when attacker hits defender
        /// </summary>
        /// <param name="attackerType">The type of the attacking undead</param>
        /// <param name="defenderType">The type of the defending undead</param>
        /// <returns>Damage multiplier (2.0 = super effective, 0.5 = not effective, 1.0 = neutral)</returns>
        public float GetEffectiveness(UndeadType attackerType, UndeadType defenderType)
        {
            // We'll implement the actual chart next
            return NEUTRAL;
        }
    }
}