using Game.Common;
using TMPro;

namespace UX.UI.Ingame
{
    public class EndGameUI : Singleton<EndGameUI>
    {
        public enum MessageID
        {
            Lose,
            Win,
            Draw
        }

        public TMP_Text tmp;

        public void SetMessage(MessageID messageID)
        {
            tmp.text = messageID.ToString();
        }
    }
}