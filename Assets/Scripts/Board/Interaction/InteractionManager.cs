using System;
using System.Collections.Generic;
using System.Linq;
using Board.Action;
using Board.Piece;
using Board.Tile;
using Core;
using Core.General;
using Core.Piece;
using Unity.IL2CPP.CompilerServices;

namespace Board.Interaction
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class InteractionManager
    {
        public static PieceLogic SelectPieceLock;
        public static int SelectingPiece;
        public static int MaxRank;
        public static int MaxFile;
        public static PieceManager PieceManager;
        public static TileManager TileManager;
        public static GameState GameState;

        public static void Init(MatchManager match)
        {
            SelectingPiece = -1;
            MaxRank = match.GameState.MaxRank;
            MaxFile = match.GameState.MaxFile;
            TileManager = match.TileManager;
            PieceManager = match.PieceManager;
            GameState = match.GameState;
        }
        
        public static void Select(int rank, int file)
        {
            if (SelectingPiece == -1)
            {
                var selected = rank * MaxFile + file;
                var p = GameState.MainBoard[selected];
                if (p == null || p.Color != GameState.OurSide) return;
                SelectingPiece = selected;
                MarkPiece(selected, null);
                
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
                    if (SelectPieceLock == null)
                    {
                        var action = ActionToTake.Find(x => x.To == selected);
                        if (action != null)
                        {
                            ActionManager.Execute(action);
                        }
                        UnmarkPiece(SelectingPiece);
                        SelectingPiece = -1;
                    }
                    else if (SelectPieceLock.Type == PieceType.GuidingSiren)
                    {
                        var action = ActionToTake.Find(x => x.From == selected && x.GetType() == typeof(SirenActive));
                        if (action != null)
                        {
                            ActionManager.Execute(action);
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
            TileManager.Select(pos);
            ActionToTake = GameState.MainBoard[pos].MoveToMake();

            if (type == null)
            {
                foreach (var action in ActionToTake.Select(action => action).Where(ac => ac.GetType() == typeof(NormalCapture) || ac.GetType() == typeof(NormalMove)))
                {
                    TileManager.MarkAsMoveable(action.To);
                    ActionMarked.Add(action);
                }
            }
            else if (type == typeof(SirenActive))
            {
                foreach (var action in ActionToTake.Select(action => action).Where(ac => ac.GetType() == typeof(SirenActive)))
                {
                    TileManager.MarkAsMoveable(action.From);
                    ActionMarked.Add(action);
                }
                SelectPieceLock = GameState.MainBoard[pos];
            }
        }

        public static void UnmarkPiece(int pos)
        {
            if (pos == -1) return;
            TileManager.Unmark(pos);
            foreach (var action in ActionMarked)
            {
                if (action.GetType() == typeof(SirenActive))
                {
                    TileManager.Unmark(action.From);
                }
                else
                {
                    TileManager.Unmark(action.To);
                }
                
            }
            ActionMarked.Clear();
            SelectPieceLock = null;
        }
    }
}