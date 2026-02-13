using System.Collections.Generic;
using Game.Action.Captures;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnappingStrike: Effect, IOnMoveGenTrigger
    {
        public SnappingStrike(PieceLogic piece, int duration = -1) : base(duration, -1, piece,
            "effect_snapping_strike")
        {}

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            for (var i = 0; i < actions.Count; i++)
            {
                if (PieceOn(actions[i].Maker).Effects
                    .All(e => e.EffectName != "effect_snapping_strike"))
                {
                    continue;
                }
                
                if (actions[i] is NormalCapture capture)
                {
                    actions[i] = new Action.Captures.SnappingStrike(capture.Maker, capture.Target);
                }
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 50;
        }
    }
}