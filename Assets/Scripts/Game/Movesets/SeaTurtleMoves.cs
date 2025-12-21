using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;
using Game.Effects;
using System.Linq;
using Game.Piece;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaTurtleMoves
    {
        public static void Quiets(List<Action.Action> list, int pos)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);
            
            var effectiveMoveRange = caller.GetMoveRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, effectiveMoveRange))
            {
                MakeMove(rankOff, fileOff);
            }
            
            return;
            
            void MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                
                var piece = PieceOn(index);
                if (piece != null ||
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                    return;
                list.Add(new NormalMove(pos, index));
            }
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var piece = PieceOn(pos);
            var color = piece.Color;
            var moveRange = piece.GetAttackRange();

            var (rank, file) = RankFileOf(pos);

            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }

            if (color)
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange))
                {
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, moveRange))
                {
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, moveRange))
                {
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                }
            }
            else
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange))
                {
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, moveRange))
                {
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                }
            
                foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, moveRange))
                {
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                }
            }

            return;
            
            bool MakeCapture(int index)
            {
                var p = PieceOn(index);
                if (p == null) return true;
                if (!IsActive(index)) return false;
                if (!p.Effects.Any(e => e.Category == EffectCategory.Debuff)) return false;
                if (p.PieceRank != PieceRank.Construct && !p.Effects.Any(e => e.Category == EffectCategory.Debuff)) return false;
                if (p.Color != color)
                {
                    list.Add(new NormalCapture(pos, index));
                }

                return false;
            }
        }
        
    }
}