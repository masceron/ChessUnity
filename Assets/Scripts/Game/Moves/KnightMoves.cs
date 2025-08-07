using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Moves
{
    public static class KnightMoves
    {
        public static void Quiets(List<Action.Action> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            var caller = PieceOn(pos);
            var maxRange = caller.EffectiveMoveRange;

            for (var range = 1; range <= maxRange; range++)
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.KnightMovement(rank, file, range))
                {
                    MakeMove(rankOff, fileOff);
                }
            }

            return;
            
            void MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                var piece = PieceOn(index);
                if (piece != null ||
                    Distance(pos, index) != maxRange ||
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                    return;
                list.Add(new NormalMove(pos, index));
            }
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            var caller = PieceOn(pos);
            var color = caller.Color;
            var maxRange = caller.AttackRange;

            for (var range = 1; range <= maxRange; range++)
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.KnightMovement(rank, file, range))
                {
                    MakeCapture(rankOff, fileOff);
                }
            }

            return;
            
            void MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                var piece = PieceOn(index);
                
                if (piece == null || 
                    piece.Color == color || 
                    Distance(pos, index) != maxRange ||
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                    return;
                list.Add(new NormalCapture(pos, index));
            }
        }
    }
}