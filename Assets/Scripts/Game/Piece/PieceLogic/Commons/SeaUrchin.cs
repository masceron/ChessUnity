using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Data.Pieces;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using Game.Moves;

namespace Game.Piece.PieceLogic.Commons
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaUrchin: PieceLogic
    {
        public SeaUrchin(PieceConfig cfg) : base(cfg, PawnPushMoves.Quiets, PawnPushMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Carapace(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Blinded(-1, 50, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Demolisher(this)));
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Captures(list, Pos);
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = new DestroyConstruct(Pos, list[i].Target);
            }

            Quiets(list, Pos);
        }
    }
}