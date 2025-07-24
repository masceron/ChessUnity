using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Effects.Buffs;
using Game.Board.Effects.Debuffs;
using Game.Board.Effects.Traits;
using Game.Board.General;
using Color = Game.Board.General.Color;
using static Game.Common.BoardUtils;

namespace Game.Board.Piece.PieceLogic.Commons
{
    public class SeaUrchin: PieceLogic
    {
        public SeaUrchin(PieceConfig cfg) : base(cfg)
        {
            SkillCooldown = -1;
            ActionManager.ExecuteImmediately(new ApplyEffect(new Carapace(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Blinded(-1, 50, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Demolisher(this)));
        }

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            var push = color == Color.White ? -MaxLength : MaxLength;
            
            for (var i = 1; i <= Math.Max(effectiveMoveRange, attackRange); i++)
            {
                var posTo = pos + push * i;

                if (!VerifyIndex(posTo) ||
                    !MatchManager.gameState.ActiveBoard[posTo]) continue;
                
                var pieceAt = MatchManager.gameState.MainBoard[posTo];
                
                if (pieceAt == null)
                {
                    if (effectiveMoveRange >= i)
                        list.Add(new NormalMove(pos, pos, posTo));
                }
                else if (pieceAt.pieceRank == PieceRank.Construct && attackRange >= i)
                {
                    list.Add(new DestroyConstruct(pos, (ushort)posTo));
                }
            }

            return list;
        }
    }
}