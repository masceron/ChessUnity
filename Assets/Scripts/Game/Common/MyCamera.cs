using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Game.Common
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MyCamera : MonoBehaviour
    { 
        private FreeCamera moveScript;
        private bool isLocking;
    
        private void Start()
        {
            moveScript = transform.GetComponent<FreeCamera>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void ToggleCursor(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (!isLocking)
            {
                isLocking = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                moveScript.enabled = false;
            }
            else
            {
                isLocking = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                moveScript.enabled = true;
            }
        }
    }
}
