namespace NecromancersRising.Battle
{
    public interface IStatusEffect
    {
        string StatusName { get; }
        int Duration { get; }
        UnityEngine.Sprite Icon { get; }
        
        void Apply(IBattleEntity target);
        void OnTurnStart(IBattleEntity target);
        void OnTurnEnd(IBattleEntity target);
        void Remove(IBattleEntity target);
    }
}