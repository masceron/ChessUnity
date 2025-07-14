using System.Collections;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Piece;
using Game.Board.PieceLogic;
using Game.Board.PieceLogic.Commanders;
using Game.Board.PieceLogic.Commons;
using Game.Board.PieceLogic.Elites;
using Game.Board.Triggers;

namespace Game.Board.General
{
    public enum PieceType : sbyte
    {
        Velkaris,
        GuidingSiren,
        Barracuda,
        SeaUrchin
    }

    public enum PieceRank : byte
    {
        Commander,
        Construct,
        Champion,
        Elite,
        Common,
        Swarm,
        Summoned
    }

    public enum Color : byte
    {
        White,
        Black
    }
    

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GameState
    {
        public Color OurSide;
        public readonly int MaxRank;
        public readonly int MaxFile;

        public readonly PieceLogic.PieceLogic[] MainBoard;
        public readonly BitArray ActiveBoard;
        public Color SideToMove;

        public GameState(int maxRank, int maxFile, byte[] ac, Color side, Color ourSide)
        {
            OurSide = ourSide;
            MaxFile = maxFile;
            MaxRank = maxRank;

            MainBoard = new PieceLogic.PieceLogic[maxRank * maxFile];
            ActiveBoard = new BitArray(maxRank * maxFile);
            SideToMove = side;

            for (var i = 0; i < ac.Length; i++)
            {
                if (ac[i] == 0) ActiveBoard[i] = false;
                else ActiveBoard[i] = true;
            }
        }

        public void SpawnPiece(PieceConfig piece)
        {
            PieceLogic.PieceLogic p = piece.Type switch
            {
                PieceType.Velkaris => new Velkaris(piece),
                PieceType.GuidingSiren => new GuidingSiren(piece),
                PieceType.Barracuda => new Barracuda(piece),
                PieceType.SeaUrchin => new SeaUrchin(piece),
                _ => null
            };

            MainBoard[piece.Index] = p;
        }

        private readonly List<Trigger> triggersToRemove = new();

        public void QueueTriggerDeleter(Trigger t)
        {
            triggersToRemove.Add(t);
        }
        
        public void EndTurn()
        {
            foreach (var piece in MainBoard)
            {
                if (piece == null) continue;
                
                piece.PassTurn();

                for (var i = 0; i < piece.Effects.Count; i++)
                {
                    var eff = piece.Effects[i];
                    if (eff.Duration < 0) continue;
                    eff.Duration -= 1;
                    piece.Effects[i] = eff;

                    if (eff.Duration == 0)
                    {
                        ActionManager.TakeAction(new RemoveEffect(piece.Effects[i]));
                    }
                }

                piece.Effects.RemoveAll(e => e.Duration == 0);
            }

            foreach (var trigger in triggersToRemove)
            {
                EventObserver.RemoveObserver(trigger);
            }
            
            triggersToRemove.Clear();
            
        }

        public void Destroy(int pos)
        {
            var pieceAffected = MainBoard[pos];
            
            pieceAffected.Effects.ForEach(EventObserver.RemoveObserver);
        }

        public void Move(ushort f, ushort t)
        {
            MainBoard[t] = MainBoard[f];
            MainBoard[t].pos = t;
            MainBoard[f] = null;
        }
    }
}
