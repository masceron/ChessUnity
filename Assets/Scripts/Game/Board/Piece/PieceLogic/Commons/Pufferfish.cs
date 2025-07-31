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
using static Game.Board.General.MatchManager;

namespace Game.Board.Piece.PieceLogic.Commons
{
    public class Pufferfish: PieceLogic
    {
        public Pufferfish(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Dominator(this)));
        }

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            var range = Math.Max(AttackRange, EffectiveMoveRange);
            var push = Color == Color.White ? -1 : 1;

            for (var rank = RankOf(Pos) - (range - 1) * push; rank != RankOf(Pos) + (range + 1) * push; rank += push)
            {
                if (!VerifyBounds(rank)) continue;
                for (var file = FileOf(Pos) - 1; file <= FileOf(Pos) + 1; file++)
                {
                    if (!VerifyBounds(file)) continue;
                    var idx = IndexOf(rank, file);
                    if (!gameState.ActiveBoard[idx]) continue;
                    
                    var piece = gameState.PieceBoard[idx];
                    
                    if (piece == null)
                    {
                        list.Add(new NormalMove(Pos, idx));
                    }
                    else if (piece.Color != Color && piece.PieceRank < PieceRank)
                    {
                        list.Add(new NormalCapture(Pos, idx));
                    }
                }
            }

            list.Add(new PufferfishExplode(Pos));

            return list;
        }
    }
}