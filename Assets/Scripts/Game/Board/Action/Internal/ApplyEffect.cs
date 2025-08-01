using System;
using System.Linq;
using Game.Board.Effects;
using Game.Board.General;

namespace Game.Board.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ApplyEffect: Action, IInternal
    {
        private readonly Effect effect;
        public ApplyEffect(Effect e) : base(-1, false)
        {
            effect = e;
        }

        protected override void Animate()
        {
            if (effect.ObserverType != ObserverType.None)
            {
                EventObserver.AddObserver(effect);
            }
        }

        protected override void ModifyGameState()
        {
            var already = effect.Piece.Effects.FirstOrDefault(e => e.EffectName == effect.EffectName);

            if (already == null)
            {
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
                        break;
                    case EffectStack.NonStackable:
                        return;
                    case EffectStack.Additive:
                        already.Strength += effect.Strength;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
        }
    }
}