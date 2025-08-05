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

namespace Game.Piece.PieceLogic.Elites
{
    public class ElectricEel: PieceLogic, IPieceWithSkill
    {
        public ElectricEel(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ElectricEelVengeful(this)));
        }

        private void Quiets(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(Pos);
            
            for (var i = file - EffectiveMoveRange + 1; i <= file + EffectiveMoveRange - 1; i++)
            {
                if (i == file || !VerifyBounds(i)) continue;
                var posTo = IndexOf(rank, i);
                
                if (PieceOn(posTo) != null ||
                    !IsActive(posTo)) continue;

                if (Pathfinder.LineBlocker(rank, file,
                        rank, i).Item1 != -1) continue;
                
                list.Add(new NormalMove(Pos, posTo));
            }
            
            for (var i = 1; i <= EffectiveMoveRange; i++)
            {
                var rankOffFront = rank + i;
                var rankOffBack = rank - i;

                for (var j = -i; j <= i; j++)
                {
                    var fileOff = file + j;
                    if (!VerifyBounds(fileOff)) continue;
                    if (VerifyBounds(rankOffFront))
                    {
                        var posOffFront = IndexOf(rankOffFront, fileOff);

                        if (Pathfinder.LineBlocker(rank, file,
                                rankOffFront, fileOff).Item1 == -1)
                        {
                            if (PieceOn(posOffFront) == null &&
                                IsActive(posOffFront))
                            {
                                list.Add(new NormalMove(Pos, posOffFront));
                            }
                        }
                    }

                    if (!VerifyBounds(rankOffBack)) continue;

                    var posOffBack = IndexOf(rankOffBack, fileOff);

                    if (Pathfinder.LineBlocker(rank, file,
                            rankOffBack, fileOff).Item1 != -1) continue;
                    
                    if (PieceOn(posOffBack) == null &&
                        IsActive(posOffBack))
                    {
                        list.Add(new NormalMove(Pos, posOffBack));
                    }
                }
            }
        }

        private void Captures(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(Pos);
            var push = !Color ? -1 : 1;
            
            for (var i = file - AttackRange + 1; i <= file + AttackRange - 1; i++)
            {
                if (i == file || !VerifyBounds(i)) continue;
                var posTo = IndexOf(rank, i);

                var p = PieceOn(posTo);

                if (p == null || p.Color == Color) continue;
                
                 if (Pathfinder.LineBlocker(rank, file,
                         rank, i).Item1 != -1) continue;
                
                list.Add(new NormalCapture(Pos, posTo));
            }

            for (var rankOff = push; Math.Abs(rankOff) <= AttackRange; rankOff += push)
            {
                var rankAfter = rank + rankOff;
                if (!VerifyBounds(rankAfter)) break;
                
                for (var j = file - AttackRange + Math.Abs(rankOff); j <= file + AttackRange - Math.Abs(rankOff); j++)
                {
                    if (!VerifyBounds(j)) continue;
                    
                    var posTo = IndexOf(rankAfter, j);
                    var p = PieceOn(posTo);
                    
                    if (p == null || p.Color == Color) continue;
                    
                     if (Pathfinder.LineBlocker(rank, file,
                             rank, j).Item1 != -1) continue;
                
                    list.Add(new NormalCapture(Pos, posTo));
                    
                }
            }
        }

        private void Skills(List<Action.Action> list)
        {
            if (SkillCooldown == 0)
            {
                list.Add(new ElectricEelActive(Pos));
            }
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Captures(list);
            Quiets(list);
            Skills(list);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}