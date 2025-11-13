using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Others;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class HammerOyster : PieceLogic
    {
        public HammerOyster(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, PawnPushMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Extremophile(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Shield(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new HammerOysterPassive(this)));
        }
    }
}