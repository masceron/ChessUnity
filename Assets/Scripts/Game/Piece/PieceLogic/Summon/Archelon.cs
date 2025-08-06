using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Summon
{
    public class Archelon: PieceLogic, IPieceWithSkill
    {
        public Archelon(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new HardenedShield(this, 3)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new ArchelonDraw(this)));
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

        private void Skill(List<Action.Action> list)
        {
            if (SkillCooldown != 0) return;
            
            var (rank, file) = RankFileOf(Pos);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn == null || pOn == this) continue;
                if (pOn.Color == Color)
                {
                    list.Add(new ArchelonShield(Pos, index));
                }
            }
        }

        private void Moves(List<Action.Action> list)
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
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Moves(list);
            Skill(list);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}