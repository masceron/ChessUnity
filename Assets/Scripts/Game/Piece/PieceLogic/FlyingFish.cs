using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FlyingFish: PieceLogic
    {
        public FlyingFish(PieceConfig cfg) : base(cfg, FlyingFishMoves.Quiets, FlyingFishMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }

        protected override void CustomBehaviors(List<Action.Action> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] is IQuiets)
                {
                    list[i] = new FlyingFishMove(Pos, list[i].Target);
                }
            }
        }
    }
}