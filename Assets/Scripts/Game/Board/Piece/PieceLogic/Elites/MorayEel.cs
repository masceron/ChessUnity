using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Effects.Traits;
using Game.Common;
using NUnit.Framework;
using static Game.Common.BoardUtils;

namespace Game.Board.Piece.PieceLogic.Elites
{
    public class MorayEel: PieceLogic
    {
        public MorayEel(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Ambush(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new MorayEelCamouflage(this)));
        }
        
        private bool MakeCapture(List<Action.Action> list, int index)
        {
            var pieceOn = PieceOn(index);
            if (pieceOn == null) return true;
            
            if (pieceOn.Color != Color)
            {
                list.Add(new NormalCapture(Pos, index));
            }
            return false;
        }

        private void Capture(List<Action.Action> list)
        {
            var rank = RankOf(Pos);
            var file = FileOf(Pos);
            
            for (var rankOff = rank - 1; rankOff >= 0 && rank - rankOff <= AttackRange; rankOff--)
            {
                if (!MakeCapture(list, IndexOf(rankOff, file))) break;
            }
            
            for (var rankOff = rank + 1; VerifyUpperBound(rankOff) && rankOff - rank <= AttackRange; rankOff++)
            {
                if (!MakeCapture(list, IndexOf(rankOff, file))) break;
            }
            
            for (var fileOff = file - 1; fileOff >= 0 && file - fileOff <= AttackRange; fileOff--)
            {
                if (!MakeCapture(list, IndexOf(rank, fileOff))) break;
            }
            
            for (var fileOff = file + 1; VerifyUpperBound(fileOff) && fileOff - file <= AttackRange; fileOff++)
            {
                if (!MakeCapture(list, IndexOf(rank, fileOff))) break;
            }
            
            for (int rankOff = rank - 1, fileOff = file - 1;
                 rankOff >= 0 && fileOff >= 0 && rank - rankOff <= AttackRange && file - fileOff <= AttackRange;
                 rankOff--, fileOff--)
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
            
            for (int rankOff = rank - 1, fileOff = file + 1;
                 rankOff >= 0 && VerifyUpperBound(fileOff) && rank - rankOff <= AttackRange && fileOff - file <= AttackRange;
                 rankOff--, fileOff++)
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
            
            for (int rankOff = rank + 1, fileOff = file + 1;
                 VerifyBounds(rankOff) && VerifyUpperBound(fileOff) && rankOff - rank <= AttackRange && fileOff - file <= AttackRange;
                 rankOff++, fileOff++)
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
            
            for (int rankOff = rank + 1, fileOff = file - 1;
                 VerifyBounds(rankOff) && fileOff >= 0 && rankOff - rank <= AttackRange && file - fileOff <= AttackRange;
                 rankOff++, fileOff--)
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
        }

        private bool MakeStraightQuiets(List<Action.Action> list, int index)
        {
            if (!IsActive(index) || PieceOn(index) != null) return false;
            
            list.Add(new NormalMove(Pos, index));
            return true;
        }

        private void Quiets(List<Action.Action> list)
        {
            var rank = RankOf(Pos);
            var file = FileOf(Pos);
            
            for (var rankOff = rank - 1; rankOff >= 0 && rank - rankOff <= EffectiveMoveRange; rankOff--)
            {
                if (!MakeStraightQuiets(list, IndexOf(rankOff, file))) break;
            }
            
            for (var rankOff = rank + 1; VerifyUpperBound(rankOff) && rankOff - rank <= EffectiveMoveRange; rankOff++)
            {
                if (!MakeStraightQuiets(list, IndexOf(rankOff, file))) break;
            }
            
            for (var fileOff = file - 1; fileOff >= 0 && file - fileOff <= EffectiveMoveRange; fileOff--)
            {
                if (!MakeStraightQuiets(list, IndexOf(rank, fileOff))) break;
            }
            
            for (var fileOff = file + 1; VerifyUpperBound(fileOff) && fileOff - file <= EffectiveMoveRange; fileOff++)
            {
                if (!MakeStraightQuiets(list, IndexOf(rank, fileOff))) break;
            }
            
            if (EffectiveMoveRange <= 1) return;
            
            var rankTo = rank - EffectiveMoveRange;
            if (rankTo >= 0)
            {
                for (var tFileTo = file - EffectiveMoveRange + 1; tFileTo <= file + EffectiveMoveRange - 1; tFileTo++)
                {
                    if (!VerifyBounds(tFileTo)) continue;
                    var idx = IndexOf(rankTo, tFileTo);
                    
                    if (!IsActive(idx) || PieceOn(idx) != null) continue;
                    if (Pathfinder.LineBlocker(rank, file, rankTo, tFileTo, PieceBoard()).Item1 != -1) continue;
                    
                    list.Add(new NormalMove(Pos, idx));
                }
            }

            //Down
            rankTo = rank + EffectiveMoveRange;
            if (VerifyUpperBound(rankTo))
            {
                for (var tFileTo = file - EffectiveMoveRange + 1; tFileTo <= file + EffectiveMoveRange - 1; tFileTo++)
                {
                    if (!VerifyBounds(tFileTo)) continue;
                    var idx = IndexOf(rankTo, tFileTo);
                    
                    if (!IsActive(idx) || PieceOn(idx) != null) continue;
                    if (Pathfinder.LineBlocker(rank, file, rankTo, tFileTo, PieceBoard()).Item1 != -1) continue;
                    
                    list.Add(new NormalMove(Pos, idx));
                }
            }

            //Left
            var fileTo = file - EffectiveMoveRange;
            if (fileTo >= 0)
            {
                for (var tRankTo = rank - EffectiveMoveRange + 1; tRankTo <= rank + EffectiveMoveRange - 1; tRankTo++)
                {
                    if (!VerifyBounds(tRankTo)) continue;
                    var idx = IndexOf(tRankTo, fileTo);
                    
                    if (!IsActive(idx) || PieceOn(idx) != null) continue;
                    if (Pathfinder.LineBlocker(rank, file, tRankTo, fileTo, PieceBoard()).Item1 != -1) continue;
                    
                    list.Add(new NormalMove(Pos, idx));
                }
            }

            //Right
            fileTo = file + EffectiveMoveRange;
            if (VerifyUpperBound(fileTo))
            {
                for (var tRankTo = rank - EffectiveMoveRange + 1; tRankTo <= rank + EffectiveMoveRange - 1; tRankTo++)
                {
                    if (!VerifyBounds(tRankTo)) continue;
                    var idx = IndexOf(tRankTo, fileTo);
                    
                    if (!IsActive(idx) || PieceOn(idx) != null) continue;
                    if (Pathfinder.LineBlocker(rank, file, tRankTo, fileTo, PieceBoard()).Item1 != -1) continue;
                    
                    list.Add(new NormalMove(Pos, idx));
                }
            }
        }

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            
            Quiets(list);
            Capture(list);

            return list;
        }
    }
}