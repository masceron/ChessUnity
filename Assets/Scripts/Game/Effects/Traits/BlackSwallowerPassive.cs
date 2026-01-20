using Game.Piece.PieceLogic.Commons;
using Game.Action;
using Game.Action.Captures;
using Game.Effects.Others;
using Game.Piece;
using static Game.Common.BoardUtils;
using Game.Action.Internal;
namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlackSwallowerPassive: Effect, IAfterPieceActionEffect
    {
        public BlackSwallowerPassive(PieceLogic piece) : base(-1, 1, piece, "effect_black_swallower_passive")
        {
        }
        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 40;
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;

            if (action.Result != ResultFlag.Success) return;

            if (action.Maker == Piece.Pos) return;
            
            var targetPiece = PieceOn(action.Target);
            if (targetPiece is { PieceRank: PieceRank.Elite or PieceRank.Champion or PieceRank.Commander })
            {
                ActionManager.EnqueueAction(new ApplyEffect(new KillPieceAfterSwitchTurn(Piece), Piece));
            }
        }
    }
}