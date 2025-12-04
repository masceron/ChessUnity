using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    public interface ISkills
    {
        public int AIPenaltyValue(PieceLogic maker);
    }
}