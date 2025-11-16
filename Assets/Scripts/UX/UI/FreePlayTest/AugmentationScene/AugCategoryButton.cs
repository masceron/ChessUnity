using UnityEngine;
using UnityEngine.UI;
using Game.Augmentation;

namespace UX.UI.FreePlayTest
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