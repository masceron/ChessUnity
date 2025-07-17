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
            var maxLength = MatchManager.MaxLength;
            
            var rank = pos / maxLength;
            var file = pos % maxLength;
            
            for (var i = file - effectiveMoveRange + 1; i <= file + effectiveMoveRange - 1; i++)
            {
                if (i == file || i < 0 || i >= maxLength) continue;
                var posTo = rank * maxLength + i;
                
                if (MatchManager.GameState.MainBoard[posTo] != null ||
                    !MatchManager.GameState.ActiveBoard[posTo]) continue;

                if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                        new Vector2Int(rank, i),
                        MatchManager.GameState.MainBoard,
                        MatchManager.MaxLength) != -Vector2Int.one) continue;
                
                list.Add(new NormalMove(pos, pos, posTo));
            }
            
            for (var i = 1; i <= effectiveMoveRange; i++)
            {
                var rankOffFront = rank + i;
                var rankOffBack = rank - i;

                for (var j = -i; j <= i; j++)
                {
                    var fileOff = file + j;
                    if (fileOff < 0 || fileOff >= maxLength) continue;
                    if (rankOffFront >= 0 && rankOffFront < maxLength)
                    {
                        var posOffFront = rankOffFront * maxLength + fileOff;

                        if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                                new Vector2Int(rankOffFront, fileOff),
                                MatchManager.GameState.MainBoard,
                                MatchManager.MaxLength) == -Vector2Int.one)
                        {
                            if (MatchManager.GameState.MainBoard[posOffFront] == null &&
                                MatchManager.GameState.ActiveBoard[posOffFront])
                            {
                                list.Add(new NormalMove(pos, pos, posOffFront));
                            }
                        }
                    }

                    if (rankOffBack < 0 || rankOffBack >= maxLength) continue;

                    var posOffBack = rankOffBack * maxLength + fileOff;

                    if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                            new Vector2Int(rankOffBack, fileOff),
                            MatchManager.GameState.MainBoard,
                            MatchManager.MaxLength) != -Vector2Int.one) continue;
                    
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
            var maxLength = MatchManager.MaxLength;
            
            var rank = pos / maxLength;
            var file = pos % maxLength;
            var push = color == Color.White ? -1 : 1;
            
            for (var i = file - attackRange + 1; i <= file + attackRange - 1; i++)
            {
                if (i == file || i < 0 || i >= maxLength) continue;
                var posTo = rank * maxLength + i;

                var p = MatchManager.GameState.MainBoard[posTo];

                if (p == null || p.color == color) continue;
                
                 if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                         new Vector2Int(rank, i),
                         MatchManager.GameState.MainBoard,
                         MatchManager.MaxLength) != -Vector2Int.one) continue;
                
                list.Add(new NormalCapture(pos, pos, posTo));
            }

            for (var rankOff = push; Math.Abs(rankOff) <= attackRange; rankOff += push)
            {
                var rankAfter = rank + rankOff;
                if (rankAfter < 0 || rankAfter >= maxLength) break;
                
                for (var j = file - attackRange + Math.Abs(rankOff); j <= file + attackRange - Math.Abs(rankOff); j++)
                {
                    if (j < 0 || j >= maxLength) continue;
                    
                    var posTo = rankAfter * maxLength + j;
                    var p = MatchManager.GameState.MainBoard[posTo];
                    
                    if (p == null || p.color == color) continue;
                    
                     if (Pathfinder.LineBlocker(new Vector2Int(rank, file),
                             new Vector2Int(rank, j),
                             MatchManager.GameState.MainBoard,
                             MatchManager.MaxLength) != -Vector2Int.one) continue;
                
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