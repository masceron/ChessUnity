using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Velkaris: PieceLogic, IPieceWithSkill
    {
        public PieceLogic Marked;

        public Velkaris(PieceConfig cfg) : base(cfg)
        {
            Marked = null;
            SkillCooldown = -1;
            ActionManager.ExecuteImmediately(new ApplyEffect(new VelkarisMarker(this)));
        }

        private bool MakeMove(List<Action.Action> list, int index, int distance)
        {
            if (!IsActive(index)) return false;
            var p = PieceOn(index);
            if (p != null)
            {
                if (p.Color != Color || distance > AttackRange) return false;
                
                list.Add(new NormalCapture(Pos, index));
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
            var (rank, file) = RankFileOf(Pos);
            
            var maxRange = Math.Max(EffectiveMoveRange, AttackRange);
            
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
            
            if (SkillCooldown == 0 && Marked != null)
            {
                list.Add(new VelkarisKill(Pos, Pos, Marked.Pos));
            }
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}