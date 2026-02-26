using Game.Save.Money;
using TMPro;
using UnityEngine;

namespace UX.UI.Money
{
    public class MoneyUIText : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;

        private void OnEnable()
        {
            moneyText.text = MoneySaveLoader.GetMoney().ToString();
        }
    }
}