using Game.Common;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;
using UX.UI.Loader;

namespace UX.UI.Menus
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StartGame : Singleton<StartGame>
    {
        [SerializeField] private RectTransform contents;
        private void OnEnable()
        {
            for (var i = 0; i < contents.childCount; i++)
            {
                var content = contents.GetChild(i);
                content.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
                content.transform.rotation = new Quaternion
                {
                    eulerAngles = new Vector3(0, 90, 0)
                };
                
                Tween.Rotation(content.transform, Vector3.zero, 0.25f);
            }
        }

        public void OnClickAssignments()
        {
            UIManager.Ins.Load(CanvasID.Followers);
        }

        public void OnClickTrainingGround()
        {
            UIManager.Ins.Load(CanvasID.TrainingGround);
        }

        public void OnClickTrader()
        {
            UIManager.Ins.Load(CanvasID.Trader);
        }

        public void OnClickMurkyTower()
        {
            UIManager.Ins.Load(CanvasID.MurkyTower);
        }

        public void OnClickOutworldInvaders()
        {
            UIManager.Ins.Load(CanvasID.OutworldInvader);
        }

        public void OnClickVault()
        {
            UIManager.Ins.Load(CanvasID.Vault);
        }
        
        public void OnClickCampaign()
        {
            SceneLoader.LoadSceneWithLoadingScreen(1);
        }
        public void OnClickFreePlayTest()
        {
            SceneLoader.LoadSceneWithLoadingScreen(2);
        }
        public void OnClickMultiplayer()
        {
            
        }
        
        public void OnClickFollowers() 
        {
            UIManager.Ins.Load(CanvasID.Followers);
        }
        
        public void OnClickTrial()
        {
            
        }

        public void OnClickSanctuary()
        {
            
        }

        public void OnClickBack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            UIManager.Ins.Load(CanvasID.MainMenu);
        }

        public void OnClickReturn()
        {
            UIManager.Ins.LoadPreviousCanvas();
        }
        
    }
}
