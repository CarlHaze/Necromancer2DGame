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
        // We'll add the effectiveness system next
    }
}