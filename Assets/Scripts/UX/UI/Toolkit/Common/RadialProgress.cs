using UnityEngine;
using UnityEngine.UIElements;

namespace UX.UI.Toolkit.Common
{
    [UxmlElement]
    public partial class RadialProgress: VisualElement
    {
        private float _progress = 1.0f;
        public float Progress
        {
            get => _progress;
            set { _progress = Mathf.Clamp01(value); MarkDirtyRepaint(); }
        }

        public RadialProgress()
        {
            style.position = Position.Absolute;
            style.top = 0; style.left = 0; style.right = 0; style.bottom = 0;
            
            style.overflow = Overflow.Visible;
        
            generateVisualContent += OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            if (_progress >= 1f) return;

            var painter = mgc.painter2D;
            var r = contentRect;
            var center = r.center;
            
            var radius = Mathf.Sqrt(r.width * r.width + r.height * r.height) / 2f;
            
            painter.fillColor = new Color(0, 62f / 255f, 116 / 255f, 1f);
            painter.BeginPath();
        
            painter.MoveTo(center);

            if (_progress <= 0f)
            {
                painter.Arc(center, radius, 0f, 360f);
            }
            else
            {
                var startAngle = -90f + 360f * _progress;
                var sweepAngle = 360f * (1f - _progress);

                painter.Arc(center, radius, startAngle, startAngle + sweepAngle);
                painter.LineTo(center);
            }

            painter.Fill();
        }
    }
}