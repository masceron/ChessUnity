using System;
using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using Game.Effects;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;
using SnappingStrike = Game.Action.Captures.SnappingStrike;

namespace Game.Piece.PieceLogic.Champions
{
    public class Swordfish: PieceLogic, IPieceWithSkill
    {
        public Swordfish(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Piercing(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SwordfishAttack(this)));
        }

        private bool snap;

        private bool MakeMove(List<Action.Action> list, int index, int distance)
        {
            if (!IsActive(index)) return false;
            var pieceOn = PieceOn(index);
            if (pieceOn != null)
            {
                if (pieceOn.Color == Color || distance > AttackRange) return false;
                
                if (snap)
                {
                    list.Add(new SnappingStrike(Pos, index));
                }
                else list.Add(new NormalCapture(Pos, index));

                return false;
            }

            if (distance <= EffectiveMoveRange)
            {
                list.Add(new NormalMove(Pos, index));
            }

            return true;
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            var rank = RankOf(Pos);
            var file = FileOf(Pos);
            var maxRange = Math.Max(AttackRange, EffectiveMoveRange);
            snap = Effects.Any(e => e.EffectName == EffectName.SnappingStrike);

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
            
            if (SkillCooldown == 0) list.Add(new SwordFishActive(Pos));
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}