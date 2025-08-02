using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Internal.Pending;
using Game.Board.Action.Quiets;
using Game.Board.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Board.Piece.PieceLogic.Commanders
{
    public class Thalassos: PieceLogic, IPieceWithSkill
    {
        public Thalassos(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ThalassosShielder(this)));
        }
        
        private bool MakeMove(List<Action.Action> list, int index, int distance)
        {
            if (!IsActive(index)) return false;
            var pieceOn = PieceOn(index);
            if (pieceOn != null)
            {
                if (pieceOn.Color != Color && distance <= AttackRange)
                {
                    list.Add(new NormalCapture(Pos, index));
                }

                return false;
            }

            if (distance <= EffectiveMoveRange)
            {
                list.Add(new NormalMove(Pos, index));
            }

            return true;
        }

        private void Skills(List<Action.Action> list)
        {
            if (SkillCooldown != 0) return;

            for (var rankOff = -1; rankOff <= 1; rankOff++)
            {
                var rank = RankOf(Pos) + rankOff;
                if (!VerifyBounds(rank)) continue;
                
                for (var fileOff = -1; fileOff <= 1; fileOff++)
                {
                    if (rankOff == 0 && fileOff == 0) continue;
                    var file = FileOf(Pos) + fileOff;
                    if (!VerifyBounds(file)) continue;
                    var posTo = IndexOf(rank, file);

                    if (PieceOn(posTo) == null)
                    {
                        list.Add(new ThalassosResurrectCandidate(Pos, posTo));
                    }
                }
            }
            
        }

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            var rank = RankOf(Pos);
            var file = FileOf(Pos);
            var push = !Color ? -1 : 1;
            
            var maxRange = Math.Max(AttackRange, EffectiveMoveRange);
            
            for (var fileOff = file - 1; fileOff >= 0 && file - fileOff <= maxRange; fileOff--)
            {
                if (!MakeMove(list, IndexOf(rank, fileOff), file - fileOff)) break;
            }
            
            for (var fileOff = file + 1; VerifyUpperBound(fileOff) && fileOff - file <= maxRange; fileOff++)
            {
                if (!MakeMove(list, IndexOf(rank, fileOff), fileOff - file)) break;
            }
            
            for (var rankOff = rank + push; VerifyBounds(rankOff) && Math.Abs(rank - rankOff) <= maxRange; rankOff += push)
            {
                if (!MakeMove(list, IndexOf(rankOff, file), Math.Abs(rank - rankOff))) break;
            }
            
            for (int rankOff = rank + push, fileOff = file - 1;
                 VerifyBounds(rankOff) && VerifyUpperBound(fileOff) && Math.Abs(rank - rankOff) <= maxRange && fileOff - file <= maxRange;
                 rankOff += push, fileOff--)
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            for (int rankOff = rank + push, fileOff = file + 1;
                 VerifyBounds(rankOff) && VerifyUpperBound(fileOff) && Math.Abs(rank - rankOff) <= maxRange && fileOff - file <= maxRange;
                 rankOff += push, fileOff++)
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }

            Skills(list);

            return list;
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}