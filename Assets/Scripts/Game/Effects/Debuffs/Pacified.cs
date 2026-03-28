using System.Collections.Generic;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Debuffs
{
    public class Pacified : Effect, IOnMoveGenTrigger
    {
        public Pacified(int duration, PieceLogic piece) : base(duration, 1, piece, "effect_pacified")
        {
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 30;
        }

        // chỉ xóa action ăn quân mà quân target là quân địch
        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;

            actions.RemoveAll(action =>
                action is ICaptures &&
                action.GetTarget()?.Color != Piece.Color);
        }
    }
}