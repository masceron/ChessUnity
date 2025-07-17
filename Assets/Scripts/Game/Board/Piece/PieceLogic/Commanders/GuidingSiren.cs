using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Action.Skills;
using Game.Board.Effects.Buffs;
using Game.Board.Effects.Debuffs;
using Game.Board.General;
using Game.Board.Interaction;

namespace Game.Board.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GuidingSiren: PieceLogic
    {
        public sbyte SkillCooldown;

        public GuidingSiren(PieceConfig cfg) : base(cfg)
        {
            SkillCooldown = 0;
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SirenDebuffer(this)));
        }

        public override void PassTurn()
        {
            if (SkillCooldown > 0) SkillCooldown--;
        }
        
        private void MakeMove(List<Action.Action> list, int trank, int file, int curr, GameState gameState)
        {
            if (trank < 0 || trank >= gameState.MaxLength || file < 0 || file >= gameState.MaxLength) return;
            
            var tpos = trank * gameState.MaxLength + file;
            if (!gameState.ActiveBoard[tpos]) return;

            var p = gameState.MainBoard[tpos];
            if (p == null)
            {
                if (curr <= effectiveMoveRange)
                    list.Add(new NormalMove(pos, pos, tpos));
            }
            else if (p.color != color && curr <= attackRange)
            {
                list.Add(new NormalCapture(pos, pos, tpos));
            }
        }

        private void SirenActive(List<Action.Action> list, int trank, int file, GameState state)
        {
            var push = color == Color.White ? 1 : -1;
            for (var i = -6; i <= 6; i++)
            {
                var rankOff = trank + i;
                if (rankOff < 0 || rankOff >= state.MaxLength) continue;
                for (var j = -6; j <= 6; j++)
                {
                    var fileOff = file + j;
                    if (fileOff < 0 || fileOff >= state.MaxLength) continue;
                    
                    var tpos = rankOff * state.MaxLength + fileOff;
                    var pieceAt = state.MainBoard[tpos];
                    if (pieceAt == null || pieceAt.color == color) continue;
                    
                    var rankForce = rankOff + push;
                    while (Math.Abs(rankForce - rankOff) <= 3 &&
                           rankForce >= 0 && rankForce < state.MaxLength &&
                           state.MainBoard[rankForce * state.MaxLength + fileOff] == null &&
                           state.ActiveBoard[rankForce * state.MaxLength + fileOff])
                    {
                         rankForce += push;
                    }
                    rankForce -= push;
                    if (rankForce == rankOff) continue;
                    list.Add(new SirenActive(pos, tpos, rankForce * state.MaxLength + fileOff, InteractionManager.PieceManager));
                }
            }
        }

        protected override List<Action.Action> MoveToMake()
        {
            var gameState = InteractionManager.GameState;
            var trank = pos / gameState.MaxLength;
            var file = pos % gameState.MaxLength;
            
            var list = new List<Action.Action>();
            
            for (var i = 1; i <= Math.Max(attackRange, effectiveMoveRange); i++)
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