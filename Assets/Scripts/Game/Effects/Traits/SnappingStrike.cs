using System.Collections.Generic;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnappingStrike: Effect
    {
        public SnappingStrike(PieceLogic piece, sbyte duration = -1) : base(duration, -1, piece,
            "effect_snapping_strike")
        {}

        public override void OnCallMoveGen(List<Action.Action> actions)
        {
            for (var i = 0; i < actions.Count; i++)
            {
                if (actions[i] is NormalCapture capture)
                {
                    actions[i] = new Action.Captures.SnappingStrike(capture.Maker, capture.Target);
                }
            }
        }
    }
}