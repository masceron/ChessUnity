using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    public class Illusion : Effect, IOnApplyTrigger, IOnMoveGenTrigger
    {
        public Illusion(PieceLogic piece) : base(-1, 1, piece, "effect_illusion")
        {
        }

        public void OnApply()
        {
            foreach (var effect in Piece.Effects.ToList())
            {
                Debug.Log("Illusion OnApply RemoveEffect: " + effect.EffectName);
                if (effect.EffectName == "effect_illusion") continue;
                if (effect.Category == EffectCategory.Augmentation && effect.Duration < 0)
                {
                    RemoveObserver(effect);
                    if (effect is IOnRemoveTrigger onRemove)
                        onRemove.OnRemove();
                    Piece.Effects.Remove(effect);
                }
                else
                {
                    ActionManager.EnqueueAction(new RemoveEffect(effect));
                }
            }
        }

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            for (var i = 0; i < actions.Count; i++)
                if (actions[i] is NormalCapture capture && capture.Maker == Piece.Pos)
                    actions[i] = new IllusionCapture(capture.Maker, capture.Target);
        }
    }
}