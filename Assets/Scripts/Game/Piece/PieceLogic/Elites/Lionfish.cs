using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Data.Pieces;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Elites
{
    public class Lionfish: PieceLogic, IPieceWithSkill
    {
        public Lionfish(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new LionfishVengeful(this)));
        }

        private void Captures(List<Action.Action> list)
        {
            var file = FileOf(Pos);
            var rank = RankOf(Pos);

            //Up
            var rankTo = rank - AttackRange;
            if (rankTo >= 0)
            {
                for (var tFileTo = file - AttackRange; tFileTo <= file + AttackRange; tFileTo++)
                {
                    if (!VerifyBounds(tFileTo)) continue;
                    var idx = IndexOf(rankTo, tFileTo);
                    var piece = PieceOn(idx);
                    if (piece == null || piece.Color == Color) continue;
                    list.Add(new NormalCapture(Pos, idx));
                }
            }

            //Down
            rankTo = rank + AttackRange;
            if (VerifyUpperBound(rankTo))
            {
                for (var tFileTo = file - AttackRange; tFileTo <= file + AttackRange - 1; tFileTo++)
                {
                    if (!VerifyBounds(tFileTo)) continue;
                    var idx = IndexOf(rankTo, tFileTo);
                    var piece = PieceOn(idx);
                    if (piece == null || piece.Color == Color) continue;
                    list.Add(new NormalCapture(Pos, idx));
                }
            }

            //Left
            var fileTo = file - AttackRange;
            if (fileTo >= 0)
            {
                for (var tRankTo = rank - AttackRange + 1; tRankTo <= rank + AttackRange - 1; tRankTo++)
                {
                    if (!VerifyBounds(tRankTo)) continue;
                    var idx = IndexOf(tRankTo, fileTo);
                    var piece = PieceOn(idx);
                    if (piece == null || piece.Color == Color) continue;
                    list.Add(new NormalCapture(Pos, idx));
                }
            }

            //Right
            fileTo = file + AttackRange;
            if (VerifyUpperBound(fileTo))
            {
                for (var tRankTo = rank - AttackRange + 1; tRankTo <= rank + AttackRange; tRankTo++)
                {
                    if (!VerifyBounds(tRankTo)) continue;
                    var idx = IndexOf(tRankTo, fileTo);
                    var piece = PieceOn(idx);
                    if (piece == null || piece.Color == Color) continue;
                    list.Add(new NormalCapture(Pos, idx));
                }
            }
        }
        
        private bool MakeMove(List<Action.Action> list, int index, int distance)
        {
            if (!IsActive(index)) return false;
            var pieceOn = PieceOn(index);
            if (pieceOn != null)
            {
                return false;
            }

            if (distance <= EffectiveMoveRange)
            {
                list.Add(new NormalMove(Pos, index));
            }

            return true;
        }

        private void Quiets(List<Action.Action> list)
        {
            var rank = RankOf(Pos);
            var file = FileOf(Pos);
            var maxRange = Math.Max(AttackRange, EffectiveMoveRange);
            
            for (int rankOff = rank - 1, fileOff = file - 1;
                 rankOff >= 0 && fileOff >= 0 && rank - rankOff <= maxRange && file - fileOff <= maxRange;
                 rankOff--, fileOff--)
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            for (int rankOff = rank - 1, fileOff = file + 1;
                 rankOff >= 0 && VerifyUpperBound(fileOff) && rank - rankOff <= maxRange && fileOff - file <= maxRange;
                 rankOff--, fileOff++)
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            for (int rankOff = rank + 1, fileOff = file + 1;
                 VerifyBounds(rankOff) && VerifyUpperBound(fileOff) && rankOff - rank <= maxRange && fileOff - file <= maxRange;
                 rankOff++, fileOff++)
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            for (int rankOff = rank + 1, fileOff = file - 1;
                 VerifyBounds(rankOff) && fileOff >= 0 && rankOff - rank <= maxRange && file - fileOff <= maxRange;
                 rankOff++, fileOff--)
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list);
            Captures(list);
            if (SkillCooldown == 0) list.Add(new LionfishActive(Pos));
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}