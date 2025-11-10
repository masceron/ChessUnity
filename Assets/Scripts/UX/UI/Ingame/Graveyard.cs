using UnityEngine;

namespace UX.UI.Ingame
{
    public class Graveyard : MonoBehaviour
    {
        public Transform deathList;
        public void OnClick()
        {
            deathList.gameObject.SetActive(!deathList.gameObject.activeSelf);
        }
    }
}

