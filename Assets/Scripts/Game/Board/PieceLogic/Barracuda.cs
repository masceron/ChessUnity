using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects;
using Game.Board.General;
using Game.Board.Interaction;
using Game.Board.Piece;

namespace Game.Board.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Barracuda: PieceLogic
    {
        public Barracuda(PieceConfig cfg) : base(cfg)
        {
            ActionManager.Execute(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.Execute(new ApplyEffect(new Surpass(-1, this)));
            ActionManager.Execute(new ApplyEffect(new Ambush(-1, this)));
        }

        private void MakeMove(List<Action.Action> list, int tRank, int file, int distance)
        {
            if (tRank < 0 ||
                tRank >= InteractionManager.MaxRank ||
                file < 0 ||
                file >= InteractionManager.MaxFile) return;
            
            var rank = pos / InteractionManager.MaxFile;

            var tpos = tRank * InteractionManager.MaxFile + file;
            var pieceOn = InteractionManager.GameState.MainBoard[tpos];
            if (pieceOn == null)
            {
                if (distance == moveRange)
                {
                    if (color == Color.White)
                    {
                        if (tRank >= rank) return;
                    }
                    else if (tRank <= rank) return;
                }
                
                if (distance <= moveRange)
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
                    list.Add(new NormalCapture(pos, pos, (ushort)tpos));
            }
        }

        public override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            var rank = pos / InteractionManager.MaxFile;
            var pieceFile = pos % InteractionManager.MaxFile;
            
            for (var i = 1; i <= Math.Max(moveRange, attackRange); i++)
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