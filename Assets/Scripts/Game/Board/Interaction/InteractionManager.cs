using System;
using System.Collections.Generic;
using System.Linq;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Quiets;
using Game.Board.Action.Skills;
using Game.Board.General;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using Game.Board.Piece.PieceLogic.Commanders;
using Game.Board.Tile;

using static Game.Common.BoardUtils;

namespace Game.Board.Interaction
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class InteractionManager
    {
        public static PieceLogic SelectPieceLock;
        public static int SelectingPiece;
        public static PieceManager PieceManager;
        private static TileManager _tileManager;
        public static GameState GameState;

        public static void Init()
        {
            SelectingPiece = -1;
            _tileManager = MatchManager.TileManager;
            PieceManager = MatchManager.PieceManager;
            GameState = MatchManager.GameState;
        }
        
        public static void Select(int rank, int file)
        {
            if (SelectingPiece == -1)
            {
                var selected = IndexOf(rank, file);
                var p = GameState.MainBoard[selected];
                if (p == null || p.color != GameState.OurSide || p.color != GameState.SideToMove) return;
                SelectingPiece = selected;
                MarkPiece(selected, null);
                
            }
            else
            {
                var selected = IndexOf(rank, file);
                if (selected == SelectingPiece)
                {
                    UnmarkPiece(selected);
                    SelectingPiece = -1;
                }
                else
                {
                    //Just a normal capture or quiet move
                    if (SelectPieceLock == null)
                    {
                        var action = ActionToTake.Find(x => x.To == selected);
                        if (action != null)
                        {
                            ActionManager.EnqueueAction(action);
                        }
                        UnmarkPiece(SelectingPiece);
                        SelectingPiece = -1;
                    }
                    else if (SelectPieceLock.GetType() == typeof(GuidingSiren))
                    {
                        var action = ActionToTake.Find(x => x.From == selected && x.GetType() == typeof(SirenActive));
                        if (action != null)
                        {
                            ActionManager.EnqueueAction(action);
                        }
                        UnmarkPiece(SelectingPiece);
                        SelectingPiece = -1;
                    }
                }
            }
        }

        public static List<Action.Action> ActionToTake = new();
        private static readonly List<Action.Action> ActionMarked = new();
        public static void MarkPiece(int pos, Type type)
        {
            _tileManager.Select(pos);
            ActionToTake = GameState.MainBoard[pos].MoveList();

            if (type == null)
            {
                foreach (var action in ActionToTake.Select(action => action).Where(ac => ac is IQuiets or ICaptures))
                {
                    _tileManager.MarkAsMoveable(action.To);
                    ActionMarked.Add(action);
                }
            }
            else if (type == typeof(SirenActive))
            {
                foreach (var action in ActionToTake.Select(action => action).Where(ac => ac.GetType() == typeof(SirenActive)))
                {
                    _tileManager.MarkAsMoveable(action.From);
                    ActionMarked.Add(action);
                }
                SelectPieceLock = GameState.MainBoard[pos];
            }
        }

        public static void UnmarkPiece(int pos)
        {
            if (pos == -1) return;
            _tileManager.Unmark(pos);
            foreach (var action in ActionMarked)
            {
                if (action.GetType() == typeof(SirenActive))
                {
                    _tileManager.Unmark(action.From);
                }
                else
                {
                    _tileManager.Unmark(action.To);
                }
                
            }
            ActionMarked.Clear();
            SelectPieceLock = null;
        }
    }
}