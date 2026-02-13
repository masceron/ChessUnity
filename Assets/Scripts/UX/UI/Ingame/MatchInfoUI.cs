using Game.Managers;
using TMPro;
using UnityEngine;

namespace UX.UI.Ingame
{
    public class MatchInfoUI : MonoBehaviour
    {
        public TMP_Text round, day;

        private void Start()
        {
            MatchManager.Ins.GameState.OnIncreaseTurn += UpdateUI;
            UpdateUI(1);
        }


        private void UpdateUI(int turn)
        {
            round.text = $"Round : {turn}/150";
            day.text = MatchManager.Ins.GameState.IsDay ? "Day" : "Night";
        }
    }
}