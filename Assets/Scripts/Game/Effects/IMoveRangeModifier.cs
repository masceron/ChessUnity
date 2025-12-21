namespace Game.Effects
{
    public interface IMoveRangeModifier
    {
        public int ModifyMoveRange(int baseRange);
    }

    public interface IAttackRangeModifier
    {
        public int ModifyAttackRange(int baseRange);
    }
}

