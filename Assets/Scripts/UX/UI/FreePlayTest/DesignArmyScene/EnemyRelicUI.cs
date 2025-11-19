using TMPro;
using UnityEngine;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class EnemyRelicUI : MonoBehaviour
    {
        public TMP_Text tmp;
        void Awake()
        {
            tmp.text = Config.relicBlackConfig.Type.ToString();
        }
    }
}