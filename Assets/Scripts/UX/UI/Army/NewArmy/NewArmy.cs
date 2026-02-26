using UnityEngine;
using UnityEngine.InputSystem;
using UX.UI.Army.DesignArmy;

namespace UX.UI.Army.NewArmy
{
    public class NewArmy : MonoBehaviour
    {
        public void Create(int size)
        {
            UIManager.Ins.Load(CanvasID.DesignArmy);
            ArmyDesign.Ins.Load(size, null);
        }

        public void Back(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            UIManager.Ins.Load(CanvasID.Followers);
        }
    }
}