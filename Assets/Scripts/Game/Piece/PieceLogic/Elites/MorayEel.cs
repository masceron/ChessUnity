using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Data.Pieces;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Elites
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
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, AttackRange))
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, AttackRange))
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, AttackRange))
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, AttackRange))
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, AttackRange))
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, AttackRange))
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, AttackRange))
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, AttackRange))
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
                    if (Pathfinder.LineBlocker(rank, file, rankTo, tFileTo).Item1 != -1) continue;
                    
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
                    if (Pathfinder.LineBlocker(rank, file, rankTo, tFileTo).Item1 != -1) continue;
                    
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
                    if (Pathfinder.LineBlocker(rank, file, tRankTo, fileTo).Item1 != -1) continue;
                    
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
                    if (Pathfinder.LineBlocker(rank, file, tRankTo, fileTo).Item1 != -1) continue;
                    
                    list.Add(new NormalMove(Pos, idx));
                }
            }
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list);
            Capture(list);
        }
    }
}