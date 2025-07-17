using System.Collections;
using System.Linq;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Piece;
using Game.Board.PieceLogic.Commanders;
using Game.Board.PieceLogic.Commons;
using Game.Board.PieceLogic.Elites;
using Game.Board.PieceLogic.Swarm;

namespace Game.Board.General
{
    public enum Color : byte
    {
        White,
        Black
    }
    

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GameState
    {
        public Color OurSide;
        public readonly int MaxLength;

        public readonly PieceLogic.PieceLogic[] MainBoard;
        public readonly BitArray ActiveBoard;
        public Color SideToMove;

        public GameState(int maxLength, byte[] ac, Color side, Color ourSide)
        {
            OurSide = ourSide;
            MaxLength = maxLength;

            MainBoard = new PieceLogic.PieceLogic[maxLength * maxLength];
            ActiveBoard = new BitArray(maxLength * maxLength);
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
                PieceType.ElectricEel => new ElectricEel(piece),
                PieceType.FlyingFish => new FlyingFish(piece),
                _ => null
            };

            MainBoard[piece.Index] = p;
        }

        public void EffectCountdown()
        {
            foreach (var piece in MainBoard)
            {
                if (piece == null) continue;
                
                piece.PassTurn();

                foreach (var effect in piece.Effects.Where(effect => effect.Duration >= 0))
                {
                    effect.Duration -= 1;

                    if (effect.Duration == 0)
                    {
                        ActionManager.EnqueueAction(new RemoveEffect(effect));
                    }
                }
            }
        }

        public void Destroy(int pos)
        {
            var pieceAffected = MainBoard[pos];
            MainBoard[pos] = null;
            
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
