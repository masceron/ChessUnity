using System;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Triggers
{
    public interface ISkillStatModifierTrigger: IComparable<ISkillStatModifierTrigger>
    {
        public int Modify(SkillStat stat);

        int IComparable<ISkillStatModifierTrigger>.CompareTo(ISkillStatModifierTrigger other)
        {
            return 0;
        }
    }
}