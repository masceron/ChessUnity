using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    public class YellowlinedBassletPassive : Effect, IBeforeApplyEffectTrigger
    {
        private const int Strength1 = 1;
        private const int Duration1 = 3;
        private const int Strength2 = 1;
        private const int Duration2 = 3;
        
        public YellowlinedBassletPassive(PieceLogic piece) : base(-1, 1, piece, "effect_yellowlined_basslet_passive")
        {
            SetStat(EffectStat.Strength, Strength1, 1);
            SetStat(EffectStat.Duration, Duration1, 1);
            SetStat(EffectStat.Strength, Strength2, 2);
            SetStat(EffectStat.Duration, Duration2, 2);
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Reaction;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            var pieceApplied = applyEffect.Effect.Piece;
            if (applyEffect.Effect.EffectName == "effect_blinded" &&  pieceApplied.Color != Piece.Color)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new LongReach(Piece, GetStat(EffectStat.Duration, 1)
                    , GetStat(EffectStat.Strength, 1))));
               ActionManager.EnqueueAction(new ApplyEffect(new Haste(GetStat(EffectStat.Duration, 2)
                   , GetStat(EffectStat.Strength, 2), Piece)));
            }
        }
    }
}