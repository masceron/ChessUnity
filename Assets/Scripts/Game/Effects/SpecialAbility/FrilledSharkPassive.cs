using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class FrilledSharkPassive : Effect, IBlockEffect
    {
        public FrilledSharkPassive(PieceLogic piece) : base(-1, 1, piece, "effect_frilled_shark_passive")
        {

        }
        public void OnCallBlocked(Block action)
        {
            if (action.Maker != Piece.Pos) { return; }
            PieceLogic pieceOn = PieceOn(action.Target);
            if (pieceOn == null){ return; }
            if (PieceOn(action.Target).Effects.Any(e => e is Relentless))
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, pieceOn)));
            }
        }
    }
}