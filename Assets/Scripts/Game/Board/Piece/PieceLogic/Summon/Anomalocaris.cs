using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Action.Skills;
using Game.Board.Effects.Buffs;
using static Game.Common.BoardUtils;
using SnappingStrike = Game.Board.Effects.Traits.SnappingStrike;

namespace Game.Board.Piece.PieceLogic.Summon
{
    public class Anomalocaris: PieceLogic, IPieceWithSkill
    {
        public Anomalocaris(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new HardenedShield(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SnappingStrike(this)));
        }

        private bool MakeMove(List<Action.Action> list, int index, int distance)
        {
            if (!IsActive(index)) return false;
            var pieceOn = PieceOn(index);
            if (pieceOn != null)
            {
                if (pieceOn.Color != Color && distance <= AttackRange)
                {
                    list.Add(new Action.Captures.SnappingStrike(Pos, index));
                }

                return false;
            }

            if (distance <= EffectiveMoveRange)
            {
                list.Add(new NormalMove(Pos, index));
            }

            return true;
        }

        private void Skill(List<Action.Action> list)
        {
            if (SkillCooldown != 0) return;
            
            var (rank, file) = RankFileOf(Pos);

            //Up
            var rankTo = rank - 5;
            if (rankTo >= 0)
            {
                for (var tFileTo = file - 5; tFileTo <= file + 5; tFileTo++)
                {
                    if (!VerifyBounds(tFileTo)) continue;
                    var idx = IndexOf(rankTo, tFileTo);
                    var piece = PieceOn(idx);
                    if (piece == null || piece.Color == Color) continue;
                    list.Add(new AnomalocarisActive(Pos, idx));
                }
            }

            //Down
            rankTo = rank + 5;
            if (VerifyUpperBound(rankTo))
            {
                for (var tFileTo = file - 5; tFileTo <= file + 5 - 1; tFileTo++)
                {
                    if (!VerifyBounds(tFileTo)) continue;
                    var idx = IndexOf(rankTo, tFileTo);
                    var piece = PieceOn(idx);
                    if (piece == null || piece.Color == Color) continue;
                    list.Add(new AnomalocarisActive(Pos, idx));
                }
            }

            //Left
            var fileTo = file - 5;
            if (fileTo >= 0)
            {
                for (var tRankTo = rank - 5 + 1; tRankTo <= rank + 5 - 1; tRankTo++)
                {
                    if (!VerifyBounds(tRankTo)) continue;
                    var idx = IndexOf(tRankTo, fileTo);
                    var piece = PieceOn(idx);
                    if (piece == null || piece.Color == Color) continue;
                    list.Add(new AnomalocarisActive(Pos, idx));
                }
            }

            //Right
            fileTo = file + 5;
            if (VerifyUpperBound(fileTo))
            {
                for (var tRankTo = rank - 5 + 1; tRankTo <= rank + 5; tRankTo++)
                {
                    if (!VerifyBounds(tRankTo)) continue;
                    var idx = IndexOf(tRankTo, fileTo);
                    var piece = PieceOn(idx);
                    if (piece == null || piece.Color == Color) continue;
                    list.Add(new AnomalocarisActive(Pos, idx));
                }
            }
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            var rank = RankOf(Pos);
            var file = FileOf(Pos);
            var maxRange = Math.Max(AttackRange, EffectiveMoveRange);
            
            for (var rankOff = rank - 1; rankOff >= 0 && rank - rankOff <= maxRange; rankOff--)
            {
                if (!MakeMove(list, IndexOf(rankOff, file), rank - rankOff)) break;
            }
            
            for (var rankOff = rank + 1; VerifyUpperBound(rankOff) && rankOff - rank <= maxRange; rankOff++)
            {
                if (!MakeMove(list, IndexOf(rankOff, file), rankOff - rank)) break;
            }
            
            for (var fileOff = file - 1; fileOff >= 0 && file - fileOff <= maxRange; fileOff--)
            {
                if (!MakeMove(list, IndexOf(rank, fileOff), file - fileOff)) break;
            }
            
            for (var fileOff = file + 1; VerifyUpperBound(fileOff) && fileOff - file <= maxRange; fileOff++)
            {
                if (!MakeMove(list, IndexOf(rank, fileOff), fileOff - file)) break;
            }
            
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
            
            Skill(list);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}