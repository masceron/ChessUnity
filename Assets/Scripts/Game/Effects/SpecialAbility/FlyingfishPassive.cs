using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class FlyingfishPassive : Effect, IOnMoveGenTrigger, IAfterPieceActionTrigger
    {
        public FlyingfishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_flying_fish_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action.GetMakerAsPiece() != Piece) return;
            if (action is not FlyingFishMove flyingFishMove) return;

            var (rankFrom, fileFrom) = RankFileOf(flyingFishMove.GetFrom());
            var (rankTo, fileTo) = RankFileOf(flyingFishMove.GetTargetPos());
            var board = PieceBoard();

            var rankDir = rankTo == rankFrom ? 0 : rankTo > rankFrom ? 1 : -1;
            var fileDir = fileTo == fileFrom ? 0 : fileTo > fileFrom ? 1 : -1;

            while (rankFrom != rankTo || fileFrom != fileTo)
            {
                rankFrom += rankDir;
                fileFrom += fileDir;

                var p = board[IndexOf(rankFrom, fileFrom)];
                if (p == null || p.Color == Piece.Color) continue;

                ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 1, p), Piece));
                break;
            }
        }

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            for (var i = 0; i < actions.Count; i++)
                if (actions[i] is IQuiets)
                    actions[i] = new FlyingFishMove(Piece, actions[i].GetTargetPos());
        }
    }
}