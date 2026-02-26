#region Namespace Imports

using UnityEngine;
using UnityEngine.EventSystems;

#endregion

namespace UI.UIObject3D.Scripts
{
    [RequireComponent(typeof(UIObject3D))]
    [AddComponentMenu("UI/UIObject3D/Drag Rotate UIObject3D")]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DragRotateUIObject3D : MonoBehaviour
    {
        [Header("Speed")] public float RotationSpeed = 10f;

        [Header("X")] public bool RotateX = true;

        public bool InvertX;

        [Header("Y")] public bool RotateY = true;

        public bool InvertY;

        [Header("Inertia")] public bool UseInertia;

        public float SlowSpeed = 1f;
        private Vector3 averageSpeed = Vector3.zero;
        private bool beingDragged;

        private Vector2 lastMousePosition = Vector2.zero;

        private Vector3 speed = Vector3.zero;

        private UIObject3D UIObject3D;
        private int _xMultiplier => InvertX ? -1 : 1;
        private int _yMultiplier => InvertY ? -1 : 1;

        private void Awake()
        {
            UIObject3D = GetComponent<UIObject3D>();

            SetupEvents();
        }

        private void Update()
        {
            if (!UIObject3D || !UIObject3D.targetContainer) return;

            if (lastMousePosition == Vector2.zero) lastMousePosition = Input.mousePosition;

            if (Input.GetMouseButton(0) && beingDragged)
            {
                var mouseDelta = ((Vector2)Input.mousePosition - lastMousePosition) * 100;
                mouseDelta.Set(mouseDelta.x / Screen.width, mouseDelta.y / Screen.height);

                speed = new Vector3(-mouseDelta.x * _xMultiplier, mouseDelta.y * _yMultiplier, 0);
                averageSpeed = Vector3.Lerp(averageSpeed, speed, Time.deltaTime * 5);
            }
            else
            {
                if (beingDragged)
                {
                    speed = averageSpeed;
                    beingDragged = false;
                }

                if (UseInertia)
                {
                    var i = Time.deltaTime * SlowSpeed;
                    speed = Vector3.Lerp(speed, Vector3.zero, i);
                }
                else
                {
                    speed = Vector3.zero;
                }
            }

            if (speed != Vector3.zero)
            {
                if (RotateX)
                    UIObject3D.targetContainer.Rotate(Camera.main!.transform.up * (speed.x * RotationSpeed),
                        Space.World);
                if (RotateY)
                    UIObject3D.targetContainer.Rotate(Camera.main!.transform.right * (speed.y * RotationSpeed),
                        Space.World);
                UIObject3D.TargetRotation = UIObject3D.targetContainer.localRotation.eulerAngles;
            }

            lastMousePosition = Input.mousePosition;
        }

        private void SetupEvents()
        {
            // get or add the event trigger
            var trigger = GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();

            var onPointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            onPointerDown.callback.AddListener(_ => beingDragged = true);
            trigger.triggers.Add(onPointerDown);
        }
    }
}