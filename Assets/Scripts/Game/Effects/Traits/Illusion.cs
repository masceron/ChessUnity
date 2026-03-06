using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    public class Illusion : StateEffect, IOnApplyTrigger, IOnMoveGenTrigger
    {
        public override StateType StateType => StateType.Illusion;
        private bool color;
        private int PieceIdx;
        public Illusion(PieceLogic piece) : base(-1, 1, piece, "effect_illusion")
        {

        }

        public Illusion(int idx, bool color) : base(-1, 1, null, "effect_illusion")
        {
            this.PieceIdx = idx;
            this.color = color;
        }   

        public override void OnApply()
        {
            base.OnApply();
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