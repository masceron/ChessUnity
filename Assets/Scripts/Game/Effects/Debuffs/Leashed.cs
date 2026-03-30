using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Leashed : Effect, IOnMoveGenTrigger
    {
        public Leashed(PieceLogic piece, int duration) : base(duration, 1, piece, "effect_leashed")
        {
        }

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            if (actions == null || actions.Count == 0) return;

            actions.RemoveAll(action =>
                action.GetMakerAsPiece() == Piece &&
                Distance(action.GetTargetPos(), Piece.Pos) > 3
            );
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 20;
        }
    }
}