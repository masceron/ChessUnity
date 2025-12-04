using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class GuidingSirenMoves
    {
        public static int Quiets(List<Action.Action> list, int pos, ref int index, bool isPlayer)
        {
            var (rank, file) = RankFileOf(pos);
            var p = PieceOn(pos);
            var effectiveMoveRange = p.GetMoveRange(ref index);
            
            for (var i = 1; i <= effectiveMoveRange; i++)
            {
                MakeMove(rank + i, file);
                MakeMove(rank - i, file);
                MakeMove(rank, file + i);
                MakeMove(rank, file - i);
                MakeMove(rank + i, file + i);
                MakeMove(rank - i, file + i);
                MakeMove(rank + i, file - i);
                MakeMove(rank - i, file - i);
            }

            return 15 + 5 * effectiveMoveRange;
            
            void MakeMove(int tRank, int tFile)
            {
                if (!VerifyBounds(tRank) || !VerifyBounds(tFile)) return;

                var tPos = IndexOf(tRank, tFile);
                if (!IsActive(tPos)) return;
                
                if (PieceOn(tPos) == null)
                {
                    list.Add(new NormalMove(pos, tPos));
                }
            }
        }

        public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
        {
            var (rank, file) = RankFileOf(pos);
            var p = PieceOn(pos);
            var color = p.Color;
            var effectiveMoveRange = p.AttackRange;
            
            for (var i = 1; i <= effectiveMoveRange; i++)
            {
                MakeCapture(rank + i, file);
                MakeCapture(rank - i, file);
                MakeCapture(rank, file + i);
                MakeCapture(rank, file - i);
                MakeCapture(rank + i, file + i);
                MakeCapture(rank - i, file + i);
                MakeCapture(rank + i, file - i);
                MakeCapture(rank - i, file - i);
            }

            return 15 + 5 * effectiveMoveRange;

            void MakeCapture(int tRank, int tFile)
            {
                if (!VerifyBounds(tRank) || !VerifyBounds(tFile)) return;

                var tPos = IndexOf(tRank, tFile);

                var pieceOn = PieceOn(tPos);
                
                if (pieceOn != null && pieceOn.Color != color)
                {
                    list.Add(new NormalCapture(pos, tPos));
                } else if (pieceOn == null && !isPlayer)
                {
                    list.Add(new NormalCapture(pos, tPos));
                }
            }
        }
    }
}