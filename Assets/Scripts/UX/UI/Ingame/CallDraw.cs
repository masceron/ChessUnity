using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.Ingame
{
    [RequireComponent(typeof(Button))]
    public class CallDraw : MonoBehaviour
    {
        [SerializeField] private CallDraw opponent;
        [SerializeField] private Button button;
        [SerializeField] private Image targetGraphic; // usually button.image
        [SerializeField] private UnityEngine.Color toggledColor = UnityEngine.Color.yellow;

        private UnityEngine.Color defaultColor;
        private bool isToggled;

        void Awake()
        {
            if (button == null) button = GetComponent<Button>();
            if (targetGraphic == null) targetGraphic = button.targetGraphic as Image;
            defaultColor = targetGraphic.color;
            button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            isToggled = !isToggled;
            targetGraphic.color = isToggled ? toggledColor : defaultColor;
            if (this.IsToggled() && opponent.IsToggled())
            {
                UIManager.Ins.Load(CanvasID.EndGameMessage);
                EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Draw);
            }
        }
        public bool IsToggled()
        {
            return isToggled;
        }
    }


}
