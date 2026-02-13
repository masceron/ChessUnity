using Game.Action.Internal.Pending;
using UnityEngine;

namespace UX.UI.Ingame
{
    public abstract class IngamePendingMenu : MonoBehaviour
    {
        protected abstract PendingAction PendingAction { get; set; }

        protected void OnDisable()
        {
            PendingAction?.CancelResult();
        }
    }
}