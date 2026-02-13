using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ZLinq;
using Object = UnityEngine.Object;

namespace UI.UIObject3D.Scripts
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    internal class DelayedEditorAction
    {
        internal readonly Action Action;
        internal readonly MonoBehaviour ActionTarget;
        internal readonly bool ForceEvenIfTargetIsGone;
        internal readonly double TimeToExecute;

        public DelayedEditorAction(double timeToExecute, Action action, MonoBehaviour actionTarget,
            bool forceEvenIfTargetIsGone = false)
        {
            TimeToExecute = timeToExecute;
            Action = action;
            ActionTarget = actionTarget;
            ForceEvenIfTargetIsGone = forceEvenIfTargetIsGone;
        }
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class UIObject3DTimer
    {
        private static UIObject3DTimerComponent _timerComponent;

        private static UIObject3DTimerComponent TimerComponent
        {
            get
            {
                if (_timerComponent) return _timerComponent;
                _timerComponent = Object.FindAnyObjectByType<UIObject3DTimerComponent>();

                if (_timerComponent || IsQuitting) return _timerComponent;
                var timerGo = new GameObject("UIObject3DTimer");
                _timerComponent = timerGo.AddComponent<UIObject3DTimerComponent>();

                Object.DontDestroyOnLoad(timerGo);

                return _timerComponent;
            }
        }

        public static bool IsFirstFrame => Time.frameCount <= 1;

        private static bool IsQuitting { get; set; }

#if UNITY_2018_1_OR_NEWER
        [RuntimeInitializeOnLoadMethod]
        public static void OnLoad()
        {
            Application.quitting += () => IsQuitting = true;
        }
#endif

        public static WaitForSecondsRealtime GetWaitForSecondsRealtimeInstruction(float seconds)
        {
            return new WaitForSecondsRealtime(seconds);
        }

        public static WaitForSeconds GetWaitForSecondsInstruction(float seconds)
        {
            return new WaitForSeconds(seconds);
        }

        private static void EditorUpdate()
        {
#if UNITY_EDITOR
            if (Application.isPlaying) return;

            var actionsToExecute = DelayedEditorActions
                .Where(dea => EditorApplication.timeSinceStartup >= dea.TimeToExecute).ToList();

            if (actionsToExecute.Count == 0) return;

            foreach (var actionToExecute in actionsToExecute)
                try
                {
                    if (actionToExecute.ActionTarget != null ||
                        actionToExecute.ForceEvenIfTargetIsGone) // don't execute if the target is gone
                        actionToExecute.Action.Invoke();
                }
                finally
                {
                    DelayedEditorActions.Remove(actionToExecute);
                }
#endif
        }

        /// <summary>
        ///     Call Action 'action' after the specified delay, provided the 'actionTarget' is still present and active in the
        ///     scene at that time.
        ///     Can be used in both edit and play modes.
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="action"></param>
        /// <param name="actionTarget"></param>
        public static void DelayedCall(float delay, Action action, MonoBehaviour actionTarget,
            bool forceEvenIfObjectIsInactive = false)
        {
            if (Application.isPlaying)
            {
                if (TimerComponent)
                    TimerComponent.DelayedCall(delay, action, actionTarget, forceEvenIfObjectIsInactive);
            }
#if UNITY_EDITOR
            else
            {
                DelayedEditorActions.Add(new DelayedEditorAction(EditorApplication.timeSinceStartup + delay, action,
                    actionTarget, forceEvenIfObjectIsInactive));
            }
#endif
        }

        /// <summary>
        ///     Shorthand for DelayedCall(0, action, actionTarget)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actionTarget"></param>
        public static void AtEndOfFrame(Action action, MonoBehaviour actionTarget,
            bool forceEvenIfObjectIsInactive = false)
        {
            DelayedCall(0, action, actionTarget, forceEvenIfObjectIsInactive);
        }

#if UNITY_EDITOR
        private static readonly List<DelayedEditorAction> DelayedEditorActions = new();

        static UIObject3DTimer()
        {
            EditorApplication.update += EditorUpdate;
        }
#endif
    }
}