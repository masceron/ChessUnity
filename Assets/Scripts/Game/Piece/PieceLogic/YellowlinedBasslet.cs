using Game.Action;
using Game.Action.Internal;
using Game.Effects.Others;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class YellowlinedBasslet : Commons.PieceLogic
    {
        public YellowlinedBasslet(PieceConfig cfg) : base(cfg, UpDoorMoves.Quiets, UpDoorMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new YellowlinedBassletPassive(this)));
        }
    }
}