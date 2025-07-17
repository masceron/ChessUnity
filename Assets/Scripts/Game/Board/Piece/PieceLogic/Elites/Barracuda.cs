using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Effects.Buffs;
using Game.Board.General;
using Game.Board.Interaction;

namespace Game.Board.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Barracuda: PieceLogic
    {
        public Barracuda(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Ambush(-1, this)));
        }

        private void MakeMove(List<Action.Action> list, int tRank, int file, int distance)
        {
            if (tRank < 0 ||
                tRank >= MatchManager.MaxLength ||
                file < 0 ||
                file >= MatchManager.MaxLength || !MatchManager.GameState.ActiveBoard[tRank * MatchManager.MaxLength + file]) return;
            
            var rank = pos / InteractionManager.MaxLength;

            var tpos = tRank * MatchManager.MaxLength + file;
            var pieceOn = MatchManager.GameState.MainBoard[tpos];
            if (pieceOn == null)
            {
                if (distance == effectiveMoveRange)
                {
                    if (color == Color.White)
                    {
                        if (tRank >= rank) return;
                    }
                    else if (tRank <= rank) return;
                }
                
                if (distance <= effectiveMoveRange)
                {
                    list.Add(new NormalMove(pos, pos, tpos));
                }
            }
            else if (pieceOn.color != color)
            {
                if (distance == attackRange)
                {
                    if (color == Color.White)
                    {
                        if (tRank >= rank) return;
                    }
                    else if (tRank <= rank) return;
                }
                
                if (pieceOn.color != color)
                    list.Add(new NormalCapture(pos, pos, tpos));
            }
        }

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            var rank = pos / InteractionManager.MaxLength;
            var pieceFile = pos % InteractionManager.MaxLength;
            
            for (var i = 1; i <= Math.Max(effectiveMoveRange, attackRange); i++)
            {
                for (var file = pieceFile - i; file <= pieceFile + i; file += 1)
                {
                    MakeMove(list, rank - i, file, i);
                }
                
                for (var file = pieceFile - i; file <= pieceFile + i - 1; file += 1)
                {
                    MakeMove(list, rank + i, file, i);
                }
                
                for (var tRank = rank - i + 1; tRank <= rank + i; tRank++)
                {
                    MakeMove(list, tRank, pieceFile + i, i);
                }
                
                for (var tRank = rank - i + 1; tRank <= rank + i - 1; tRank++)
                {
                    MakeMove(list, tRank, pieceFile - i, i);
                }
            }
            
            return list;
        }
    }
}