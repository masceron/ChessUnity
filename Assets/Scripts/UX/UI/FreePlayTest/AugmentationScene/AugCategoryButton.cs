using Game.Augmentation;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.FreePlayTest.AugmentationScene
{
    public class AugCategoryButton : MonoBehaviour
    {
        public AugmentationSlot slot;
        public Button button;
        public Image greyImage;

        public void OnButtonClicked()
        {
            Debug.Log($"{name} category button clicked");
            AugmentationFilter.Ins.ToggleFilter(slot);
        }

        public void GreyOut()
        {
            greyImage.enabled = true;
        }

        public void DeGreyOut()
        {
            greyImage.enabled = false;
        }
    }
}