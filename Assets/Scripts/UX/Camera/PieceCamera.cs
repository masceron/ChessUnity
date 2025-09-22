using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UX.Camera
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceCamera : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private PlayerInput input;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private CinemachineOrbitalFollow orbitalFollow;
        
        private InputAction move;
        private InputAction zoom;

        [Header("Speed")] 
        [SerializeField] private float moveSpeed;
        [SerializeField] private float zoomSpeed;
        [SerializeField] private float zoomRotationSpeed;

        private void Awake()
        {
            move = input.actions["CameraMove"];
            zoom = input.actions["CameraZoom"];
        }

        private void Update()
        {
            CameraMove(Time.unscaledDeltaTime);
            CameraZoom(Time.unscaledDeltaTime);
        }

        private void CameraMove(float dt)
        {
            var tmp = cameraTransform.transform.position;
            var scale = dt * moveSpeed;

            var direction = move.ReadValue<Vector2>().normalized;
            
            tmp.z += direction.x * scale;
            tmp.x -= direction.y * scale;
            
            cameraTransform.transform.position = tmp;
        }

        private void CameraZoom(float dt)
        {
            var zoomDelta = zoom.ReadValue<float>();

            if (zoomDelta == 0) return;
            
            var radialAxis = orbitalFollow.RadialAxis;
            var verticalAxis = orbitalFollow.VerticalAxis;
                
            var newRadial = radialAxis.Value + zoomSpeed * dt * -zoomDelta;
            radialAxis.Value = Math.Clamp(newRadial, radialAxis.Range.x, radialAxis.Range.y);
                
            var newVertical = verticalAxis.Value + zoomRotationSpeed * dt * -zoomDelta;
            verticalAxis.Value = Math.Clamp(newVertical, verticalAxis.Range.x, verticalAxis.Range.y);
                    
            orbitalFollow.RadialAxis = radialAxis;
            orbitalFollow.VerticalAxis = verticalAxis;
        }
    }
}
