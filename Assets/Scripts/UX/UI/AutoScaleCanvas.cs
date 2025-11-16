using UnityEngine;
using UnityEngine.UI;

namespace UX.UI
{
    [RequireComponent(typeof(CanvasScaler))]
    public class AutoScaleCanvas : MonoBehaviour
    {
        private CanvasScaler _scaler;

        private void Awake()
        {
            _scaler = GetComponent<CanvasScaler>();
        }

        private void Update()
        {
            if (((float)Screen.height / (float)Screen.width) <= (9f / 16f))
            {
                _scaler.matchWidthOrHeight = 1;
            }
            else
            {
                _scaler.matchWidthOrHeight = 0;
            }
        }
    }
}