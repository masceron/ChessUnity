using UnityEngine;
using Game.Managers;
using TMPro;


namespace UX.UI.Ingame
{
    public class MatchInfoUI : MonoBehaviour
    {
        public TMP_Text round, day;

        void Start()
        {
            MatchManager.Ins.GameState.OnIncreaseTurn += UpdateUI;
        }

        
        void UpdateUI(int turn)
        {
            round.text = $"Round : {turn}/150";
            day.text = MatchManager.Ins.GameState.IsDay ? "Day" : "Night";
        }
    }

}
