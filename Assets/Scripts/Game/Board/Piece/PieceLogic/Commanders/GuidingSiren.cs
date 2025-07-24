using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Action.Skills;
using Game.Board.Effects.Traits;
using Game.Board.General;
using static Game.Common.BoardUtils;

namespace Game.Board.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GuidingSiren: PieceLogic
    {
        public GuidingSiren(PieceConfig cfg) : base(cfg)
        {
            SkillCooldown = 0;
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SirenDebuffer(this)));
        }
        
        
        private void MakeMove(List<Action.Action> list, int trank, int file, int curr, GameState gameState)
        {
            if (!VerifyBounds(trank) || !VerifyBounds(file)) return;
            
            var tpos = IndexOf(trank, file);
            if (!gameState.ActiveBoard[tpos]) return;

            var p = gameState.MainBoard[tpos];
            if (p == null)
            {
                if (curr <= effectiveMoveRange)
                    list.Add(new NormalMove(pos, pos, tpos));
            }
            else if (p.color != color && curr <= attackRange)
            {
                list.Add(new NormalCapture(pos, tpos));
            }
        }

        private void SirenActive(List<Action.Action> list, int trank, int file, GameState state)
        {
            var push = color == Color.White ? 1 : -1;
            for (var i = -6; i <= 6; i++)
            {
                var rankOff = trank + i;
                if (!VerifyBounds(rankOff)) continue;
                for (var j = -6; j <= 6; j++)
                {
                    var fileOff = file + j;
                    if (!VerifyBounds(fileOff)) continue;
                    
                    var tpos = IndexOf(rankOff, fileOff);
                    var pieceAt = state.MainBoard[tpos];
                    if (pieceAt == null || pieceAt.color == color) continue;
                    
                    var rankForce = rankOff + push;
                    while (Math.Abs(rankForce - rankOff) <= 3 &&
                           VerifyBounds(rankForce) &&
                           state.MainBoard[IndexOf(rankForce, fileOff)] == null &&
                           state.ActiveBoard[IndexOf(rankForce, fileOff)])
                    {
                         rankForce += push;
                    }
                    rankForce -= push;
                    if (rankForce == rankOff) continue;
                    list.Add(new SirenActive(pos, tpos, IndexOf(rankForce, fileOff)));
                }
            }
        }

        protected override List<Action.Action> MoveToMake()
        {
            var gameState = MatchManager.gameState;
            var (trank, file) = RankFileOf(pos);
            
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