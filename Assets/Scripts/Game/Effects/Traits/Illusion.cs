using System.Collections.Generic;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Internal;
using Game.Action;

namespace Game.Effects.Traits
{
    public class Illusion : Effect, IOnApply, IOnMoveGenEffect
    {
        public Illusion(PieceLogic piece) : base(-1, 1, piece, "effect_illusion")
        {
            
        }

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            for (var i = 0; i < actions.Count; i++)
            {
                if (actions[i] is NormalCapture capture && capture.Maker == Piece.Pos)
                {
                    actions[i] = new IllusionCapture(capture.Maker, capture.Target);
                }
            }
        }

        public void OnApply()
        {
            foreach (var effect in Piece.Effects.ToList())
            {
                UnityEngine.Debug.Log("Illusion OnApply RemoveEffect: " + effect.EffectName);
                if (effect.EffectName == "effect_illusion")
                {
                    continue;
                }
                if (effect.Category == EffectCategory.Augmentation && effect.Duration < 0)
                {
                    RemoveEffectObserver(effect);
                    if (effect is IOnRemove onRemove)
                        onRemove.OnRemove();
                    Piece.Effects.Remove(effect);
                }
                else
                {
                    ActionManager.ExecuteImmediately(new RemoveEffect(effect));
                }
            }
        }
    }
}