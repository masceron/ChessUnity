using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using Game.Board.Piece.PieceLogic.Commanders;
using Game.Board.Piece.PieceLogic.Commons;
using Game.Board.Piece.PieceLogic.Elites;
using Game.Board.Piece.PieceLogic.Summon;
using Game.Board.Piece.PieceLogic.Summoned;
using Game.Board.Piece.PieceLogic.Swarm;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Board.General
{
    public enum Color : byte
    {
        White,
        Black,
        None
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GameState
    {
        public readonly Color OurSide;
        public readonly PieceLogic[] PieceBoard;
        public readonly BitArray ActiveBoard;
        public readonly BitArray SquareColor;
        public Color SideToMove;
        public readonly ObservableCollection<PieceConfig> WhiteCaptured = new();
        public readonly ObservableCollection<PieceConfig> BlackCaptured = new();

        public GameState(int maxLength, Vector2Int startingSize, Color side, Color ourSide)
        {
            OurSide = ourSide;

            PieceBoard = new PieceLogic[maxLength * maxLength];
            ActiveBoard = new BitArray(maxLength * maxLength);
            SquareColor = new BitArray(maxLength * maxLength);
            
            SideToMove = side;

            for (var i = 0; i < SquareColor.Count; i++)
            {
                SquareColor[i] = (RankOf(i) + FileOf(i)) % 2 != 0;
            }

            var rankStart = (maxLength - startingSize.x) / 2;
            var fileStart = (maxLength - startingSize.y) / 2;

            for (var offRank = 0; offRank < startingSize.x; offRank++)
            {
                var rank = rankStart + offRank;
                for (var offFile = 0; offFile < startingSize.y; offFile++)
                {
                    var file = fileStart + offFile;
                    ActiveBoard[IndexOf(rank, file)] = true;
                }
            }
        }

        public void SpawnPiece(PieceConfig piece)
        {
            PieceLogic p = piece.Type switch
            {
                PieceType.Velkaris => new Velkaris(piece),
                PieceType.GuidingSiren => new GuidingSiren(piece),
                PieceType.Barracuda => new Barracuda(piece),
                PieceType.SeaUrchin => new SeaUrchin(piece),
                PieceType.ElectricEel => new ElectricEel(piece),
                PieceType.FlyingFish => new FlyingFish(piece),
                PieceType.Chrysos => new Chrysos(piece),
                PieceType.Anomalocaris => new Anomalocaris(piece),
                PieceType.Archelon => new Archelon(piece),
                PieceType.Thalassos => new Thalassos(piece),
                _ => null
            };

            PieceBoard[piece.Index] = p;
        }

        public void EffectCountdown()
        {
            foreach (var piece in PieceBoard)
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
            var pieceAffected = PieceBoard[pos];
            PieceBoard[pos] = null;
            
            pieceAffected.Effects.ForEach(EventObserver.RemoveObserver);
            
            (pieceAffected.Color == Color.White ? WhiteCaptured : BlackCaptured).Add(new PieceConfig(pieceAffected.Type, pieceAffected.Color, pieceAffected.Pos));
        }

        public void Move(ushort f, ushort t)
        {
            PieceBoard[t] = PieceBoard[f];
            PieceBoard[t].Pos = t;
            PieceBoard[f] = null;
        }
    }
}
