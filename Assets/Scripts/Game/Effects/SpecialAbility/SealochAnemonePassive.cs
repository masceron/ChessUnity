using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.SpecialAbility
{
    public class SealochAnemonePassive : Effect, IBeforeApplyEffectTrigger
    {
        public SealochAnemonePassive(PieceLogic piece) : base(-1, 1, piece, "effect_sealoch_anemone_passive")
        {
            SetStat(EffectStat.Radius, 1);
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Reaction;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.SourcePiece != Piece) return;
            if (applyEffect.Effect is not Tethered tethered) return;

            var tetheredPiece = tethered.Piece;
            if (tetheredPiece == null || tetheredPiece == Piece) return;
            if (tetheredPiece.Color != Piece.Color) return;

            var amplify = new Amplify(-1, GetStat(EffectStat.Radius), tetheredPiece);
            tethered.GrantedEffects.Add(amplify);
            ActionManager.EnqueueAction(new ApplyEffect(amplify, Piece));
        }
    }
}