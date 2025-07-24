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
using UnityEngine;
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
            var (rank, file) = RankFileOf(pos);
            
            for (var i = file - effectiveMoveRange + 1; i <= file + effectiveMoveRange - 1; i++)
            {
                if (i == file || !VerifyBounds(i)) continue;
                var posTo = IndexOf(rank, i);
                
                if (MatchManager.gameState.MainBoard[posTo] != null ||
                    !MatchManager.gameState.ActiveBoard[posTo]) continue;

                if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                        new Vector2Int(rank, i),
                        MatchManager.gameState.MainBoard) != -Vector2Int.one) continue;
                
                list.Add(new NormalMove(pos, pos, posTo));
            }
            
            for (var i = 1; i <= effectiveMoveRange; i++)
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

                        if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                                new Vector2Int(rankOffFront, fileOff),
                                MatchManager.gameState.MainBoard) == -Vector2Int.one)
                        {
                            if (MatchManager.gameState.MainBoard[posOffFront] == null &&
                                MatchManager.gameState.ActiveBoard[posOffFront])
                            {
                                list.Add(new NormalMove(pos, pos, posOffFront));
                            }
                        }
                    }

                    if (!VerifyBounds(rankOffBack)) continue;

                    var posOffBack = IndexOf(rankOffBack, fileOff);

                    if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                            new Vector2Int(rankOffBack, fileOff),
                            MatchManager.gameState.MainBoard) != -Vector2Int.one) continue;
                    
                    if (MatchManager.gameState.MainBoard[posOffBack] == null &&
                        MatchManager.gameState.ActiveBoard[posOffBack])
                    {
                        list.Add(new NormalMove(pos, pos, posOffBack));
                    }
                }
            }
        }

        private void Captures(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(pos);
            var push = color == Color.White ? -1 : 1;
            
            for (var i = file - attackRange + 1; i <= file + attackRange - 1; i++)
            {
                if (i == file || !VerifyBounds(i)) continue;
                var posTo = IndexOf(rank, i);

                var p = MatchManager.gameState.MainBoard[posTo];

                if (p == null || p.color == color) continue;
                
                 if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                         new Vector2Int(rank, i),
                         MatchManager.gameState.MainBoard) != -Vector2Int.one) continue;
                
                list.Add(new NormalCapture(pos, posTo));
            }

            for (var rankOff = push; Math.Abs(rankOff) <= attackRange; rankOff += push)
            {
                var rankAfter = rank + rankOff;
                if (!VerifyBounds(rankAfter)) break;
                
                for (var j = file - attackRange + Math.Abs(rankOff); j <= file + attackRange - Math.Abs(rankOff); j++)
                {
                    if (!VerifyBounds(j)) continue;
                    
                    var posTo = IndexOf(rankAfter, j);
                    var p = MatchManager.gameState.MainBoard[posTo];
                    
                    if (p == null || p.color == color) continue;
                    
                     if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                             new Vector2Int(rank, j),
                             MatchManager.gameState.MainBoard) != -Vector2Int.one) continue;
                
                    list.Add(new NormalCapture(pos, posTo));
                    
                }
            }
        }

        private void Skills(List<Action.Action> list)
        {
            if (SkillCooldown == 0)
            {
                list.Add(new ElectricEelActive(pos));
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