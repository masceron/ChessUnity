using System;
using System.Collections.Generic;
using Board.Action;
using Board.Interaction;
using Core.General;
using Action = Board.Action.Action;

namespace Core.Piece
{
    public class GuidingSiren: PieceLogic
    {
        public sbyte SkillCooldown;

        public GuidingSiren(PieceType type, Color color, ushort pos, List<Effect> effects) : base(type, color, pos, effects, 5)
        {
            SkillCooldown = 0;
            Rank = PieceRank.Commander;
        }

        public override void PassTurn()
        {
            if (SkillCooldown > 0) SkillCooldown--;
        }
        
        private void MakeMove(List<Action> list, int rank, int file, int curr, int maxRange, GameState gameState)
        {
            if (rank < 0 || rank >= gameState.MaxRank || file < 0 || file >= gameState.MaxFile) return;
            
            var pos = rank * gameState.MaxFile + file;
            if (!gameState.ActiveBoard[pos]) return;

            var p = gameState.MainBoard[pos];
            if (p == null)
            {
                if (curr <= maxRange)
                    list.Add(new NormalMove(Pos, Pos, pos, InteractionManager.PieceManager));
            }
            else if (p.Color != Color)
            {
                list.Add(new NormalCapture(Pos, Pos, (ushort)pos));
            }
        }

        private void SirenActive(List<Action> list, int rank, int file, GameState state)
        {
            var push = Color == Color.White ? 1 : -1;
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
                    if (pieceAt == null || pieceAt.Color == Color) continue;
                    
                    var rankForce = rankOff + push;
                    while (Math.Abs(rankForce - rankOff) <= 3 &&
                           rankForce >= 0 && rankForce < state.MaxRank &&
                           state.MainBoard[rankForce * state.MaxFile + fileOff] == null &&
                           state.ActiveBoard[rankForce * state.MaxFile + fileOff])
                    {
                         rankForce += push;
                    }
                    rankForce -= push;
                    if (rankForce == rankOff) continue;
                    list.Add(new SirenActive(Pos, pos, rankForce * state.MaxFile + fileOff, InteractionManager.PieceManager));
                }
            }
        }

        public override List<Action> MoveToMake()
        {
            var gameState = InteractionManager.GameState;
            var effectiveRange = Math.Max(0, Range - (Effects.Contains(GameState.Slow) ? 1 : 0));
            var rank = Pos / gameState.MaxFile;
            var file = Pos % gameState.MaxFile;
            
            var list = new List<Action>();
            
            for (var i = 1; i <= Range; i++)
            {
                MakeMove(list, rank + i, file, i, effectiveRange, gameState);
                MakeMove(list, rank - i, file, i, effectiveRange, gameState);
                MakeMove(list, rank, file + i, i, effectiveRange, gameState);
                MakeMove(list, rank, file - i, i, effectiveRange, gameState);
                MakeMove(list, rank + i, file + i, i, effectiveRange, gameState);
                MakeMove(list, rank - i, file + i, i, effectiveRange, gameState);
                MakeMove(list, rank + i, file - i, i, effectiveRange, gameState);
                MakeMove(list, rank - i, file - i, i, effectiveRange, gameState);
            }

            if (SkillCooldown == 0)
            {
                SirenActive(list, rank, file, gameState);
            }

            return list;
        }
    }
}