using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Skills;
using Game.Effects;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BasketStar : Commons.PieceLogic
    {
        public BasketStar(PieceConfig cfg) : base(cfg, PhantomJellyMoves.Quiets, PhantomJellyMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new BasketStarPassive(this)));
        }
    }
}