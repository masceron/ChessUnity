using Game.Piece.PieceLogic.Commons;

namespace Game.Effects
{
    public interface ISkillStatModifier
    {
        public int Modify(SkillStat stat);
    }
}