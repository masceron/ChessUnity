using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class WireCoralGobyPassive : Effect, IAfterPieceActionTrigger
    {
        public WireCoralGobyPassive(PieceLogic piece) : base(-1, 1, piece, "effect_wire_coral_goby_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not SymbioticCapture) return;
            if (action.Maker != Piece.Pos) return;
            if (action.Result != ResultFlag.Success) return;

            var tetheredPiece = PieceOn(action.Target);
            if (tetheredPiece == null) return;

            ActionManager.EnqueueAction(new ApplyEffect(new TrueBite(-1, tetheredPiece), Piece));

            var formation = GetFormation(Piece.Pos);
            if (formation != null && formation.GetColor() == Piece.Color)
                ActionManager.EnqueueAction(new ApplyEffect(new Game.Effects.Traits.SnappingStrike(tetheredPiece), Piece));
        }
    }
}
