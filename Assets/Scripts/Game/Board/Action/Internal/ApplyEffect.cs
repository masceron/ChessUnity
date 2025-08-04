using System;
using System.Linq;
using Game.Board.Effects;
using Game.Board.General;
using Game.Common;

namespace Game.Board.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ApplyEffect: Action, IInternal
    {
        private readonly Effect effect;
        public ApplyEffect(Effect e) : base(-1)
        {
            effect = e;
        }

        protected override void Animate()
        {
            if (effect.ObserverActivateWhen != ObserverActivateWhen.None)
            {
                BoardUtils.AddObserver(effect);
            }
        }

        protected override void ModifyGameState()
        {
            var already = effect.Piece.Effects.FirstOrDefault(e => e.EffectName == effect.EffectName);

            if (already == null)
            {
                if (effect.Duration != -1 && ActionManager.CurrentPhase == Phase.BeforeEndTurn) effect.Duration++;
                
                effect.OnApply();
                effect.Piece.Effects.Add(effect);
            }
            else
            {
                switch (AssetManager.Ins.EffectData[effect.EffectName].stack)
                {
                    case EffectStack.Stackable:
                        if (already.Strength < effect.Strength) already.Strength = effect.Strength;
                        var weakerEffect = already.Strength < effect.Strength ? already : effect;
                        var strongerEffect = weakerEffect == already ? effect : already;
                        var newDuration = strongerEffect.Duration + Math.Floor(weakerEffect.Duration * (float)weakerEffect.Duration / strongerEffect.Duration);
                        already.Duration = (sbyte)newDuration;
                        //Remove and reapply.
                        ActionManager.EnqueueAction(new RemoveEffect(already));
                        ActionManager.EnqueueAction(new ApplyEffect(already));
                        break;
                    case EffectStack.NonStackable: default:
                        break;
                    case EffectStack.Additive:
                        already.Strength += effect.Strength;
                        break;
                }
            }
            
        }
    }
}