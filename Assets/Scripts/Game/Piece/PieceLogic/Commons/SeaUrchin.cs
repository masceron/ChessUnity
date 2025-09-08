using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using Game.Movesets;

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

        protected override void CustomBehaviors(List<Action.Action> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] is ICaptures)
                    list[i] = new DestroyConstruct(Pos, list[i].Target);
            }
        }
    }
}