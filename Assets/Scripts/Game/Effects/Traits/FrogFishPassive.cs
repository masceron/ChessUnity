using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrogFishPassive : Effect, IAfterPieceActionTrigger
    {
        public FrogFishPassive(PieceLogic piece) : base(-1, -1, piece, "effect_frog_fish_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action.GetMaker() != Piece) return;

            if (action.GetFrom() == action.GetMakerPos() || action.GetMakerPos() != action.GetTargetPos()) return;
            var targetFormation = GetFormation(action.GetTargetPos());
            if (targetFormation is not PredatorLair) return;

            var (rank, file) = RankFileOf(action.GetTargetPos());
            foreach (var (nRank, nFile) in MoveEnumerators.AroundUntil(rank, file, 2))
            {
                var piece = PieceOn(IndexOf(nRank, nFile));
                if (piece == null || piece.Color == Piece.Color) continue;

                ActionManager.EnqueueAction(new ApplyEffect(new Marked(-1, piece), Piece));
            }
        }
    }
}