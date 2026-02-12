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
        private bool _surpassed;
        private const int EvasionProbability = 25;

        public BottlenoseDolphinPassive(PieceLogic piece) : base(-1, 1, piece, "effect_bottlenose_dolphin_passive")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
        }

        public StartTurnEffectType StartTurnEffectType { get; }

        public void OnCallStart(Action.Action lastMainAction)
        {
            _surpassed = Piece.Effects.Any(e => e.EffectName == "effect_surpass");
            if (MatchManager.Ins.GameState.IsDay && !_surpassed) 
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Surpass(Piece)));
            }
            switch (MatchManager.Ins.GameState.IsDay)
            {
                case true:
                {
                    var existingEvasion = Piece.Effects.OfType<Evasion>().FirstOrDefault();
                    if (existingEvasion != null)
                    {
                        existingEvasion.Strength = Math.Max(existingEvasion.Strength, EvasionProbability);
                    }
                    else
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, EvasionProbability, Piece)));
                    }

                    break;
                }
                case false:
                {
                    var surpassEffect = Piece.Effects.OfType<Surpass>().FirstOrDefault();
                    if (surpassEffect != null)
                    {
                        ActionManager.EnqueueAction(new RemoveEffect(surpassEffect));
                    }
                    var evasionEffect = Piece.Effects.OfType<Evasion>().FirstOrDefault();
                    if (evasionEffect != null)
                    {
                        if (evasionEffect.Strength <= EvasionProbability)
                        {
                            ActionManager.EnqueueAction(new RemoveEffect(evasionEffect));
                        }
                        else
                        {
                            evasionEffect.Strength -= EvasionProbability;
                        }
                    }

                    break;
                }
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 20;
        }
    }
}