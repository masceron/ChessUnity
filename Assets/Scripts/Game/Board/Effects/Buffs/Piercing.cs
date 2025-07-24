using Game.Board.Action;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Buffs
{
    public class Piercing: Effect
    {
        public Piercing(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectType.Piercing)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action != null && action.Caller == Piece.pos) action.Result = ActionResult.Unblockable;
        }

        public override string Description()
        {
            return MatchManager.assetManager.EffectData[EffectName].description;
        }
    }
}