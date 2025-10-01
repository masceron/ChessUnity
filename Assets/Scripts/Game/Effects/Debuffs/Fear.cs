using System;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Piece;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Fear: Effect
    {
        public Fear(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectName.Taunted)
        {
        }
        
        public override void OnCall(Action.Action action)
        {
            if (action == null || action.Target != Piece.Pos || action.Result != ActionResult.Succeed) return;
            
            action.Result = ActionResult.Failed;
            
            var caller = PieceOn(action.Target);
            var color = caller.Color;
            var (rank, file) = RankFileOf(Piece.Pos);
            
            var push = !color ? -1 : 1;

            for (var fillOff = file + push; fillOff != fillOff + push * 2; fillOff += push)
            {
                var MoveToPice = PieceOn(IndexOf(rank, fillOff));
                if (MoveToPice != null)
                {
                    break;
                }
                
                ActionManager.ExecuteImmediately(new NormalMove(rank, fillOff));
            }
        }
    }
}