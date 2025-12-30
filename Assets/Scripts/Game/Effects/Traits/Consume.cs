using System.Linq;
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
            if (action.Maker == Piece.Pos && action is ICaptures && action.Succeed)
            {
                var captured = BoardUtils.PieceOn(action.Target);

                if (captured.Effects.Any(e => e.EffectName == "effect_surpass" 
                                              || e.EffectName == "effect_vigorous")) return;
                
                // Piece.Quiets += captured.Quiets;
                // Piece.MoveRange.Add(AssetManager.Ins.PieceData[captured.Type].moveRange);
                Piece.Quiets = captured.Quiets;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }
    }
}