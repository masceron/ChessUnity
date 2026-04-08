using System;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UX.Camera
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceCamera : MonoBehaviour
    {
        [NonSerialized] private CameraInput _input;

        [Header("Camera")] [SerializeField] private Transform cameraTransform;
        [SerializeField] private CinemachineOrbitalFollow orbitalFollow;

        [Header("Speed")] [SerializeField] private float moveSpeed;
        [SerializeField] private float zoomSpeed;
        [SerializeField] private float zoomRotationSpeed;

        [Header("Spin Settings")] [SerializeField]
        private float spinAngle;

        [SerializeField] private float spinDuration;

        private Vector2 _currentMoveInput;
        private float _currentZoomInput;
        private float _currentSpinInput;

        private Transform _mainCameraTransform;
        private bool _isSpinning;

        private void Awake()
        {
            _input = new CameraInput();

            if (UnityEngine.Camera.main)
            {
                _mainCameraTransform = UnityEngine.Camera.main.transform;
            }

            _input.CameraActions.CameraMove.performed += ctx => _currentMoveInput = ctx.ReadValue<Vector2>();
            _input.CameraActions.CameraMove.canceled += _ => _currentMoveInput = Vector2.zero;

            _input.CameraActions.CameraZoom.performed += ctx => _currentZoomInput = ctx.ReadValue<float>();
            _input.CameraActions.CameraZoom.canceled += _ => _currentZoomInput = 0f;

            _input.CameraActions.CameraSpin.performed += HandleSpinInput;
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return; 
            }
            CameraMove(Time.unscaledDeltaTime);
            CameraZoom(Time.unscaledDeltaTime);
        }

        private void CameraMove(float dt)
        {
            if (_currentMoveInput == Vector2.zero) return;

            var inputDirection = new Vector3(_currentMoveInput.x, 0, _currentMoveInput.y).normalized;
            var relativeDirection = _mainCameraTransform.rotation * inputDirection;

            relativeDirection.y = 0;
            relativeDirection.Normalize();

            cameraTransform.position += relativeDirection * (moveSpeed * dt);
        }

        private void CameraZoom(float dt)
        {
            if (_currentZoomInput == 0) return;

            var radialAxis = orbitalFollow.RadialAxis;
            var verticalAxis = orbitalFollow.VerticalAxis;

            var newRadial = radialAxis.Value + zoomSpeed * dt * -_currentZoomInput;
            radialAxis.Value = Math.Clamp(newRadial, radialAxis.Range.x, radialAxis.Range.y);

            var newVertical = verticalAxis.Value + zoomRotationSpeed * dt * -_currentZoomInput;
            verticalAxis.Value = Math.Clamp(newVertical, verticalAxis.Range.x, verticalAxis.Range.y);

            orbitalFollow.RadialAxis = radialAxis;
            orbitalFollow.VerticalAxis = verticalAxis;
        }

        private void HandleSpinInput(InputAction.CallbackContext ctx)
        {
            if (_isSpinning) return;

            var inputDirection = ctx.ReadValue<float>();
            if (inputDirection == 0) return;

            _isSpinning = true;

            var currentAngle = orbitalFollow.HorizontalAxis.Value;
            var targetAngle = currentAngle + inputDirection * spinAngle;

            Tween.Custom(
                    startValue: orbitalFollow.HorizontalAxis.Value,
                    endValue: targetAngle,
                    duration: spinDuration,
                    onValueChange: x =>
                    {
                        var axis = orbitalFollow.HorizontalAxis;
                        axis.Value = x;
                        orbitalFollow.HorizontalAxis = axis;
                    },
                    ease: Ease.OutBack
                )
                .OnComplete(() =>
                    {
                        _isSpinning = false;
                    }
                );
        }
    }
}