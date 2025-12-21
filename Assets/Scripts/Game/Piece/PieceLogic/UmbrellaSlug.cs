using Game.Action;
using Game.Action.Internal;
using Game.Effects.Others;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class UmbrellaSlug : Commons.PieceLogic
    {
        public UmbrellaSlug(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new UmbrellaSlugSpecialAbility(this)));
        }
    }
}