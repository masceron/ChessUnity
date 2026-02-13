using System;

namespace Game.Effects.Triggers
{
    public interface IMoveRangeModifierTrigger: IComparable<IMoveRangeModifierTrigger>
    {
        public int ModifyMoveRange(int baseRange);

        int IComparable<IMoveRangeModifierTrigger>.CompareTo(IMoveRangeModifierTrigger other)
        {
            return 0;
        }
    }

    public interface IAttackRangeModifier: IComparable<IAttackRangeModifier>
    {
        public int ModifyAttackRange(int baseRange);

        int IComparable<IAttackRangeModifier>.CompareTo(IAttackRangeModifier other)
        {
            return 0;
        }
    }
}

