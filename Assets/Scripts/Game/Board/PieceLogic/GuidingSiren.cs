using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects;
using Game.Board.General;
using Game.Board.Interaction;
using Game.Board.Piece;
using Game.Board.Triggers;

namespace Game.Board.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GuidingSiren: PieceLogic
    {
        public sbyte SkillCooldown;

        public GuidingSiren(PieceConfig cfg) : base(cfg)
        {
            SkillCooldown = 0;
            ActionManager.Execute(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.Execute(new ApplyEffect(new Surpass(-1, this)));
            new SirenDebuffer(this);
        }

        public override void PassTurn()
        {
            if (SkillCooldown > 0) SkillCooldown--;
        }
        
        private void MakeMove(List<Action.Action> list, int trank, int file, int curr, GameState gameState)
        {
            if (trank < 0 || trank >= gameState.MaxRank || file < 0 || file >= gameState.MaxFile) return;
            
            var tpos = trank * gameState.MaxFile + file;
            if (!gameState.ActiveBoard[tpos]) return;

            var p = gameState.MainBoard[tpos];
            if (p == null)
            {
                if (curr <= moveRange)
                    list.Add(new NormalMove(this.pos, this.pos, tpos));
            }
            else if (p.color != color && curr <= attackRange)
            {
                list.Add(new NormalCapture(this.pos, this.pos, (ushort)tpos));
            }
        }

        private void SirenActive(List<Action.Action> list, int trank, int file, GameState state)
        {
            var push = color == Color.White ? 1 : -1;
            for (var i = -6; i <= 6; i++)
            {
                var rankOff = trank + i;
                if (rankOff < 0 || rankOff >= state.MaxRank) continue;
                for (var j = -6; j <= 6; j++)
                {
                    var fileOff = file + j;
                    if (fileOff < 0 || fileOff >= state.MaxFile) continue;
                    
                    var tpos = rankOff * state.MaxFile + fileOff;
                    var pieceAt = state.MainBoard[tpos];
                    if (pieceAt == null || pieceAt.color == color) continue;
                    
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
                    list.Add(new SirenActive(pos, tpos, rankForce * state.MaxFile + fileOff, InteractionManager.PieceManager));
                }
            }
        }

        public override List<Action.Action> MoveToMake()
        {
            var gameState = InteractionManager.GameState;
            var trank = pos / gameState.MaxFile;
            var file = pos % gameState.MaxFile;
            
            var list = new List<Action.Action>();
            
            for (var i = 1; i <= Math.Max(attackRange, moveRange); i++)
            {
                MakeMove(list, trank + i, file, i, gameState);
                MakeMove(list, trank - i, file, i, gameState);
                MakeMove(list, trank, file + i, i, gameState);
                MakeMove(list, trank, file - i, i, gameState);
                MakeMove(list, trank + i, file + i, i, gameState);
                MakeMove(list, trank - i, file + i, i, gameState);
                MakeMove(list, trank + i, file - i, i, gameState);
                MakeMove(list, trank - i, file - i, i, gameState);
            }

            if (SkillCooldown == 0)
            {
                SirenActive(list, trank, file, gameState);
            }

            return list;
        }
    }
}