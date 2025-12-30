using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Common;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Consume: Effect
    {
        public Consume(PieceLogic piece) : base(-1, 1, piece, "effect_consume")
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action.Maker == Piece.Pos && action is ICaptures && action.Result == ResultFlag.Success)
            {
                var captured = BoardUtils.PieceOn(action.Target);

                if (captured.Effects.Any(e => e.EffectName is "effect_surpass" or "effect_vigorous")) return;
                
                Piece.Quiets = captured.Quiets;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }
    }
}