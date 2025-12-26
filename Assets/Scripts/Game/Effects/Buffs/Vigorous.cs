using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Vigorous : Effect
    {
        public Vigorous(PieceLogic piece) : base(-1, 1, piece, "effect_vigorous")
        {
        }

        // public override void OnCallPieceAction(Action.Action action)
        // {
        //     if (action != null && action is NormalCapture && action.Target == Piece.Pos)
        //     {
        //         var capturer = BoardUtils.PieceOn(action.Maker);
        //         capturer.ImmuneEffect("effect_consume");
        //     }
        // }
        
        
        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 80;
        }
    }
}