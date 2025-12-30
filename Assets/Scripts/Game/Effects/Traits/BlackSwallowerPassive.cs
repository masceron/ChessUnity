using Game.Piece.PieceLogic.Commons;
using Game.Action;
using Game.Effects.Others;
using Game.Piece;
using static Game.Common.BoardUtils;
using Game.Action.Internal;
namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlackSwallowerPassive: Effect
    {
        public BlackSwallowerPassive(PieceLogic piece) : base(-1, 1, piece, "effect_black_swallower_passive")
        {
        }

        public override void OnCallPieceAction(Action.Action action)    
        {
            if (action == null) return;

            if (action.Result != ResultFlag.Success) return;

            if (action.Maker == Piece.Pos) return;
            
            var targetPiece = PieceOn(action.Target);
            if (targetPiece is { PieceRank: PieceRank.Elite or PieceRank.Champion or PieceRank.Commander })
            {
                ActionManager.EnqueueAction(new ApplyEffect(new KillPieceAfterSwitchTurn(Piece)));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 40;
        }

    }
}