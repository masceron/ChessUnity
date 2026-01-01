using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class FrilledSharkPassive : Effect, IAfterPieceActionEffect
    {
        public FrilledSharkPassive(PieceLogic piece) : base(-1, 1, piece, "effect_frilled_shark_passive")
        {

        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Maker != Piece.Pos) { return; }
            var pieceOn = PieceOn(action.Target);
            if (pieceOn == null) { return; }
            if (pieceOn.Effects.Any(e => e is Relentless) && !pieceOn.IsDead())
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, pieceOn)));
            }
        }
    }
}