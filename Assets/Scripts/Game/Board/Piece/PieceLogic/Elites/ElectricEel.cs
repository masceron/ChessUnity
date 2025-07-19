using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Action.Skills;
using Game.Board.Effects.Others;
using Game.Board.General;
using Game.Common;
using UnityEngine;
using Color = Game.Board.General.Color;
using static Game.Common.BoardUtils;

namespace Game.Board.Piece.PieceLogic.Elites
{
    public class ElectricEel: PieceLogic
    {
        public sbyte SkillCooldown;
        
        public ElectricEel(PieceConfig cfg) : base(cfg)
        {
            SkillCooldown = 0;
            ActionManager.ExecuteImmediately(new ApplyEffect(new Vengeful(this)));
        }

        public override void PassTurn()
        {
            if (SkillCooldown > 0) SkillCooldown--;
        }

        private void Quiets(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(pos);
            
            for (var i = file - effectiveMoveRange + 1; i <= file + effectiveMoveRange - 1; i++)
            {
                if (i == file || !VerifyBounds(i)) continue;
                var posTo = IndexOf(rank, i);
                
                if (MatchManager.GameState.MainBoard[posTo] != null ||
                    !MatchManager.GameState.ActiveBoard[posTo]) continue;

                if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                        new Vector2Int(rank, i),
                        MatchManager.GameState.MainBoard) != -Vector2Int.one) continue;
                
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
                                MatchManager.GameState.MainBoard) == -Vector2Int.one)
                        {
                            if (MatchManager.GameState.MainBoard[posOffFront] == null &&
                                MatchManager.GameState.ActiveBoard[posOffFront])
                            {
                                list.Add(new NormalMove(pos, pos, posOffFront));
                            }
                        }
                    }

                    if (!VerifyBounds(rankOffBack)) continue;

                    var posOffBack = IndexOf(rankOffBack, fileOff);

                    if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                            new Vector2Int(rankOffBack, fileOff),
                            MatchManager.GameState.MainBoard) != -Vector2Int.one) continue;
                    
                    if (MatchManager.GameState.MainBoard[posOffBack] == null &&
                        MatchManager.GameState.ActiveBoard[posOffBack])
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

                var p = MatchManager.GameState.MainBoard[posTo];

                if (p == null || p.color == color) continue;
                
                 if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                         new Vector2Int(rank, i),
                         MatchManager.GameState.MainBoard) != -Vector2Int.one) continue;
                
                list.Add(new NormalCapture(pos, pos, posTo));
            }

            for (var rankOff = push; Math.Abs(rankOff) <= attackRange; rankOff += push)
            {
                var rankAfter = rank + rankOff;
                if (!VerifyBounds(rankAfter)) break;
                
                for (var j = file - attackRange + Math.Abs(rankOff); j <= file + attackRange - Math.Abs(rankOff); j++)
                {
                    if (!VerifyBounds(j)) continue;
                    
                    var posTo = IndexOf(rankAfter, j);
                    var p = MatchManager.GameState.MainBoard[posTo];
                    
                    if (p == null || p.color == color) continue;
                    
                     if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                             new Vector2Int(rank, j),
                             MatchManager.GameState.MainBoard) != -Vector2Int.one) continue;
                
                    list.Add(new NormalCapture(pos, pos, posTo));
                    
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