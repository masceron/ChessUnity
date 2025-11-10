using Game.Action;
using Game.Action.Internal;
using Game.Effects.Others;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Commons
{
    public class Slimehead : PieceLogic
    {
        public Slimehead(PieceConfig cfg) : base(cfg, FrontDefenderMoves.Quiets, FrontDefenderMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SlimeheadPassive(this)));
        }
    }
}