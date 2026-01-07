using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

enum AppeearType
{
    None,
    FromNagative = 1,
    FromPositive = -1,
}

public class UIAppearAnim : MonoBehaviour
{

    [SerializeField] Vector3 startPos, endPos;
    [SerializeField] Vector3 startScale, endScale;
    [SerializeField] AppeearType _horizontalAppearType = AppeearType.FromPositive;
    [SerializeField] AppeearType _verticalAppearType = AppeearType.None ;

    [SerializeField] bool playOnWake = true, isMoved = true;
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

        Tween.LocalPosition(transform, endPos, duration).OnComplete(() =>
        {
            _OnMoveCompleted?.Invoke();
        });
    }

    public void DoScale()
    {
        transform.localScale = startScale;
        Tween.Scale(transform, endScale, duration, Ease.OutQuad).OnComplete(() =>
        {
            _OnScaleCompleted?.Invoke();
        });
    }
}
