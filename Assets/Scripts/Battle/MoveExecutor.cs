using UnityEngine;
using NecromancersRising.Combat; // Fixed namespace

namespace NecromancersRising.Battle
{
    public class MoveExecutor : IMove
    {
        private readonly MoveData _moveData;

        public string MoveName => _moveData.moveName;
        public int SPCost => _moveData.maxActionPoints;
        public TargetType Target => _moveData.target == MoveTarget.Enemy ? TargetType.Enemy : TargetType.Ally;

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

            user.ConsumeSP(SPCost);

            switch (_moveData.effectType)
            {
                case MoveEffectType.Damage:
                    ExecuteDamageMove(user, target);
                    break;
                case MoveEffectType.BuffStat:
                    ExecuteBuffMove(user, target);
                    break;
                case MoveEffectType.DebuffStat:
                    ExecuteDebuffMove(user, target);
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
            
            DamageType damageType = DamageType.Physical; // Can refine this later
            target.TakeDamage(damage, damageType);
            
            Debug.Log($"{user.EntityName} used {MoveName} on {target.EntityName} for {damage} damage!");
        }

        private void ExecuteBuffMove(IBattleEntity user, IBattleEntity target)
        {
            Debug.Log($"{user.EntityName} used {MoveName} on {target.EntityName}, buffing {_moveData.statModified}!");
            // TODO: Implement stat buffs
        }

        private void ExecuteDebuffMove(IBattleEntity user, IBattleEntity target)
        {
            Debug.Log($"{user.EntityName} used {MoveName} on {target.EntityName}, debuffing {_moveData.statModified}!");
            // TODO: Implement stat debuffs
        }

        private int CalculateDamage(IBattleEntity user, IBattleEntity target)
        {
            int baseDamage = _moveData.basePower;
            float randomFactor = Random.Range(0.9f, 1.1f);
            
            return Mathf.Max(1, Mathf.RoundToInt(baseDamage * randomFactor));
        }
    }
}