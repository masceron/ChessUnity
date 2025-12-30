using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AmbushPredatorMoves : BaseMovePattern
    {
        /*public static void Quiets(List<Action.Action> list, int pos)
        {
            var rank = RankOf(pos);
            var file = FileOf(pos);
            var piece = PieceOn(pos);
            var effectiveMoveRange = piece.GetMoveRange(ref index);
            
            for (var rankOff = rank - 1; rankOff >= 0 && rank - rankOff <= effectiveMoveRange; rankOff--)
            {
                if (!MakeStraightQuiets(IndexOf(rankOff, file))) break;
            }
            
            for (var rankOff = rank + 1; VerifyUpperBound(rankOff) && rankOff - rank <= effectiveMoveRange; rankOff++)
            {
                if (!MakeStraightQuiets(IndexOf(rankOff, file))) break;
            }
            
            for (var fileOff = file - 1; fileOff >= 0 && file - fileOff <= effectiveMoveRange; fileOff--)
            {
                if (!MakeStraightQuiets(IndexOf(rank, fileOff))) break;
            }
            
            for (var fileOff = file + 1; VerifyUpperBound(fileOff) && fileOff - file <= effectiveMoveRange; fileOff++)
            {
                if (!MakeStraightQuiets(IndexOf(rank, fileOff))) break;
            }
            
            if (effectiveMoveRange <= 1) return;
            
            var rankTo = rank - effectiveMoveRange;
            if (rankTo >= 0)
            {
                for (var tFileTo = file - effectiveMoveRange + 1; tFileTo <= file + effectiveMoveRange - 1; tFileTo++)
                {
                    if (!VerifyBounds(tFileTo)) continue;
                    var idx = IndexOf(rankTo, tFileTo);
                    
                    if (!IsActive(idx) || PieceOn(idx) != null) continue;
                    if (Pathfinder.LineBlocker(rank, file, rankTo, tFileTo).Item1 != -1) continue;
                    
                    list.Add(new NormalMove(pos, idx));
                }
            }

            //Down
            rankTo = rank + effectiveMoveRange;
            if (VerifyUpperBound(rankTo))
            {
                for (var tFileTo = file - effectiveMoveRange + 1; tFileTo <= file + effectiveMoveRange - 1; tFileTo++)
                {
                    if (!VerifyBounds(tFileTo)) continue;
                    var idx = IndexOf(rankTo, tFileTo);
                    
                    if (!IsActive(idx) || PieceOn(idx) != null) continue;
                    if (Pathfinder.LineBlocker(rank, file, rankTo, tFileTo).Item1 != -1) continue;
                    
                    list.Add(new NormalMove(pos, idx));
                }
            }

            //Left
            var fileTo = file - effectiveMoveRange;
            if (fileTo >= 0)
            {
                for (var tRankTo = rank - effectiveMoveRange + 1; tRankTo <= rank + effectiveMoveRange - 1; tRankTo++)
                {
                    if (!VerifyBounds(tRankTo)) continue;
                    var idx = IndexOf(tRankTo, fileTo);
                    
                    if (!IsActive(idx) || PieceOn(idx) != null) continue;
                    if (Pathfinder.LineBlocker(rank, file, tRankTo, fileTo).Item1 != -1) continue;
                    
                    list.Add(new NormalMove(pos, idx));
                }
            }

            //Right
            fileTo = file + effectiveMoveRange;
            if (VerifyUpperBound(fileTo))
            {
                for (var tRankTo = rank - effectiveMoveRange + 1; tRankTo <= rank + effectiveMoveRange - 1; tRankTo++)
                {
                    if (!VerifyBounds(tRankTo)) continue;
                    var idx = IndexOf(tRankTo, fileTo);
                    
                    if (!IsActive(idx) || PieceOn(idx) != null) continue;
                    if (Pathfinder.LineBlocker(rank, file, tRankTo, fileTo).Item1 != -1) continue;
                    
                    list.Add(new NormalMove(pos, idx));
                }
            }

            return;
            
            bool MakeStraightQuiets(int index)
            {
                if (!IsActive(index) || PieceOn(index) != null) return false;
            
                list.Add(new NormalMove(pos, index));
                return true;
            }
        }
        public static void Captures(List<Action.Action> list, int pos)
        {
            var rank = RankOf(pos);
            var file = FileOf(pos);
            var piece = PieceOn(pos);
            var color = piece.Color;
            var effectiveMoveRange = piece.AttackRange;
            
            for (var rankOff = rank - 1; rankOff >= 0 && rank - rankOff <= effectiveMoveRange; rankOff--)
            {
                if (!MakeStraightCaptures(IndexOf(rankOff, file))) break;
            }
            
            for (var rankOff = rank + 1; VerifyUpperBound(rankOff) && rankOff - rank <= effectiveMoveRange; rankOff++)
            {
                if (!MakeStraightCaptures(IndexOf(rankOff, file))) break;
            }
            
            for (var fileOff = file - 1; fileOff >= 0 && file - fileOff <= effectiveMoveRange; fileOff--)
            {
                if (!MakeStraightCaptures(IndexOf(rank, fileOff))) break;
            }
            
            for (var fileOff = file + 1; VerifyUpperBound(fileOff) && fileOff - file <= effectiveMoveRange; fileOff++)
            {
                if (!MakeStraightCaptures(IndexOf(rank, fileOff))) break;
            }
            
            if (effectiveMoveRange <= 1) return;
            
            var rankTo = rank - effectiveMoveRange;
            if (rankTo >= 0)
            {
                for (var tFileTo = file - effectiveMoveRange + 1; tFileTo <= file + effectiveMoveRange - 1; tFileTo++)
                {
                    if (!VerifyBounds(tFileTo)) continue;
                    var idx = IndexOf(rankTo, tFileTo);
                    
                    if (!IsActive(idx)) continue;
                    var targetPiece = PieceOn(idx);
                    if (targetPiece == null || targetPiece.Color == color) continue;
                    if (Pathfinder.LineBlocker(rank, file, rankTo, tFileTo).Item1 != -1) continue;
                    
                    list.Add(new NormalCapture(pos, idx));
                }
            }

            //Down
            rankTo = rank + effectiveMoveRange;
            if (VerifyUpperBound(rankTo))
            {
                for (var tFileTo = file - effectiveMoveRange + 1; tFileTo <= file + effectiveMoveRange - 1; tFileTo++)
                {
                    if (!VerifyBounds(tFileTo)) continue;
                    var idx = IndexOf(rankTo, tFileTo);
                    
                    if (!IsActive(idx)) continue;
                    var targetPiece = PieceOn(idx);
                    if (targetPiece == null || targetPiece.Color == color) continue;
                    if (Pathfinder.LineBlocker(rank, file, rankTo, tFileTo).Item1 != -1) continue;
                    
                    list.Add(new NormalCapture(pos, idx));
                }
            }

            //Left
            var fileTo = file - effectiveMoveRange;
            if (fileTo >= 0)
            {
                for (var tRankTo = rank - effectiveMoveRange + 1; tRankTo <= rank + effectiveMoveRange - 1; tRankTo++)
                {
                    if (!VerifyBounds(tRankTo)) continue;
                    var idx = IndexOf(tRankTo, fileTo);
                    
                    if (!IsActive(idx)) continue;
                    var targetPiece = PieceOn(idx);
                    if (targetPiece == null || targetPiece.Color == color) continue;
                    if (Pathfinder.LineBlocker(rank, file, tRankTo, fileTo).Item1 != -1) continue;
                    
                    list.Add(new NormalCapture(pos, idx));
                }
            }

            //Right
            fileTo = file + effectiveMoveRange;
            if (VerifyUpperBound(fileTo))
            {
                for (var tRankTo = rank - effectiveMoveRange + 1; tRankTo <= rank + effectiveMoveRange - 1; tRankTo++)
                {
                    if (!VerifyBounds(tRankTo)) continue;
                    var idx = IndexOf(tRankTo, fileTo);
                    
                    if (!IsActive(idx)) continue;
                    var targetPiece = PieceOn(idx);
                    if (targetPiece == null || targetPiece.Color == color) continue;
                    if (Pathfinder.LineBlocker(rank, file, tRankTo, fileTo).Item1 != -1) continue;
                    
                    list.Add(new NormalCapture(pos, idx));
                }
            }

            return;
            
            bool MakeStraightCaptures(int index)
            {
                if (!IsActive(index)) return false;
                var p = PieceOn(index);
                if (p == null) return true;
                if (p.Color != color)
                {
                    list.Add(new NormalCapture(pos, index));
                }
                return false;
            }
        }*/

        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateAmbushPredatorPattern(makerPos);
        }

        private List<int> GenerateAmbushPredatorPattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            var push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 2, file + 1));
            positions.Add(IndexOf(rank + push * 2, file));
            positions.Add(IndexOf(rank + push * 2, file - 1));

            positions.Add(IndexOf(rank + push * 1, file + 2));
            positions.Add(IndexOf(rank + push * 1, file));
            positions.Add(IndexOf(rank + push * 1, file - 2));

            positions.Add(IndexOf(rank, file + 2));
            positions.Add(IndexOf(rank, file + 1));
            positions.Add(IndexOf(rank, file - 1));
            positions.Add(IndexOf(rank, file - 2));

            positions.Add(IndexOf(rank - push * 1, file + 2));
            positions.Add(IndexOf(rank - push * 1, file));
            positions.Add(IndexOf(rank - push * 1, file - 2));

            positions.Add(IndexOf(rank - push * 2, file + 1));
            positions.Add(IndexOf(rank - push * 2, file));
            positions.Add(IndexOf(rank - push * 2, file - 1));

            return positions;
        }

        public static int Quiets(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new AmbushPredatorMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false, excludeEmptyTile: excludeEmptyTile);
            return 40 + 5 * moveRange;
        }

        public static int Captures(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var attackRange = PieceOn(pos).AttackRange();
            var basePattern = new HashSet<int>(new AmbushPredatorMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true, excludeEmptyTile: excludeEmptyTile);
            return 40 + 5 * attackRange;
        }

    }


}