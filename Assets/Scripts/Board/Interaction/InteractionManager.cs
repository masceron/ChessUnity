using System.Collections.Generic;
using System.Linq;
using Board.Action;
using Board.Piece;
using Board.Tile;
using Core;
using Unity.IL2CPP.CompilerServices;

namespace Board.Interaction
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class InteractionManager
    {
        private static bool _selectPieceLock;
        private static int _selectingPiece;
        public static int maxRank;
        public static int maxFile;
        public static PieceManager pieceManager;
        public static TileManager tileManager;
        public static GameState gameState;

        public static void Init(int r, int f, TileManager t, PieceManager p, GameState g)
        {
            _selectingPiece = -1;
            maxRank = r;
            maxFile = f;
            tileManager = t;
            pieceManager = p;
            gameState = g;
        }
        
        public static void Select(int rank, int file)
        {
            if (_selectPieceLock) return;
            
            
            if (_selectingPiece == -1)
            {
                var selected = rank * maxFile + file;
                var p = pieceManager.GetPiece(selected);
                if (p == null || p.side != gameState.OurSide) return;
                    _selectingPiece = selected;
                    MarkPiece(selected);
                
            }
            else
            {
                var selected = rank * maxFile + file;
                if (selected == _selectingPiece)
                {
                    UnmarkPiece(selected);
                    _selectingPiece = -1;
                }
                else
                {
                    var p = pieceManager.GetPiece(selected);
                    if (p != null)
                    {
                        if (p.side == gameState.OurSide)
                        {
                            UnmarkPiece(_selectingPiece);
                            _selectingPiece = selected;
                            MarkPiece(selected);
                        }
                    }
                    else
                    {
                        ActionManager.Execute(gameState, new NormalMove(_selectingPiece, selected, pieceManager, maxFile));
                        UnmarkPiece(_selectingPiece);
                        _selectingPiece = -1;
                    }
                }
            }
        }

        private static List<IAction> _actionToTake = new();
        private static void MarkPiece(int pos)
        {
            tileManager.Select(pos);
            _actionToTake = pieceManager.GetPiece(pos).logic.MoveToMake(pos);

            foreach (var encoded in _actionToTake.Select(action => action.MakeEncodedMove()).Where(encoded => encoded.flag == MoveFlag.NormalMove))
            {
                tileManager.MarkAsMoveable(encoded.to);
            }
        }

        private static void UnmarkPiece(int pos)
        {
            tileManager.Unmark(pos);
            foreach (var encoded in _actionToTake.Select(action => action.MakeEncodedMove()).Where(encoded => encoded.flag == MoveFlag.NormalMove))
            {
                tileManager.Unmark(encoded.to);
            }
        }
    }
}