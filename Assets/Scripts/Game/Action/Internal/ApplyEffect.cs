using System;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using Game.Tile;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ApplyEffect: Action, IInternal
    {
        public readonly Effect Effect;
        public readonly FormationType SourceFormationType;
        public readonly PieceLogic SourcePiece;
        public readonly RelicLogic SourceRelic;

        /// <summary>
        /// Sử dụng apply chung chung, tự apply cho mình
        /// </summary>
        /// <param name="e">Effect is applied</param>
        public ApplyEffect(Effect e) : base(-1)
        {
            Effect = e;
        }

        /// <summary>
        /// Sử dụng khi formation apply effect.
        /// </summary>
        /// <param name="e">Effect is applied</param>
        /// <param name="formationType">Formation that apply this effect</param>
        public ApplyEffect(Effect e, FormationType formationType = FormationType.None) : base(-1)
        {
            Effect = e;
            SourceFormationType = formationType;
        }

        /// <summary>
        /// Sử dụng khi Piece apply effect.
        /// </summary>
        /// <param name="e">Effect is applied</param>
        /// <param name="source">Piece that apply this effect</param>
        public ApplyEffect(Effect e, PieceLogic source) : base(source.Pos)
        {
            Effect = e;
            SourcePiece = source;
        }

        /// <summary>
        /// Sử dụng khi Relic apply effect. Bên trắng side = false, bên đen side = true
        /// </summary>
        /// <param name="e">Effect is applied</param>
        /// <param name="source">Relic that apply this effect</param>
        public ApplyEffect(Effect e, RelicLogic source): base(-1)
        {
            Effect = e;
            SourceRelic = source;
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
                
                if (Effect is IOnApply onApply)
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
                        var newDuration = strongerEffect.Duration + Math.Floor(weakerEffect.Duration * (float)weakerEffect.Duration / strongerEffect.Duration);
                        already.Duration = (int)newDuration;
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