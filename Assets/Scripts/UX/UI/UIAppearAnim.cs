using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace UX.UI
{
    internal enum AppeearType
    {
        None,
        FromNagative = 1,
        FromPositive = -1
    }

    public class UIAppearAnim : MonoBehaviour
    {
        [SerializeField] private Vector3 startPos, endPos;
        [SerializeField] private Vector3 startScale, endScale;
        [SerializeField] private AppeearType _horizontalAppearType = AppeearType.FromPositive;
        [SerializeField] private AppeearType _verticalAppearType = AppeearType.None;

        [SerializeField] private bool playOnWake = true, isMoved = true;
        [SerializeField] private float duration = .35f;
        public UnityEvent _OnMoveCompleted, _OnScaleCompleted;

        private void OnEnable()
        {
            endPos = transform.localPosition;
            var xOffset = 0;
            var yOffset = 0;
            if (_horizontalAppearType == AppeearType.FromPositive)
                xOffset = 2000;
            else if (_horizontalAppearType == AppeearType.FromNagative)
                xOffset = -2000;
            if (_verticalAppearType == AppeearType.FromPositive)
                yOffset = 2000;
            else if (_verticalAppearType == AppeearType.FromNagative)
                yOffset = -2000;
            if (isMoved)
                startPos = new Vector3(endPos.x + xOffset, endPos.y + yOffset, endPos.z);
            else
                startPos = endPos;
            if (playOnWake)
                Appear();
        }

        public void Appear()
        {
            DoMove();
            DoScale();
        }

        public void DoMove()
        {
            transform.localPosition = startPos;

            Tween.LocalPosition(transform, endPos, duration).OnComplete(() => { _OnMoveCompleted?.Invoke(); });
        }

        public void DoScale()
        {
            transform.localScale = startScale;
            Tween.Scale(transform, endScale, duration, Ease.OutQuad).OnComplete(() => { _OnScaleCompleted?.Invoke(); });
        }
    }
}