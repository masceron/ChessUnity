using Game.Common;
using TMPro;

namespace UX.UI.Ingame
{
    public class EndGameUI : Singleton<EndGameUI>
    {
        public TMP_Text tmp;
        public enum MessageID
        {
            Lose,
            Win,
            Draw,
        }
        public void SetMessage(MessageID messageID)
        {
            tmp.text = messageID.ToString();
        }
    
    }
}
