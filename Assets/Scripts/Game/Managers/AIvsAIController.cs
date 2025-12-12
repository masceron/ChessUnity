using System.Collections;
using Game.AI;
using UnityEditor.SettingsManagement;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Menus;

namespace Game.Managers
{
    public class AIvsAIController : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(DoAIAction());
            PauseButton.Ins.Enable();
        }
        IEnumerator DoAIAction()
        {
            while (true)
            {
                if (PauseButton.isPause)
                {
                    yield return new WaitForSeconds(SettingPanel.AIvsAIplayspeed);
                    continue;
                }
                AIManager.Ins.PlayBestActionForSide(MatchManager.Ins.GameState.SideToMove);
                yield return new WaitForSeconds(SettingPanel.AIvsAIplayspeed);
            }
        }

    }
}