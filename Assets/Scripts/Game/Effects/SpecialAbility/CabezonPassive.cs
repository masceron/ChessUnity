

using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class CabezonPassive : Effect, IAfterPieceActionEffect
    {
        public CabezonPassive(PieceLogic piece) : base(-1, 1, piece, "effect_cabezon_passive")
        {

        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is ICaptures && action.Maker == Piece.Pos && (action.Result == ResultFlag.Blocked || action.Result == ResultFlag.Miss))
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, PieceOn(action.Target)), Piece));
            }
        }
    }
}