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
        public static bool selectPieceLock;
        public static int selectingPiece;
        public static int maxRank;
        public static int maxFile;
        public static int boardSize;
        public static PieceManager pieceManager;
        public static TileManager tileManager;
        public static GameState gameState;
        
        private static Action.Action _pendingMove = null;

        public static void Init(int r, int f, TileManager t, PieceManager p, GameState g)
        {
            selectingPiece = -1;
            maxRank = r;
            maxFile = f;
            tileManager = t;
            pieceManager = p;
            gameState = g;
            boardSize = r * f;
        }
        
        public static void Select(int rank, int file)
        {
            if (selectPieceLock) return;
            
            if (selectingPiece == -1)
            {
                var selected = rank * maxFile + file;
                var p = gameState.Position.main_board[selected];
                if (p == null || p.Color != gameState.OurSide) return;
                selectingPiece = selected;
                MarkPiece(selected);
                
            }
            else
            {
                var selected = rank * maxFile + file;
                if (selected == selectingPiece)
                {
                    UnmarkPiece(selected);
                    selectingPiece = -1;
                }
                else
                {
                    //Just a normal capture or quiet move
                    if (_pendingMove == null)
                    {
                        var action = ActionToTake.Find(x => x.To == selected);
                        if (action != null)
                        {
                            ActionManager.Execute(gameState, action);
                        }
                        UnmarkPiece(selectingPiece);
                        selectingPiece = -1;
                    }
                }
            }
        }

        public static List<Action.Action> ActionToTake = new();
        private static void MarkPiece(int pos)
        {
            tileManager.Select(pos);
            ActionToTake = pieceManager.GetPiece(pos).logic.MoveToMake(pos);

            foreach (var encoded in ActionToTake.Select(action => action.Move).Where(encoded => encoded.flag is MoveFlag.NormalMove or MoveFlag.NormalCapture))
            {
                tileManager.MarkAsMoveable(encoded.to);
            }
        }

        public static void UnmarkPiece(int pos)
        {
            if (pos == -1) return;
            tileManager.Unmark(pos);
            foreach (var encoded in ActionToTake.Select(action => action.Move).Where(encoded => encoded.flag is MoveFlag.NormalMove or MoveFlag.NormalCapture))
            {
                tileManager.Unmark(encoded.to);
            }
        }
    }
}