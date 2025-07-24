using System.Collections.Generic;
using System.Linq;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal.Pending;
using Game.Board.Action.Quiets;
using Game.Board.Action.Skills;
using Game.Board.General;
using Game.UI;
using UnityEngine;

namespace Game.Interaction
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class BoardInteractionUtils
    {

        public static int Selecting;
        private static BoardViewer _viewer;
        private static List<Action> _moveList;
        private static List<Action> _listOf;

        public static void Init(BoardViewer v)
        {
            Selecting = -1;
            _viewer = v;
            _listOf = new List<Action>();
        }
        public static bool MarkMove()
        {
            MatchManager.tileManager.UnmarkAll();
            _listOf.Clear();
            
            foreach (var move in _moveList.OfType<IQuiets>())
            {
                _listOf.Add((Action)move);
                MatchManager.tileManager.MarkAsMoveable(((Action)move).To);
            }
            
            return _listOf.Count > 0;
        }

        public static bool MarkCapture()
        {
            MatchManager.tileManager.UnmarkAll();
            _listOf.Clear();
            
            foreach (var move in _moveList.OfType<ICaptures>())
            {
                _listOf.Add((Action)move);
                MatchManager.tileManager.MarkAsMoveable(((Action)move).To);
            }

            return _listOf.Count > 0;
        }
        
        public static bool MarkSkill()
        {
            MatchManager.tileManager.UnmarkAll();
            _listOf.Clear();
            
            foreach (var move in _moveList.OfType<ISkills>())
            {
                _listOf.Add((Action)move);
                MatchManager.tileManager.MarkAsMoveable(((Action)move).To);
            }
            
            return _listOf.Count > 0;
        }
        

        public static void Unmark()
        {
            Selecting = -1;
            MatchManager.tileManager.UnmarkAll();
            _viewer.DisablePieceInteractions();
            _listOf.Clear();
            _viewer.SetPieceHover(-1);
        }

        public static void ExecuteAction(Action action)
        {
            ActionManager.EnqueueAction(action);
            Unmark();
            NewTurn();
        }

        public static void MarkPiece(int pos)
        {
            if (MatchManager.gameState.SideToMove != MatchManager.gameState.OurSide) return;
            
            if (Selecting != -1)
            {
                if (_viewer.SelectingFunction == 0) return;
                
                var action = _listOf.Find(a => a.To == pos);
                switch (action)
                {
                    case null:
                        return;
                    case IPendingAble pending:
                        pending.CompleteAction();
                        return;
                }

                ExecuteAction(action);
            }
            else
            {
                var piece = MatchManager.gameState.MainBoard[pos];
                if (piece == null) return;
                
                _viewer.SetPieceHover(pos);
                MatchManager.tileManager.Select(pos);
                Selecting = pos;

                if (piece.color != MatchManager.gameState.OurSide) return;
                
                _viewer.EnablePieceInteractions();
                _moveList = MatchManager.gameState.MainBoard[Selecting].MoveList();
            }
        }

        public static void NewTurn()
        {
            ActionManager.EnqueueAction(new EndTurn());
            
            if (MatchManager.gameState.SideToMove != MatchManager.gameState.OurSide)
            {
                _viewer.DisableGameInteractions();
            }
            else
            {
                _viewer.EnableGameInteractions();
            }
        }

        public static void Hover(int pos)
        {
            _viewer.SetPieceHover(pos);
        }

        public static GameObject ChrysosShop()
        {
            return _viewer.ChrysosShopUI;
        }
    }
}