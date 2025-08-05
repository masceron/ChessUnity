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

        protected override void MoveToMake(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(Pos);
            
            var totalRange = Math.Max(EffectiveMoveRange, AttackRange);
            
            for (var rankOff = 1; rankOff <= totalRange; rankOff++)
            {
                var rankAfter = rank + rankOff;
                if (!VerifyUpperBound(rankAfter)) break;
                var newPos = IndexOf(rankAfter, file);
                
                if (!IsActive(newPos)) break;
                var p = PieceOn(newPos);
                if (p != null)
                {
                    if (p.Color != Color && rankOff <= AttackRange) list.Add(new NormalCapture(Pos, newPos));
                    break;
                }
                if (rankOff <= EffectiveMoveRange)
                {
                    list.Add(new NormalMove(Pos, newPos));
                }
            }
            
            for (var rankOff = -1; rankOff >= -totalRange; rankOff--)
            {
                var rankAfter = rank + rankOff;
                if (rankAfter < 0) break;
                var newPos = IndexOf(rankAfter, file);
                
                if (!IsActive(newPos)) break;
                var p = PieceOn(newPos);
                if (p != null)
                {
                    if (p.Color != Color && rankOff >= -AttackRange) list.Add(new NormalCapture(Pos, newPos));
                    break;
                }
                if (rankOff >= -EffectiveMoveRange)
                {
                    list.Add(new NormalMove(Pos, newPos));
                }
            }
            
            for (var fileOff = 1; fileOff <= totalRange; fileOff++)
            {
                var fileAfter = file + fileOff;
                if (!VerifyUpperBound(fileAfter)) break;
                var newPos = IndexOf(rank, fileAfter);
                
                if (!IsActive(newPos)) break;
                var p = PieceOn(newPos);
                if (p != null)
                {
                    if (p.Color != Color && fileOff <= AttackRange) list.Add(new NormalCapture(Pos, newPos));
                    break;
                }
                if (fileOff <= EffectiveMoveRange)
                {
                    list.Add(new NormalMove(Pos, newPos));
                }
            }
            
            for (var fileOff = -1; fileOff >= -totalRange; fileOff--)
            {
                var fileAfter = file + fileOff;
                if (fileAfter < 0) break;
                var newPos = IndexOf(rank, fileAfter);
                
                if (!IsActive(newPos)) break;
                var p = PieceOn(newPos);
                if (p != null)
                {
                    if (p.Color != Color && fileOff >= -AttackRange) list.Add(new NormalCapture(Pos, newPos));
                    break;
                }
                if (fileOff >= -EffectiveMoveRange)
                {
                    list.Add(new NormalMove(Pos, newPos));
                }
            }
            
            if (SkillCooldown == 0 && Marked != null)
            {
                list.Add(new VelkarisKill(Pos, Pos, Marked.Pos));
            }
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}