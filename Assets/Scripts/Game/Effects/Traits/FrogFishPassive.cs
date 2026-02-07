using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrogFishPassive : Effect, IAfterPieceActionEffect
    {
        public FrogFishPassive(PieceLogic piece) : base(-1, -1, piece, "effect_frog_fish_passive")
        {
            
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action.Maker != Piece.Pos) return;
            if (action.Maker == action.Target) return;
            var targetFormation = BoardUtils.GetFormation(action.Target);
            if (targetFormation == null || targetFormation is not PredatorLair) return;
            
            var (rank, file) = RankFileOf(action.Target);
            foreach (var (nRank, nFile) in MoveEnumerators.AroundUntil(rank, file, 2))
            {
                var piece = PieceOn(IndexOf(nRank, nFile));
                if (piece == null || piece.Color == Piece.Color) continue;

                ActionManager.EnqueueAction(new ApplyEffect(new Marked(-1, piece), Piece));
            }
        }
    }
}