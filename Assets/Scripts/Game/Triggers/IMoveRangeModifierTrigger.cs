using System;

namespace Game.Triggers
{
    public interface IMoveRangeModifierTrigger : IComparable<IMoveRangeModifierTrigger>
    {
        int IComparable<IMoveRangeModifierTrigger>.CompareTo(IMoveRangeModifierTrigger other)
        {
            return 0;
        }

        public int ModifyMoveRange(int baseRange);
    }

    public interface IAttackRangeModifier : IComparable<IAttackRangeModifier>
    {
        int IComparable<IAttackRangeModifier>.CompareTo(IAttackRangeModifier other)
        {
            return 0;
        }

        public int ModifyAttackRange(int baseRange);
    }
}