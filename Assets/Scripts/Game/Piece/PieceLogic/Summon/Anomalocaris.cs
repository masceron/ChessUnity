using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using Game.Effects.Buffs;
using static Game.Common.BoardUtils;
using SnappingStrike = Game.Effects.Traits.SnappingStrike;

namespace Game.Piece.PieceLogic.Summon
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

        private void MakeSkill(List<Action.Action> list, int index)
        {
            var piece = PieceOn(index);
            if (piece == null || piece.Color == Color) return;
            list.Add(new AnomalocarisActive(Pos, index));
        }

        private void Skill(List<Action.Action> list)
        {
            if (SkillCooldown != 0) return;
            
            var (rank, file) = RankFileOf(Pos);

            foreach (var (rankOff, fileOff) in MoveEnumerators.Around(rank, file, 5))
            {
                MakeSkill(list, IndexOf(rankOff, fileOff));
            }
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            var rank = RankOf(Pos);
            var file = FileOf(Pos);
            var maxRange = Math.Max(AttackRange, EffectiveMoveRange);
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, maxRange))
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, maxRange))
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rankOff - rank)) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, maxRange))
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), file - fileOff)) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, maxRange))
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), fileOff - file)) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, maxRange))
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, maxRange))
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rankOff - rank)) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, maxRange))
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, maxRange))
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rankOff - rank)) break;
            }
            
            Skill(list);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}