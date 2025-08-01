using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Effects;

namespace Game.Board.General
{
    public enum ObserverType: byte
    {
        None, Captures, Moves, EndTurn, ToTakeAction
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class EventObserver
    {
        private static List<Effect> _observersEnd;
        private static List<Effect> _observersCapture;
        private static List<Effect> _observersMove;
        private static List<Effect> _observerTakeAction;
        
        //The main action taken this turn.
        private static Action.Action _mainAction;

        public static void Init()
        {
            _observersEnd = new List<Effect>();
            _observersCapture = new List<Effect>();
            _observersMove = new List<Effect>();
            _observerTakeAction = new List<Effect>();
            _mainAction = null;
        }

        public static void AddObserver(Effect effect)
        {
            int pos;
            switch (effect.ObserverType)
            {
                case ObserverType.EndTurn:
                    pos = _observersEnd.BinarySearch(effect, effect);
                    _observersEnd.Insert(pos >= 0 ? pos : ~pos, effect);
                    break;
                case ObserverType.Captures:
                    pos = _observersCapture.BinarySearch(effect, effect);
                    _observersCapture.Insert(pos >= 0 ? pos : ~pos, effect);
                    break;
                case ObserverType.Moves:
                    pos = _observersMove.BinarySearch(effect, effect);
                    _observersMove.Insert(pos >= 0 ? pos : ~pos, effect);
                    break;
                case ObserverType.ToTakeAction:
                    pos = _observerTakeAction.BinarySearch(effect, effect);
                    _observerTakeAction.Insert(pos >= 0 ? pos : ~pos, effect);
                    break;
                case ObserverType.None: default:
                    break;
            }
        }

        public static void RemoveObserver(Effect effect)
        {
            switch (effect.ObserverType)
            {
                case ObserverType.EndTurn:
                    _observersEnd.Remove(effect);
                    break;
                case ObserverType.Captures:
                    _observersCapture.Remove(effect);
                    break;
                case ObserverType.Moves:
                    _observersMove.Remove(effect);
                    break;
                case ObserverType.ToTakeAction:
                    _observerTakeAction.Remove(effect);
                    break;
                case ObserverType.None: default:
                    break;
            }
        }

        public static void Notify(Action.Action action)
        {
            if (action.GetType() == typeof(EndTurn))
            {
                _observersEnd.ForEach(effect =>
                {
                    //The next turn is of the opponent.
                    if (MatchManager.Ins.GameState.SideToMove != effect.Piece.Color)
                    {
                        if (((IEndTurnEffect)effect).EndTurnEffectType == EndTurnEffectType.AtEnemyTurn)
                        {
                            ((IEndTurnEffect)effect).OnCallEnd(_mainAction);
                        }
                    }
                    //The next turn is ours.
                    else
                    {
                        if (((IEndTurnEffect)effect).EndTurnEffectType == EndTurnEffectType.AtAllyTurn)
                        {
                            ((IEndTurnEffect)effect).OnCallEnd(_mainAction);
                        }
                    }
                });
                _mainAction = null;
                return;
            }

            _mainAction ??= action;
            
            if (action is ICaptures)
            {
                _observersCapture.ForEach(effect => effect.OnCall(action));
            }

            if (action.DoesMoveChangePos)
            {
                _observersMove.ForEach(effect => effect.OnCall(action));
            }
        }

        public static void Notify(List<Action.Action> actions)
        {
            _observerTakeAction.ForEach(e => e.OnCall(actions));
        }
    }
}