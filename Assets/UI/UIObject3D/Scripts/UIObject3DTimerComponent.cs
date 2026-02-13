using System;
using System.Collections.Generic;
using UnityEngine;
using ZLinq;

namespace UI.UIObject3D.Scripts
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class UIObject3DTimerComponent : MonoBehaviour
    {
        private readonly List<DelayedAction> delayedActions = new();

        private void Update()
        {
            List<DelayedAction> actionsToExecute = null;
            foreach (var action in delayedActions.Where(action => Time.unscaledTime >= action.timeToExecute))
            {
                actionsToExecute ??= new List<DelayedAction>();
                actionsToExecute.Add(action);
            }

            if (actionsToExecute == null || actionsToExecute.Count == 0) return;

            foreach (var action in actionsToExecute)
                try
                {
                    if (action.forceEvenIfTargetIsInactive
                        || (action.target && action.target.gameObject.activeInHierarchy))
                        action.action.Invoke();
                }
                finally
                {
                    delayedActions.Remove(action);
                }

            // stop calling update if we have nothing scheduled (DelayedCall will re-enable this)
            if (delayedActions.Count == 0) enabled = false;
        }

        public void DelayedCall(float delay, Action action, MonoBehaviour target, bool forceEvenIfTargetIsInactive)
        {
            enabled = true;

            delayedActions.Add(new DelayedAction
            {
                timeToExecute = Time.unscaledTime + delay, action = action, target = target,
                forceEvenIfTargetIsInactive = forceEvenIfTargetIsInactive
            });
        }
    }

    public class DelayedAction
    {
        public Action action;
        public bool forceEvenIfTargetIsInactive;
        public MonoBehaviour target;
        public float timeToExecute;
    }
}