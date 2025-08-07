using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UX.UI.Loader;

namespace UX.UI.Menus
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PlayPanel : MonoBehaviour
    {
        public void OnOpen()
        {
            var contents = transform.GetChild(0).GetChild(0).GetChild(0);
            for (var i = 0; i < contents.childCount; i++)
            {
                var content = contents.GetChild(i);
                content.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
                content.transform.rotation = new Quaternion
                {
                    eulerAngles = new Vector3(0, 90, 0)
                };
                content.transform.DORotate(new Vector3(0, 0, 0), 0.25f);
            }
        }
        
        public void OnClickCampaign()
        {
            SceneLoader.LoadSceneWithLoadingScreen(1);
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
    }
}
