using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Effects.Buffs;
using Game.Board.Effects.Debuffs;
using Game.Board.Effects.Others;
using Game.Board.General;
using Game.Board.Piece;
using Color = Game.Board.General.Color;

namespace Game.Board.PieceLogic.Commons
{
    public class SeaUrchin: PieceLogic
    {
        public SeaUrchin(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Carapace(-1, 25, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Blinded(-1, 50, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Demolisher(this)));
        }

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            var push = color == Color.White ? -MatchManager.MaxLength : MatchManager.MaxLength;
            
            for (var i = 1; i <= Math.Max(moveRange, attackRange); i++)
            {
                var posTo = pos + push * i;

                if (posTo < 0 || pos >= MatchManager.MaxLength * MatchManager.MaxLength ||
                    !MatchManager.GameState.ActiveBoard[posTo]) continue;
                
                var pieceAt = MatchManager.GameState.MainBoard[posTo];
                
                if (pieceAt == null)
                {
                    if (moveRange >= i)
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