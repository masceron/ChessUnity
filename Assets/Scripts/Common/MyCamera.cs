using MilkShake;
using UnityEngine;
using UnityEngine.Rendering;

namespace Common
{
    public class MyCamera : MonoBehaviour
    {
        public Shaker shaker;
        public ShakePreset preset;
        private FreeCamera _moveScript;
        private bool _isLocking;
    
        private void Start()
        {
            _moveScript = transform.GetComponent<FreeCamera>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Tab)) return;
            
            if (!_isLocking)
            {
                _isLocking = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                _moveScript.enabled = false;
                return;
            }
            _isLocking = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _moveScript.enabled = true;
        }

        public void Shake()
        {
            shaker.Shake(preset);
        }
    }
}
