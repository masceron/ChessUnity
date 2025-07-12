using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects;
using Game.Board.General;
using Game.Board.Interaction;

namespace Game.Board.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GuidingSiren: PieceLogic
    {
        public sbyte SkillCooldown;

        public GuidingSiren(Color color, ushort pos) : base(color, pos, 5, 5, PieceRank.Commander)
        {
            SkillCooldown = 0;
            Rank = PieceRank.Commander;
            ActionManager.Execute(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.Execute(new ApplyEffect(new Surpass(-1, this)));
        }

        public override void PassTurn()
        {
            if (SkillCooldown > 0) SkillCooldown--;
        }
        
        private void MakeMove(List<Action.Action> list, int rank, int file, int curr, GameState gameState)
        {
            if (rank < 0 || rank >= gameState.MaxRank || file < 0 || file >= gameState.MaxFile) return;
            
            var pos = rank * gameState.MaxFile + file;
            if (!gameState.ActiveBoard[pos]) return;

            var p = gameState.MainBoard[pos];
            if (p == null)
            {
                if (curr <= MoveRange)
                    list.Add(new NormalMove(Pos, Pos, pos));
            }
            else if (p.Color != Color && curr <= AttackRange)
            {
                list.Add(new NormalCapture(Pos, Pos, (ushort)pos));
            }
        }

        private void SirenActive(List<Action.Action> list, int rank, int file, GameState state)
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

        public override List<Action.Action> MoveToMake()
        {
            var gameState = InteractionManager.GameState;
            var rank = Pos / gameState.MaxFile;
            var file = Pos % gameState.MaxFile;
            
            var list = new List<Action.Action>();
            
            for (var i = 1; i <= Math.Max(AttackRange, MoveRange); i++)
            {
                MakeMove(list, rank + i, file, i, gameState);
                MakeMove(list, rank - i, file, i, gameState);
                MakeMove(list, rank, file + i, i, gameState);
                MakeMove(list, rank, file - i, i, gameState);
                MakeMove(list, rank + i, file + i, i, gameState);
                MakeMove(list, rank - i, file + i, i, gameState);
                MakeMove(list, rank + i, file - i, i, gameState);
                MakeMove(list, rank - i, file - i, i, gameState);
            }

            if (SkillCooldown == 0)
            {
                SirenActive(list, rank, file, gameState);
            }

            return list;
        }
    }
}