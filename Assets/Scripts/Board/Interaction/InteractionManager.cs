using Board.Action;
using Board.Piece;
using Core;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Board.Interaction
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class InteractionManager
    {
        private static bool _selectPieceLock;
        private static int _selectingPiece;
        private static int _maxRank;
        private static int _maxFile;
        private static PieceManager _pieceManager;
        private static GameState _gameState;

        public static void Init(int r, int f, PieceManager p, GameState g)
        {
            _selectingPiece = -1;
            _maxRank = r;
            _maxFile = f;
            _pieceManager = p;
            _gameState = g;
        }
        
        public static void Select(int rank, int file)
        {
            if (_selectPieceLock) return;
            
            
            if (_selectingPiece == -1)
            {
                var selected = rank * _maxFile + file;
                if (_pieceManager.GetPiece(selected) == null) return;
                    _selectingPiece = selected;
                _pieceManager.Select(_selectingPiece);
                
            }
            else
            {
                var selected = rank * _maxFile + file;
                if (selected == _selectingPiece)
                {
                    _pieceManager.Unselect(selected);
                    _selectingPiece = -1;
                }
                else
                {
                    var p = _pieceManager.GetPiece(selected);
                    if (p != null)
                    {
                        if (p.side == _gameState.OurSide)
                        {
                            _pieceManager.Unselect(_selectingPiece);
                            _selectingPiece = selected;
                            _pieceManager.Select(selected);
                        }
                    }
                    else
                    {
                        ActionManager.Execute(_gameState, new NormalMove(_selectingPiece, selected, _pieceManager, _maxFile));
                        _pieceManager.Unselect(_selectingPiece);
                        _selectingPiece = -1;
                    }
                }
            }
        }
    }
}