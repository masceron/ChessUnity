#region Namespace Imports

using System;
using UnityEngine;

#endregion

namespace UI.UIObject3D.Scripts
{
    [RequireComponent(typeof(UIObject3D)), ExecuteInEditMode]
    [AddComponentMenu("UI/UIObject3D/UIObject3D Light")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class UIObject3DLight : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _LightPosition = new(0, 0, -2.5f);
        public Vector3 LightPosition
        {
            get => _LightPosition;
            set
            {
                _LightPosition = value;
                SetLightPosition();
            }
        }

        [SerializeField]
        private Color _LightColor = Color.white;
        public Color LightColor
        {
            get => _LightColor;
            set
            {
                _LightColor = value;
                SetLightProperties();
            }
        }

        [SerializeField, Range(0, 8)]
        private float _LightIntensity = 1f;
        public float LightIntensity
        {
            get => _LightIntensity;
            set
            {
                _LightIntensity = value;
                SetLightProperties();
            }
        }

        [NonSerialized]
        private UIObject3D UIObject3D;

        [NonSerialized]
        private Light _lightObject;
        private Light lightObject
        {
            get
            {
                if (!_lightObject) SpawnLight();

                return _lightObject;
            }
            set => _lightObject = value;
        }

        private void OnEnable()
        {
            if (!UIObject3D) UIObject3D = GetComponent<UIObject3D>();

            UIObject3D.OnUpdateTarget.AddListener(UpdateLightEvent);

            lightObject.enabled = true;
            UpdateLight(true);
        }

        private void OnDisable()
        {
            UIObject3D.OnUpdateTarget.RemoveListener(UpdateLightEvent);

            if (!_lightObject) return;
            lightObject.enabled = false;
            ScheduleRender();
        }

        private void UpdateLightEvent()
        {
            UpdateLight(true);
        }

        public void UpdateLight(bool scheduleRender = false)
        {
            if (!enabled) return;

            if (!lightObject)
            {
                SpawnLight();
            }

            SetLightPosition(false);
            SetLightProperties(false);

            if (scheduleRender) ScheduleRender();
        }

        private void SpawnLight()
        {
            var lightGO = new GameObject("UIObject3DLight", typeof(Light));
            _lightObject = lightGO.GetComponent<Light>();

            _lightObject.transform.localScale = Vector3.one;
            _lightObject.transform.SetParent(UIObject3D.container.gameObject.transform);

            _lightObject.range = 200;
            _lightObject.cullingMask = LayerMask.GetMask(LayerMask.LayerToName(UIObject3D.objectLayer));
            _lightObject.type = LightType.Point;
            _lightObject.bounceIntensity = 0;
        }

        private void SetLightPosition(bool scheduleRender = true)
        {
            lightObject.transform.localPosition = LightPosition;

            if (scheduleRender) ScheduleRender();
        }

        private void SetLightProperties(bool scheduleRender = true)
        {
            lightObject.intensity = LightIntensity;
            lightObject.color = LightColor;

            if (scheduleRender) ScheduleRender();
        }

        private void ScheduleRender()
        {
            if (!UIObject3D || !enabled) return;

            if (!Application.isPlaying)
            {
                UIObject3DTimer.AtEndOfFrame(() =>
                {
                    UIObject3D.OnUpdateTarget.RemoveListener(UpdateLightEvent);
                    UIObject3D.UpdateDisplay();
                    UIObject3D.OnUpdateTarget.AddListener(UpdateLightEvent);
                }, this);
            }
            else
            {
                UIObject3D.Render();
            }
        }
    }
}
