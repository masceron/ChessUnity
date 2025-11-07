using System;
using System.Linq;
using Game.Common;
using Game.Effects;
using Game.Managers;
using UnityEngine;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ApplyEffect: Action, IInternal
    {
        public readonly Effect Effect;
        public ApplyEffect(Effect e) : base(-1)
        {
            Effect = e;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            if (Effect.ObserverActivateWhen != ObserverActivateWhen.None)
            {
                BoardUtils.AddObserver(Effect);
            }
            
            var already = Effect.Piece.Effects.FirstOrDefault(e => e.EffectName == Effect.EffectName);

            if (already == null)
            {
                // If the effect is applied as a result of an Action not from end turn trigger, increment the duration by 1.
                if (Effect.Duration != -1 && ActionManager.CurrentPhase == Phase.BeforeEndTurn) Effect.Duration++;
                
                Effect.OnApply();
                Effect.Piece.Effects.Add(Effect);
            }
            else
            {
                switch (AssetManager.Ins.EffectData[Effect.EffectName].stack)
                {
                    case EffectStack.Stackable:
                        if (already.Strength < Effect.Strength) already.Strength = Effect.Strength;
                        var weakerEffect = already.Strength < Effect.Strength ? already : Effect;
                        var strongerEffect = weakerEffect == already ? Effect : already;
                        var newDuration = strongerEffect.Duration + Math.Floor(weakerEffect.Duration * (float)weakerEffect.Duration / strongerEffect.Duration);
                        already.Duration = (sbyte)newDuration;
                        UnityEngine.Debug.Log(already.Duration);
                        break;
                    case EffectStack.NonStackable: default:
                        break;
                    case EffectStack.Additive:
                        already.Strength += Effect.Strength;
                        break;
                }
            }
            
        }
    }
}