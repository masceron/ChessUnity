using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;

namespace Game.Board.General
{
    public enum ObserverType: byte
    {
        None, Captures, Moves, EndTurn
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class EventObserver
    {
        private static List<Observer> _observersEnd;
        private static List<Observer> _observersCapture;
        private static List<Observer> _observersMove;
        
        //The main action taken this turn.
        private static Action.Action _mainAction;

        public static void Init()
        {
            _observersEnd = new List<Observer>();
            _observersCapture = new List<Observer>();
            _observersMove = new List<Observer>();
            _mainAction = null;
        }

        public static void AddObserver(Observer observer)
        {
            int pos;
            switch (observer.Type)
            {
                case ObserverType.EndTurn:
                    pos = _observersEnd.BinarySearch(observer, observer);
                    _observersEnd.Insert(pos >= 0 ? pos : ~pos, observer);
                    break;
                case ObserverType.Captures:
                    pos = _observersCapture.BinarySearch(observer, observer);
                    _observersCapture.Insert(pos >= 0 ? pos : ~pos, observer);
                    break;
                case ObserverType.Moves:
                    pos = _observersMove.BinarySearch(observer, observer);
                    _observersMove.Insert(pos >= 0 ? pos : ~pos, observer);
                    break;
            }
        }

        public static void RemoveObserver(Observer observer)
        {
            switch (observer.Type)
            {
                case ObserverType.EndTurn:
                    _observersEnd.Remove(observer);
                    break;
                case ObserverType.Captures:
                    _observersCapture.Remove(observer);
                    break;
                case ObserverType.Moves:
                    _observersMove.Remove(observer);
                    break;
            }
        }

        public static void Notify(Action.Action action)
        {
            if (action.GetType() == typeof(EndTurn))
            {
                _observersEnd.ForEach(observer => observer.OnCall(_mainAction));
                _mainAction = null;
                return;
            }

            _mainAction ??= action;
            
            if (action is ICaptures)
            {
                _observersCapture.ForEach(observer => observer.OnCall(action));
            }

            if (action.DoesMoveChangePos)
            {
                _observersMove.ForEach(observer => observer.OnCall(action));
            }
        }
    }
}