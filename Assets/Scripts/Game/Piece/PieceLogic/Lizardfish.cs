using Game.Action;
using Game.Action.Internal;
using Game.Effects.Condition;
using Game.Effects.FieldEffect;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class Lizardfish : Commons.PieceLogic
    {
        public Lizardfish(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, SmallPredatorMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new NativeGround(this, FieldEffectType.Whirlpool)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Ambush(this)));
        }
    }
}