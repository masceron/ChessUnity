using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Consume: Effect
    {
        public Consume(PieceLogic piece) : base(-1, 1, piece, EffectName.Consume)
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action.Maker == Piece.Pos && action is ICaptures && action.Result != ActionResult.Failed)
            {
                var captured = BoardUtils.PieceOn(action.Target);

                if (captured.Effects.Any(e => e.EffectName == EffectName.Surpass)) return;
                
                Piece.Quiets += captured.Quiets;
                Piece.MoveRange.Add(AssetManager.Ins.PieceData[captured.Type].moveRange);
            }
        }
    }
}