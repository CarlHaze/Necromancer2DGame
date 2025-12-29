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
            //check for super effective matchups
            if (IsSuperEffective(attackerType, defenderType))
            {
                return SUPER_EFFECTIVE;
            }

            // Check for not very effective matchups
            if (IsNotEffective(attackerType, defenderType))
            {
                return NOT_EFFECTIVE;
            }

            // Default to neutral damage
            return NEUTRAL;
        }

        /// <summary>
        /// Checks if the attacker type is super effective against defender type
        /// Based on the design document type chart
        /// </summary>
        private bool IsSuperEffective(UndeadType attacker, UndeadType defender)
        {
            switch (attacker)
            {
                case UndeadType.Bone:
                // Bone has no super effective matchups listed
                return false;

                case UndeadType.Plague:
                    // Plague has no super effective matchups listed
                    return false;

                case UndeadType.Feral:
                    // Feral has high attack but no specific super effective types listed
                    return false;

                case UndeadType.Spirit:
                    // Spirit is immune to physical, but no super effective matchups listed
                    return false;

                case UndeadType.Dark:
                    // Dark has life drain but no specific super effective matchups listed
                    return false;

                case UndeadType.Hex:
                    // Hex has powerful debuffs but no specific super effective matchups listed
                    return false;

                case UndeadType.Infernal:
                    return defender == UndeadType.Frost; // Fire beats Ice

                case UndeadType.Frost:
                    return defender == UndeadType.Plague; // Cold slows disease

                case UndeadType.Blight:
                    // Blight has poison but no specific super effective matchups listed
                    return false;

                case UndeadType.Soul:
                    return defender == UndeadType.Dark || defender == UndeadType.Spirit;

                default:
                    return false;
                    }
        }
        
         private bool IsNotEffective()
        {
            
        }



    }
}