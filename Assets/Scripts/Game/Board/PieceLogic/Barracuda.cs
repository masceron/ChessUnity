using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects;
using Game.Board.General;
using Game.Board.Interaction;

namespace Game.Board.PieceLogic
{
    public class Barracuda: PieceLogic
    {
        public Barracuda(Color color, ushort pos) : base(color, pos, 2, 2, PieceRank.Elite)
        {
            ActionManager.Execute(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.Execute(new ApplyEffect(new Surpass(-1, this)));
            ActionManager.Execute(new ApplyEffect(new Ambush(-1, this)));
        }

        private void MakeMove(List<Action.Action> list, int rank, int file, int distance)
        {
            if (rank < 0 ||
                rank >= InteractionManager.MaxRank ||
                file < 0 ||
                file >= InteractionManager.MaxFile) return;
            
            var pieceRank = Pos / InteractionManager.MaxFile;

            var pos = rank * InteractionManager.MaxFile + file;
            var pieceOn = InteractionManager.GameState.MainBoard[pos];
            if (pieceOn == null)
            {
                if (distance == MoveRange)
                {
                    if (Color == Color.White)
                    {
                        if (rank >= pieceRank) return;
                    }
                    else if (rank <= pieceRank) return;
                }
                
                if (distance <= MoveRange)
                {
                    list.Add(new NormalMove(Pos, Pos, pos));
                }
            }
            else if (pieceOn.Color != Color)
            {
                if (distance == AttackRange)
                {
                    if (Color == Color.White)
                    {
                        if (rank >= pieceRank) return;
                    }
                    else if (rank <= pieceRank) return;
                }
                
                if (pieceOn.Color != Color)
                    list.Add(new NormalCapture(Pos, Pos, (ushort)pos));
            }
        }

        public override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            var pieceRank = Pos / InteractionManager.MaxFile;
            var pieceFile = Pos % InteractionManager.MaxFile;
            
            for (var i = 1; i <= Math.Max(MoveRange, AttackRange); i++)
            {
                for (var file = pieceFile - i; file <= pieceFile + i; file += 1)
                {
                    MakeMove(list, pieceRank - i, file, i);
                }
                
                for (var file = pieceFile - i; file <= pieceFile + i - 1; file += 1)
                {
                    MakeMove(list, pieceRank + i, file, i);
                }
                
                for (var rank = pieceRank - i + 1; rank <= pieceRank + i; rank++)
                {
                    MakeMove(list, rank, pieceFile + i, i);
                }
                
                for (var rank = pieceRank - i + 1; rank <= pieceRank + i - 1; rank++)
                {
                    MakeMove(list, rank, pieceFile - i, i);
                }
            }
            
            return list;
        }
    }
}