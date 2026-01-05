namespace NecromancersRising.Battle
{
    public interface IMove
    {
        string MoveName { get; }
        int SPCost { get; }
        TargetType Target { get; }
        
        bool CanExecute(IBattleEntity user);
        void Execute(IBattleEntity user, IBattleEntity target);
        void Execute(IBattleEntity user, IBattleEntity[] targets);
    }

    public enum TargetType
    {
        Enemy,
        Ally,
        Self,
        AllEnemies,
        AllAllies,
        All
    }
}