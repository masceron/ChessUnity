using DG.Tweening;
using Game.UX.UI.Loader;
using UnityEngine;

namespace Game.UX.UI.Menus
{
    public class PlayPanel : MonoBehaviour
    {
        public void OnOpen()
        {
            var contents = transform.GetChild(0).GetChild(0).GetChild(0);
            for (var i = 0; i < contents.childCount; i++)
            {
                var content = contents.GetChild(i);
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
            
        }
        
        public void OnClickTrial()
        {
            
        }

        public void OnClickSanctuary()
        {
            
        }

        public void OnClickBack()
        {
            UIManager.Ins.Load(CanvasID.MainMenu);
        }
    }
}
