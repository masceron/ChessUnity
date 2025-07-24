using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Buffs
{
    public class Shield: Effect
    {
        public Shield(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectType.Shield)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action == null || action.To != Piece.pos || action.Result != ActionResult.Succeed) return;
            
            action.Result = ActionResult.Failed;
            ActionManager.EnqueueAction(new RemoveEffect(this));
        }

        public override string Description()
        {
            return MatchManager.assetManager.EffectData[EffectName].description;
        }
    }
}