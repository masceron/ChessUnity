using System.Collections.Generic;
using System.Linq;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Internal;
using Game.Managers;
using Game.Action;

namespace Game.Effects.Traits
{
    public class Illusion : Effect
    {
        public Illusion(PieceLogic piece) : base(-1, 1, piece, "effect_illusion")
        {
            
        }

        public override void OnCallMoveGen(List<Action.Action> actions)
        {
            for (var i = 0; i < actions.Count; i++)
            {
                if (PieceOn(actions[i].Maker).Effects
                    .All(e => e.EffectName != "effect_illusion"))
                {
                    continue;
                }
                
                if (actions[i] is NormalCapture capture)
                {
                    actions[i] = new Action.Captures.IllusionCapture(capture.Maker, capture.Target);
                }
            }
        }

        public override void OnApply()
        {
            foreach (var effect in Piece.Effects.ToList())
            {
                if (effect.EffectName != "effect_illusion")
                {
                    ActionManager.EnqueueAction(new RemoveEffect(effect));
                }
            }
        }
    }
}