using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class WireCoralGobyPassive : Effect, IBeforeApplyEffectTrigger
    {
        public WireCoralGobyPassive(PieceLogic piece) : base(-1, 1, piece, "effect_wire_coral_goby_passive")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Reaction;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.SourcePiece != Piece) return;
            if (applyEffect.Effect is not Tethered tethered) return;

            var tetheredPiece = tethered.Piece;
            if (tetheredPiece == null || tetheredPiece == Piece) return;

            var trueBite = new TrueBite(-1, tetheredPiece);
            tethered.GrantedEffects.Add(trueBite);
            ActionManager.EnqueueAction(new ApplyEffect(trueBite, Piece));

            var formation = GetFormation(Piece.Pos);
            if (formation != null && formation.GetColor() == Piece.Color)
            {
                var snappingStrike = new Game.Effects.Traits.SnappingStrike(tetheredPiece);
                tethered.GrantedEffects.Add(snappingStrike);
                ActionManager.EnqueueAction(new ApplyEffect(snappingStrike, Piece));
            }
        }
    }
}
