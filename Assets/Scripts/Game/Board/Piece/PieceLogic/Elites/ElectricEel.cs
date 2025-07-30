using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Action.Skills;
using Game.Board.Effects.Buffs;
using Game.Board.General;
using Game.Common;
using Color = Game.Board.General.Color;
using static Game.Common.BoardUtils;

namespace Game.Board.Piece.PieceLogic.Elites
{
    public class ElectricEel: PieceLogic
    {
        public ElectricEel(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Vengeful(this)));
        }

        private void Quiets(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(Pos);
            
            for (var i = file - EffectiveMoveRange + 1; i <= file + EffectiveMoveRange - 1; i++)
            {
                if (i == file || !VerifyBounds(i)) continue;
                var posTo = IndexOf(rank, i);
                
                if (MatchManager.gameState.PieceBoard[posTo] != null ||
                    !MatchManager.gameState.ActiveBoard[posTo]) continue;

                if (Pathfinder.LineBlocker(rank, file,
                        rank, i,
                        MatchManager.gameState.PieceBoard).Item1 != -1) continue;
                
                list.Add(new NormalMove(Pos, Pos, posTo));
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
                                rankOffFront, fileOff,
                                MatchManager.gameState.PieceBoard).Item1 == -1)
                        {
                            if (MatchManager.gameState.PieceBoard[posOffFront] == null &&
                                MatchManager.gameState.ActiveBoard[posOffFront])
                            {
                                list.Add(new NormalMove(Pos, Pos, posOffFront));
                            }
                        }
                    }

                    if (!VerifyBounds(rankOffBack)) continue;

                    var posOffBack = IndexOf(rankOffBack, fileOff);

                    if (Pathfinder.LineBlocker(rank, file,
                            rankOffBack, fileOff,
                            MatchManager.gameState.PieceBoard).Item1 != -1) continue;
                    
                    if (MatchManager.gameState.PieceBoard[posOffBack] == null &&
                        MatchManager.gameState.ActiveBoard[posOffBack])
                    {
                        list.Add(new NormalMove(Pos, Pos, posOffBack));
                    }
                }
            }
        }

        private void Captures(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(Pos);
            var push = Color == Color.White ? -1 : 1;
            
            for (var i = file - AttackRange + 1; i <= file + AttackRange - 1; i++)
            {
                if (i == file || !VerifyBounds(i)) continue;
                var posTo = IndexOf(rank, i);

                var p = MatchManager.gameState.PieceBoard[posTo];

                if (p == null || p.Color == Color) continue;
                
                 if (Pathfinder.LineBlocker(rank, file,
                         rank, i,
                         MatchManager.gameState.PieceBoard).Item1 != -1) continue;
                
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
                    var p = MatchManager.gameState.PieceBoard[posTo];
                    
                    if (p == null || p.Color == Color) continue;
                    
                     if (Pathfinder.LineBlocker(rank, file,
                             rank, j,
                             MatchManager.gameState.PieceBoard).Item1 != -1) continue;
                
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

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            
            Captures(list);
            Quiets(list);
            Skills(list);

            return list;
        }
    }
}