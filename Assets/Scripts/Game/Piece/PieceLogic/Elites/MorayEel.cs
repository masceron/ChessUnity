using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Data.Pieces;
using Game.Effects.Traits;
using Game.Moves;

namespace Game.Piece.PieceLogic.Elites
{
    public class MorayEel: PieceLogic
    {
        public MorayEel(PieceConfig cfg) : base(cfg, MorayEelMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Ambush(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new MorayEelCamouflage(this)));
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list, Pos);
            Captures(list, Pos);
        }
    }
}