using System.Collections;
using Game.AI;
using Game.Common;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Menus;

namespace Game.Managers
{
    public class AIvsAIController : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(DoAIAction());
            PauseButton.Ins.Enable();
        }

        private IEnumerator DoAIAction()
        {
            while (true)
            {
                if (PauseButton.isPause)
                {
                    yield return new WaitForSeconds(SettingPanel.AIvsAIplayspeed);
                    continue;
                }

                AIManager.Ins.AIPlayAndExecuteBestAction(BoardUtils.SideToMove());
                yield return new WaitForSeconds(SettingPanel.AIvsAIplayspeed);
            }
        }
    }
}