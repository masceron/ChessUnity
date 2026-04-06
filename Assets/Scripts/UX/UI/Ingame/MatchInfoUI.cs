using Game.Common;
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
            //UpdateUI(1);
        }


        //Làm lại
        // private void UpdateUI(int turn)
        // {
        //     round.text = $"Round : {turn}/150";
        //     day.text = BoardUtils.IsDay() ? "Day" : "Night";
        // }
    }
}