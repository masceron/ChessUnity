using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class PaletailUnicornfish : Commons.PieceLogic
    {
        public PaletailUnicornfish(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new PaletailUnicornfishPassive(this)));
        }
    }
}