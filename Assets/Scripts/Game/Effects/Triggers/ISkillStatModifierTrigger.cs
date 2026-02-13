using System;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Triggers
{
    public interface ISkillStatModifierTrigger : IComparable<ISkillStatModifierTrigger>
    {
        int IComparable<ISkillStatModifierTrigger>.CompareTo(ISkillStatModifierTrigger other)
        {
            return 0;
        }

        public int Modify(SkillStat stat);
    }
}