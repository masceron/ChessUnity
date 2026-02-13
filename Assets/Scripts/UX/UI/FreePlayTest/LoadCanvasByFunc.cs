using Game.Managers;
using UnityEngine;

namespace UX.UI.FreePlayTest
{
    public class LoadCanvasByFunc : MonoBehaviour
    {
        public static GameMode chosenGameMode;

        public void LoadPvP()
        {
            chosenGameMode = GameMode.PlayerVsPlayer;
            LoadFreePlay();
        }

        public void LoadAIvsAIMode()
        {
            LoadFreePlay();
            chosenGameMode = GameMode.AIvsAI;
        }

        public void LoadPvAIMode()
        {
            chosenGameMode = GameMode.PlayerVsAI;
            LoadFreePlay();
        }

        private void LoadFreePlay()
        {
            UIManager.Ins.Load(CanvasID.FreePlayDesignArmy);
        }
    }
}