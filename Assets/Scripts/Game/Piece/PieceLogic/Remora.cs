using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class Remora: PieceLogic
    {
        public Remora(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {}

        protected override void CustomBehaviors(List<Action.Action> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = list[i] switch
                {
                    IQuiets => new RemoraMove(Pos, list[i].Target),
                    ICaptures => new RemoraMark(Pos, list[i].Target),
                    _ => list[i]
                };
            }
        }
    }
}