using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Data.Pieces;
using Game.Effects.Traits;
using Game.Moves;

namespace Game.Piece.PieceLogic.Swarm
{
    public class Seahorse: PieceLogic
    {
        public Seahorse(PieceConfig cfg) : base(cfg, KnightMoves.Quiets, KnightMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list, Pos);
            Captures(list, Pos);
        }
    }
}