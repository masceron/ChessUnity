using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using static Game.Common.BoardUtils;

namespace Game.Moves
{
    public static class BarracudaMoves
    {
        public static void Quiets(List<Action.Action> list, int pos)
        {
            var (rank, pieceFile) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.EffectiveMoveRange;
            var color = piece.Color;
            
            for (var i = 1; i <= moveRange; i++)
            {
                for (var file = pieceFile - i; file <= pieceFile + i; file += 1)
                {
                    MakeMove(rank - i, file, i);
                }
                
                for (var file = pieceFile - i; file <= pieceFile + i - 1; file += 1)
                {
                    MakeMove(rank + i, file, i);
                }
                
                for (var tRank = rank - i + 1; tRank <= rank + i; tRank++)
                {
                    MakeMove(tRank, pieceFile + i, i);
                }
                
                for (var tRank = rank - i + 1; tRank <= rank + i - 1; tRank++)
                {
                    MakeMove(tRank, pieceFile - i, i);
                }
            }

            return;

            void MakeMove(int tRank, int file, int distance)
            {
                if (!VerifyBounds(tRank) || !VerifyBounds(file)) return;

                var tpos = IndexOf(tRank, file);
                if (!IsActive(IndexOf(tRank, file))) return;

                var pieceOn = PieceOn(tpos);
                if (pieceOn != null) return;

                if (distance > moveRange) return;
                
                if (distance == moveRange)
                {
                    if (!color)
                    {
                        if (tRank >= rank) return;
                    }
                    else if (tRank <= rank) return;
                }
                    
                list.Add(new NormalMove(pos, tpos));
            }
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var (rank, pieceFile) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.AttackRange;
            var color = piece.Color;
            
            for (var i = 1; i <= moveRange; i++)
            {
                for (var file = pieceFile - i; file <= pieceFile + i; file += 1)
                {
                    MakeCapture(rank - i, file, i);
                }
                
                for (var file = pieceFile - i; file <= pieceFile + i - 1; file += 1)
                {
                    MakeCapture(rank + i, file, i);
                }
                
                for (var tRank = rank - i + 1; tRank <= rank + i; tRank++)
                {
                    MakeCapture(tRank, pieceFile + i, i);
                }
                
                for (var tRank = rank - i + 1; tRank <= rank + i - 1; tRank++)
                {
                    MakeCapture(tRank, pieceFile - i, i);
                }
            }

            void MakeCapture(int tRank, int file, int distance)
            {
                if (!VerifyBounds(tRank) || !VerifyBounds(file)) return;
            
                var tpos = IndexOf(tRank, file);
                if (!IsActive(IndexOf(tRank, file))) return;
            
                var pieceOn = PieceOn(tpos);

                if (distance > moveRange || pieceOn == null || pieceOn.Color == color) return;
                
                if (distance == moveRange)
                {
                    if (!color)
                    {
                        if (tRank >= rank) return;
                    }
                    else if (tRank <= rank) return;
                }
                
                if (pieceOn.Color != color)
                    list.Add(new NormalCapture(pos, tpos));
            }
        }
    }
}