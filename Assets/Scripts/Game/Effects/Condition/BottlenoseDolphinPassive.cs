using Game.Managers;
using Game.Action.Internal;
using Game.Action;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using System;
using ZLinq;

namespace Game.Effects.Condition
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BottlenoseDolphinPassive: Effect, IStartTurnEffect
    {
        private bool surpassed;
        private int evasionProbability = 25;
        public BottlenoseDolphinPassive(PieceLogic piece) : base(-1, 1, piece, "effect_bottlenose_dolphin_passive")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
        }

        public StartTurnEffectType StartTurnEffectType { get; }

        public void OnCallStart(Action.Action lastMainAction)
        {
            surpassed = Piece.Effects.Any(e => e.EffectName == "effect_surpass");
            if (MatchManager.Ins.GameState.IsDay && !surpassed) 
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Surpass(Piece)));
            }
            if (MatchManager.Ins.GameState.IsDay)
            {
                var existingEvasion = Piece.Effects.OfType<Evasion>().FirstOrDefault();
                if (existingEvasion != null)
                {
                    existingEvasion.Probability = Math.Max(existingEvasion.Probability, evasionProbability);
                }
                else
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, evasionProbability, Piece)));
                }
            }

            if (!MatchManager.Ins.GameState.IsDay)
            {
                var surpassEffect = Piece.Effects.OfType<Surpass>().FirstOrDefault();
                if (surpassEffect != null)
                {
                    ActionManager.EnqueueAction(new RemoveEffect(surpassEffect));
                }
                var evasionEffect = Piece.Effects.OfType<Evasion>().FirstOrDefault();
                if (evasionEffect != null)
                {
                    if (evasionEffect.Probability <= evasionProbability)
                    {
                        ActionManager.EnqueueAction(new RemoveEffect(evasionEffect));
                    }
                    else
                    {
                        evasionEffect.Probability -= evasionProbability;
                    }
                }
                
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 20;
        }
    }
}