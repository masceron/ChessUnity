using Game.Action;
using Game.Action.Quiets;
using Game.Managers;
using Game.Action.Internal;
using UnityEngine;
using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;
using ZLinq;
using static Game.Common.BoardUtils;

//Xuất hiện vào turn thứ 30 của ván nếu chưa kết thúc, tạo ra vòng xoáy 2x2 giữa bàn cờ, 
//Mỗi turn hút tất cả các quân vào gần với xoáy nước 1 ô. 
//Quy luật hút: tạo một vector từ tâm quân cờ đến ô vòng xoáy gần nhất(có 4 ô, bạn cần tìm ô gần nhất với quân cờ)
//Hướng của vector là từ quân cờ tới tâm ô xoáy, vì hướng của vector là vô số, mà số hướng trong bàn cờ chỉ có 8 hướng
//nên bạn cần làm tròn hướng di chuyển theo quy tắc như sau:
//Nếu hướng vector là không chuẩn, bạn hãy làm tròn nó theo 4 hướng: trên, dưới, trái phải. KHÔNG làm tròn thành đường chéo
//Nếu hướng vector là chuẩn(hướng lên trên, dưới, trái, phải, chéo) thì cứ đi đúng như bình thường
//Quân nào bị vào xoáy nước sẽ mất 
//Để giải quyết vấn đề 2 quân đi vào cùng 1 ô, bạn cần phải sắp xếp danh sách PieceLogic để quân nào gần vòng xoáy(tức độ dài vector ngắn hơn) được đi trước
namespace Game.Effects.RegionalEffect
{
    public class Whirlpool : RegionalEffect
    {

        private readonly int startTurn = 4;
        private readonly List<int> centralIndices;
        public Whirlpool() : base(RegionalEffectType.Whirpool)
        {
            // Use MaxLength to derive center coordinates (board is MaxLength x MaxLength)
            var half = MaxLength / 2;
            // Hố xoáy 2x2 nên chiếm 4 vị trí (rank, file)
            var centralPos1 = new List<(int, int)>(){
                (half - 1, half - 1),
                (half - 1, half),
                (half, half - 1),
                (half, half)
            };
            AssetManager.Ins.regionalsData.CreateWhirlPool(FromRankFileToWorldPos(half - 1.0f / 2, half - 1.0f / 2));
            centralIndices = centralPos1.Select(p => IndexOf(p.Item1, p.Item2)).ToList();
        }
        protected override void ApplyEffect(int currentTurn)
        {
            if (currentTurn < startTurn)
            {
                return;
            }

            // Precompute their indices

            var board = MatchManager.Ins.GameState.PieceBoard;
            var pieces = board.Where(piece => piece != null).ToList();

            // Build list of (piece, nearestCentralIndex, distance) and sort by distance asc
            var pieceInfos = new List<(PieceLogic piece, int nearestCentralIndex, int distance)>();
            foreach (var piece in pieces)
            {
                int pos = piece.Pos;
                // find nearest central index by chebyshev distance
                var bestIdx = centralIndices[0];
                var bestDist = Distance(pos, bestIdx);
                foreach (var c in centralIndices)
                {
                    var d = Distance(pos, c);
                    if (d < bestDist)
                    {
                        bestDist = d;
                        bestIdx = c;
                    }
                }
                pieceInfos.Add((piece, bestIdx, bestDist));
            }

            // Sort so pieces closer to whirlpool move first
            pieceInfos.Sort((a, b) => a.distance.CompareTo(b.distance));

            foreach (var info in pieceInfos)
            {
                var piece = info.piece;
                if (piece == null || IsAlive(piece)) continue;

                int fromIndex = piece.Pos;
                // If piece already on whirlpool -> destroy it
                if (centralIndices.Contains(fromIndex))
                {
                    ActionManager.ExecuteImmediately(new KillPiece(piece.Pos));
                    continue;
                }

                var rank = RankOf(fromIndex);
                var file = FileOf(fromIndex);

                var targetIdx = info.nearestCentralIndex;
                var targetRank = RankOf(targetIdx);
                var targetFile = FileOf(targetIdx);

                var dr = targetRank - rank;
                var df = targetFile - file;

                // Compute one-step move direction
                var stepRank = dr == 0 ? 0 : (dr > 0 ? 1 : -1);
                var stepFile = df == 0 ? 0 : (df > 0 ? 1 : -1);

                // If vector is NOT aligned vertically, horizontally or perfectly diagonal,
                // round to the dominant cardinal direction (no diagonal rounding).
                var isAligned = (dr == 0) || (df == 0) || (Mathf.Abs(dr) == Mathf.Abs(df));
                if (!isAligned)
                {
                    if (Mathf.Abs(dr) > Mathf.Abs(df))
                    {
                        // move vertically only
                        stepFile = 0;
                    }
                    else
                    {
                        // move horizontally only
                        stepRank = 0;
                    }
                }

                var nextRank = rank + stepRank;
                var nextFile = file + stepFile;

                // Verify bounds and active square
                if (!VerifyBounds(nextRank) || !VerifyBounds(nextFile)) continue;
                var nextIndex = IndexOf(nextRank, nextFile);

                // Only move into empty squares, if a cell have piece, the move action is cancelled
                if (MatchManager.Ins.GameState.PieceBoard[nextIndex] != null) continue;

                // Execute immediate move
                ActionManager.ExecuteImmediately(new NormalMove((ushort)fromIndex, (ushort)nextIndex));

                // If the piece lands in a whirlpool cell, destroy it
                if (centralIndices.Contains(nextIndex))
                {
                    ActionManager.ExecuteImmediately(new KillPiece(piece.Pos));
                }
            }
        }
    }
}