using System.Collections.Generic;
using Game.Common;
using Game.Effects;
using Game.Piece;
using static Game.Common.BoardUtils;
using ZLinq;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaTurtleMoves
    {
        public static int Quiets(List<int> list, int pos)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);
            var effectiveMoveRange = caller.GetMoveRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, effectiveMoveRange))
                MakeMove(rankOff, fileOff);

            return 10 + 10 * effectiveMoveRange;

            void MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                var piece = PieceOn(index);
                if (piece != null ||
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                    return;
                list.Add(index);
            }
        }

        /// <summary>
        /// SeaTurtle chỉ ăn được quân có debuff hoặc Construct.
        /// Logic debuff check thuộc pathfinding (khả năng tấn công riêng của SeaTurtle).
        /// </summary>
        public static int Captures(List<int> list, int pos)
        {
            var piece = PieceOn(pos);
            var moveRange = piece.GetAttackRange();
            var (rank, file) = RankFileOf(pos);

            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange))
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange))
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;

            if (piece.Color)
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange))
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, moveRange))
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, moveRange))
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }
            else
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange))
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, moveRange))
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, moveRange))
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }

            return 10 + 10 * moveRange;

            bool MakeCapture(int index)
            {
                var p = PieceOn(index);
                if (p == null) return true; // trống → tiếp tục slide (không thêm vào list)
                if (!IsActive(index)) return false;
                // SeaTurtle chỉ tấn công Construct hoặc quân có debuff
                if (p.PieceRank != PieceRank.Construct && !p.Effects.Any(e => e.Category == EffectCategory.Debuff))
                    return false;
                list.Add(index); // quân hợp lệ → thêm pos (bất kể màu)
                return false;
            }
        }
    }
}