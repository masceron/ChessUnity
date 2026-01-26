

using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class RedtailParrotfishPassive : Effect
    {
        public RedtailParrotfishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_redtail_parrotfish_passive")
        {
            // Không làm gì cả vì if-else đã đặt ở Demolisher
        }

    }

}
