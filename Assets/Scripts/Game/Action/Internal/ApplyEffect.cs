using System;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Triggers;
using UnityEngine;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ApplyEffect : Action, IInternal
    {
        public readonly Effect Effect;

        /// <summary>
        ///     Sử dụng apply chung chung, tự apply cho mình
        /// </summary>
        /// <param name="e">Effect is applied</param>
        public ApplyEffect(Effect e) : base(null)
        {
            Effect = e;
        }

        /// <summary>
        ///     Sử dụng khi Piece apply effect.
        /// </summary>
        /// <param name="e">Effect is applied</param>
        /// <param name="source">Piece that apply this effect</param>
        public ApplyEffect(Effect e, Entity source) : base(source)
        {
            Effect = e;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            var already = Effect.Piece.Effects.FirstOrDefault(e => e.EffectName == Effect.EffectName);

            if (already == null)
            {
                // If the effect is applied as a result of an Action not from end turn trigger, increment the duration by 1.
                if (Effect.Duration != -1 && ActionManager.CurrentPhase == Phase.BeforeEndTurn) Effect.Duration++;

                if (Effect is IOnApplyTrigger onApply)
                    onApply.OnApply();
                Effect.Piece.Effects.Add(Effect);
                BoardUtils.AddEffectObserver(Effect);
            }
            else
            {
                switch (AssetManager.Ins.EffectData[Effect.EffectName].stack)
                {
                    case EffectStack.Stackable:
                        if (already.Strength < Effect.Strength) already.Strength = Effect.Strength;
                        var weakerEffect = already.Strength < Effect.Strength ? already : Effect;
                        var strongerEffect = weakerEffect == already ? Effect : already;
                        var newDuration = strongerEffect.Duration + Math.Floor(weakerEffect.Duration *
                            (float)weakerEffect.Duration / strongerEffect.Duration);
                        already.Duration = (int)newDuration;
                        Debug.Log(already.Duration);
                        break;
                    case EffectStack.NonStackable:
                    default:
                        break;
                    case EffectStack.Additive:
                        already.Strength += Effect.Strength;
                        break;
                }
            }
        }
    }
}