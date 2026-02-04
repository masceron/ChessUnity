using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class RibbonEel : Commons.PieceLogic
    {
        public RibbonEel(PieceConfig cfg) : base(cfg, AmbushPredatorMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Camouflage(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new TrueBite(this)));
        }
    }
}