using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Moves
{
    public static class KingMoves
    {
        public static void Quiets(List<Action.Action> list, int pos)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);
            
            var effectiveMoveRange = caller.EffectiveMoveRange;

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
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);

            var color = caller.Color;
            var attackRange = caller.AttackRange;

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, attackRange))
            {
                MakeCapture(rankOff, fileOff);
            }

            return;
            
            void MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                var piece = PieceOn(index);
                
                if (piece == null || 
                    piece.Color == color || 
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                    return;
                list.Add(new NormalCapture(pos, index));
            }
        }
    }
}