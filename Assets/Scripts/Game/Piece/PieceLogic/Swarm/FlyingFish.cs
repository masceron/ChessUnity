using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Data.Pieces;
using Game.Effects.Traits;
using Game.Moves;

namespace Game.Piece.PieceLogic.Swarm
{
    public class FlyingFish: PieceLogic
    {
        public FlyingFish(PieceConfig cfg) : base(cfg, FlyingFishMoves.Quiets, FlyingFishMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list, Pos);
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = new FlyingFishMove(Pos, list[i].Target);
            }
            Captures(list, Pos);
        }
    }
}