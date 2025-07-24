using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Simple_Tooltip.Assets.Scripts
{
    public class StController : MonoBehaviour
    {
        public enum TextAlign { Left, Right };

        private Image panel;
        private TextMeshProUGUI toolTipTextLeft;
        private TextMeshProUGUI toolTipTextRight;
        private RectTransform rect;
        private int showInFrames = -1;
        private bool showNow;
    
        private void Awake()
        {
            // Load up both text layers
            var tmps = GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var t in tmps)
            {
                switch (t.name)
                {
                    case "_left":
                        toolTipTextLeft = t;
                        break;
                    case "_right":
                        toolTipTextRight = t;
                        break;
                }
            }

            // Keep a reference for the panel image and transform
            panel = GetComponent<Image>();
            rect = GetComponent<RectTransform>();

            // Hide at the start
            HideTooltip();
        }

        void Update()
        {
            ResizeToMatchText();
            UpdateShow();
        }

        private void ResizeToMatchText()
        {
            // Find the biggest height between both text layers
            var biggestY = toolTipTextLeft.textBounds.size.y;
            var rightY = toolTipTextRight.textBounds.size.y;
            if (rightY > biggestY)
                biggestY = rightY;

            // Dont forget to add the margins
            var margins = toolTipTextLeft.margin.y * 2;

            // Update the height of the tooltip panel
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, biggestY + margins);
        }

        private void UpdateShow()
        {
            if (showInFrames == -1)
                return;

            if (showInFrames == 0)
                showNow = true;

            if (showNow)
            {
                rect.anchoredPosition = Mouse.current.position.ReadValue();
            }

            showInFrames -= 1;
        }

        public void SetRawText(string text, TextAlign align = TextAlign.Left)
        {
            // Doesn't change style, just the text
            if(align == TextAlign.Left)
                toolTipTextLeft.text = text;
            if (align == TextAlign.Right)
                toolTipTextRight.text = text;
            ResizeToMatchText();
        }

        public void SetCustomStyledText(string text, SimpleTooltipStyle style, TextAlign align = TextAlign.Left)
        {
            // Update the panel sprite and color
            panel.sprite = style.slicedSprite;
            panel.color = style.color;

            // Update the font asset, size and default color
            toolTipTextLeft.font = style.fontAsset;
            toolTipTextLeft.color = style.defaultColor;
            toolTipTextRight.font = style.fontAsset;
            toolTipTextRight.color = style.defaultColor;

            // Convert all tags to TMPro markup
            var styles = style.fontStyles;
            for(int i = 0; i < styles.Length; i++)
            {
                string addTags = "</b></i></u></s>";
                addTags += "<color=#" + ColorToHex(styles[i].color) + ">";
                if (styles[i].bold) addTags += "<b>";
                if (styles[i].italic) addTags += "<i>";
                if (styles[i].underline) addTags += "<u>";
                if (styles[i].strikethrough) addTags += "<s>";
                text = text.Replace(styles[i].tag, addTags);
            }
            switch (align)
            {
                case TextAlign.Left:
                    toolTipTextLeft.text = text;
                    break;
                case TextAlign.Right:
                    toolTipTextRight.text = text;
                    break;
            }

            ResizeToMatchText();
        }

        private static string ColorToHex(Color color)
        {
            var r = (int)(color.r * 255);
            var g = (int)(color.g * 255);
            var b = (int)(color.b * 255);
            return r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
        }

        public void ShowTooltip()
        {
            // After 2 frames, showNow will be set to TRUE
            // after that the frame count won't matter
            if (showInFrames == -1)
                showInFrames = 2;
        }

        public void HideTooltip()
        {
            showInFrames = -1;
            showNow = false;
            rect.anchoredPosition = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        }
    }
}
