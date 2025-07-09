using System;
using System.Collections.Generic;
using Board.Action;
using Board.Interaction;
using UnityEngine;
using Action = Board.Action.Action;

namespace Core.PieceLogic
{
    public class GuidingSiren: PieceLogic
    {
        private const int Range = 5;
        
        public GuidingSiren(PieceData p) : base(p) {}

        private void MakeMove(List<Action> list, int rank, int file, int curr, int maxRange, GameState gameState)
        {
            if (rank < 0 || rank >= gameState.MaxRank || file < 0 || file >= gameState.MaxFile) return;
            
            var pos = rank * gameState.MaxFile + file;
            if (!gameState.ActiveBoard[pos]) return;

            var p = gameState.MainBoard[pos];
            if (p == null)
            {
                if (curr <= maxRange)
                    list.Add(new NormalMove(Piece.Pos, Piece.Pos, pos, InteractionManager.PieceManager));
            }
            else if (p.Color != Piece.Color)
            {
                list.Add(new NormalCapture(Piece.Pos, Piece.Pos, (ushort)pos));
            }
        }

        private void SirenActive(List<Action> list, int rank, int file, GameState state)
        {
            var push = Piece.Color == Color.White ? 1 : -1;
            for (var i = -6; i <= 6; i++)
            {
                var rankOff = rank + i;
                if (rankOff < 0 || rankOff >= state.MaxRank) continue;
                for (var j = -6; j <= 6; j++)
                {
                    var fileOff = file + j;
                    if (fileOff < 0 || fileOff >= state.MaxFile) continue;
                    
                    var pos = rankOff * state.MaxFile + fileOff;
                    var pieceAt = state.MainBoard[pos];
                    if (pieceAt == null || pieceAt.Color == Piece.Color) continue;
                    
                    var rankForce = rankOff + push;
                    while (Math.Abs(rankForce - rankOff) <= 3 &&
                           rankForce >= 0 && rankForce < state.MaxRank &&
                           state.MainBoard[rankForce * state.MaxFile + fileOff] == null &&
                           state.ActiveBoard[rankForce * state.MaxFile + fileOff])
                    {
                         rankForce += push;
                    }
                    Debug.Log(rankOff + "->" + rankForce);

                    rankForce -= push;
                    if (rankForce == rankOff) continue;
                    list.Add(new SirenActive(Piece.Pos, pos, rankForce * state.MaxFile + fileOff, InteractionManager.PieceManager));
                }
            }
        }

        public override List<Action> MoveToMake(int from)
        {
            var gameState = InteractionManager.GameState;
            var range = Math.Max(0, Range - (Piece.Effects.Contains(GameState.Slow) ? 1 : 0));
            var rank = Piece.Pos / gameState.MaxFile;
            var file = Piece.Pos % gameState.MaxFile;
            
            var list = new List<Action>();
            
            for (var i = 1; i <= Range; i++)
            {
                MakeMove(list, rank + i, file, i, range, gameState);
                MakeMove(list, rank - i, file, i, range, gameState);
                MakeMove(list, rank, file + i, i, range, gameState);
                MakeMove(list, rank, file - i, i, range, gameState);
                MakeMove(list, rank + i, file + i, i, range, gameState);
                MakeMove(list, rank - i, file + i, i, range, gameState);
                MakeMove(list, rank + i, file - i, i, range, gameState);
                MakeMove(list, rank - i, file - i, i, range, gameState);
            }

            if (Piece.SkillCooldown == 0)
            {
                SirenActive(list, rank, file, gameState);
            }

            return list;
        }
    }
}