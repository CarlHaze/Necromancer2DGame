using UnityEngine;
using NecromancersRising.Data;

namespace NecromancersRising.Battle
{
    public class MoveExecutor : IMove
    {
        private readonly MoveData _moveData;

        public string MoveName => _moveData.MoveName;
        public int SPCost => _moveData.MaxActionPoints; // Using action points as SP cost
        public TargetType Target => _moveData.Target == MoveTarget.Enemy ? TargetType.Enemy : TargetType.Ally;

        public MoveExecutor(MoveData moveData)
        {
            _moveData = moveData;
        }

        public bool CanExecute(IBattleEntity user)
        {
            return user.CurrentSP >= SPCost && !user.IsDead;
        }

        public void Execute(IBattleEntity user, IBattleEntity target)
        {
            if (!CanExecute(user))
            {
                Debug.LogWarning($"{user.EntityName} cannot execute {MoveName}");
                return;
            }

            // Consume SP
            user.ConsumeSP(SPCost);

            // Execute move based on effect type
            switch (_moveData.EffectType)
            {
                case MoveEffectType.Damage:
                    ExecuteDamageMove(user, target);
                    break;
                case MoveEffectType.Heal:
                    ExecuteHealMove(user, target);
                    break;
                case MoveEffectType.StatChange:
                    ExecuteStatChangeMove(user, target);
                    break;
            }
        }

        public void Execute(IBattleEntity user, IBattleEntity[] targets)
        {
            foreach (var target in targets)
            {
                Execute(user, target);
            }
        }

        private void ExecuteDamageMove(IBattleEntity user, IBattleEntity target)
        {
            int damage = CalculateDamage(user, target);
            
            DamageType damageType = _moveData.MoveType == MoveType.Bone || _moveData.MoveType == MoveType.Feral 
                ? DamageType.Physical 
                : DamageType.Magical;

            target.TakeDamage(damage, damageType);
            
            Debug.Log($"{user.EntityName} used {MoveName} on {target.EntityName} for {damage} damage!");
        }

        private void ExecuteHealMove(IBattleEntity user, IBattleEntity target)
        {
            int healAmount = _moveData.BasePower;
            target.Heal(healAmount);
            
            Debug.Log($"{user.EntityName} used {MoveName} on {target.EntityName}, healing {healAmount} HP!");
        }

        private void ExecuteStatChangeMove(IBattleEntity user, IBattleEntity target)
        {
            // For now, just apply a status effect based on the stat modified
            Debug.Log($"{user.EntityName} used {MoveName} on {target.EntityName}, modifying {_moveData.StatModified}!");
            // TODO: Implement status effects
        }

        private int CalculateDamage(IBattleEntity user, IBattleEntity target)
        {
            // Simple damage calculation
            // TODO: Factor in user's attack stat, target's defense, type effectiveness
            int baseDamage = _moveData.BasePower;
            
            // Add some randomness (90-110%)
            float randomFactor = Random.Range(0.9f, 1.1f);
            
            return Mathf.Max(1, Mathf.RoundToInt(baseDamage * randomFactor));
        }
    }
}