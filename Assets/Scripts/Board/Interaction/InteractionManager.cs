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
        public static bool SelectPieceLock;
        public static int SelectingPiece;
        public static int MaxRank;
        public static int MaxFile;
        public static int BoardSize;
        public static PieceManager PieceManager;
        public static TileManager TileManager;
        public static GameState GameState;
        
        private static Action.Action _pendingMove = null;

        public static void Init(int r, int f, TileManager t, PieceManager p, GameState g)
        {
            SelectingPiece = -1;
            MaxRank = r;
            MaxFile = f;
            TileManager = t;
            PieceManager = p;
            GameState = g;
            BoardSize = r * f;
        }
        
        public static void Select(int rank, int file)
        {
            if (SelectPieceLock) return;
            
            if (SelectingPiece == -1)
            {
                var selected = rank * MaxFile + file;
                var p = GameState.MainBoard[selected];
                if (p == null || p.Color != GameState.OurSide) return;
                SelectingPiece = selected;
                MarkPiece(selected);
                
            }
            else
            {
                var selected = rank * MaxFile + file;
                if (selected == SelectingPiece)
                {
                    UnmarkPiece(selected);
                    SelectingPiece = -1;
                }
                else
                {
                    //Just a normal capture or quiet move
                    if (_pendingMove == null)
                    {
                        var action = ActionToTake.Find(x => x.To == selected);
                        if (action != null)
                        {
                            ActionManager.Execute(GameState, action);
                        }
                        UnmarkPiece(SelectingPiece);
                        SelectingPiece = -1;
                    }
                }
            }
        }

        public static List<Action.Action> ActionToTake = new();
        private static void MarkPiece(int pos)
        {
            TileManager.Select(pos);
            ActionToTake = PieceManager.GetPiece(pos).logic.MoveToMake(pos);

            foreach (var action in ActionToTake.Select(action => action).Where(ac => ac.From != ac.To))
            {
                TileManager.MarkAsMoveable(action.To);
            }
        }

        public static void UnmarkPiece(int pos)
        {
            if (pos == -1) return;
            TileManager.Unmark(pos);
            foreach (var encoded in ActionToTake.Select(action => action).Where(ac => ac.To != ac.From))
            {
                TileManager.Unmark(encoded.To);
            }
        }
    }
}