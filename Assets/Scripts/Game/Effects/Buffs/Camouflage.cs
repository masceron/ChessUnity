using System.Collections.Generic;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Camouflage : Effect, IOnMoveGenTrigger
    {
        public Camouflage(PieceLogic piece, int duration = -1) : base(duration, 1, piece, "effect_camouflage")
        {
        }

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller.Color == Piece.Color || caller.Effects.Any(e => e.EffectName == "effect_marked")) return;

            actions.RemoveAll(a => BoardUtils.Distance(a.GetFrom(), Piece.Pos) > 4 && a.GetTargetAsPiece() == Piece);
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 10;
        }
    }
}