using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UX.UI.Tooltip
{
    //Hiển thị Description cửa Piece
    [ExecuteInEditMode]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private RectTransform whole;
        [SerializeField] private RectTransform header;
        [SerializeField] private TMP_Text headerLeft;
        [SerializeField] private TMP_Text headerRight;
        [SerializeField] private TMP_Text content;
        [SerializeField] private LayoutElement layout;

        public void SetText(string headerLeftText, string headerRightText, string contentText)
        {
            headerLeft.text = headerLeftText;
            headerRight.text = headerRightText;
            content.text = contentText;
            layout.enabled = contentText.Length > 50;

            var mousePos = Mouse.current.position;
            var pivotX = mousePos.x.ReadValue() + whole.rect.width > Screen.width ? 1 : 0;
            var pivotY = mousePos.y.ReadValue() + whole.rect.height > Screen.height ? 1 : 0;
            whole.pivot = new Vector2(pivotX, pivotY);

            transform.position = mousePos.value;
        }
    }
}