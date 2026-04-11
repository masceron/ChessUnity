using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Traits;
using Game.Effects.Debuffs;
using Game.Action;
using Game.Action.Internal;
using Game.Effects;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TuxedoUrchin : Commons.PieceLogic
    {
        private const int Duration = -1;
        public TuxedoUrchin(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, BluffingMoves.Quiets)
        {

            ActionManager.ExecuteImmediately(new ApplyEffect(new Regenerative(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Blinded(Duration, 100, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SlimeCoat(this)));
        }

    }
}