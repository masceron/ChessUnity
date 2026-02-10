using Game.Effects;
using Game.Action;
using Game.Piece.PieceLogic.Commons;
using Game.Action.Captures;
using static Game.Common.BoardUtils;

namespace Assets.Scripts.Game.Effects.Traits
{
    public class RustyParrotfishPassive : Effect, IAfterPieceActionEffect
    {
        public RustyParrotfishPassive(PieceLogic piece) : base(-1, -1, piece, "effect_rusty_parrotfish_passive")
        {
            
        }

        public void OnCallAfterPieceAction(Action action)
        {
            if (action == null || action is not ICaptures) return;

            var target = PieceOn(action.Target);

            
        }
    }
}