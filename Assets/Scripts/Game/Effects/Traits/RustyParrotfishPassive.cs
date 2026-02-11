using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    public class RustyParrotfishPassive : Effect, IAfterPieceActionEffect
    {
        public RustyParrotfishPassive(PieceLogic piece) : base(-1, -1, piece, "effect_rusty_parrotfish_passive")
        {
            
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;

            var target = PieceOn(action.Target);

            
        }
    }
}