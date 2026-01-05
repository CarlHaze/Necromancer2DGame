using UnityEngine;

namespace NecromancersRising.Battle
{
    public interface IBattleEntity
    {
        string EntityName { get; }
        int CurrentHP { get; }
        int MaxHP { get; }
        int CurrentSP { get; }
        int MaxSP { get; }
        bool IsDead { get; }
        
        void TakeDamage(int amount, DamageType damageType);
        void Heal(int amount);
        void RestoreSP(int amount);
        void ConsumeSP(int amount);
        void ApplyStatusEffect(IStatusEffect statusEffect);
        void RemoveStatusEffect(string effectName);
    }

    public enum DamageType
    {
        Physical,
        Magical,
        True // Ignores defense
    }
}