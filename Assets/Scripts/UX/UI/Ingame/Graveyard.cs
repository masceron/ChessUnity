using UnityEngine;

namespace UX.UI.Ingame
{
    public class Graveyard : MonoBehaviour
    {
        public Transform deathList;
        void Start()
        {
            deathList.gameObject.SetActive(false);
        }
        public void OnClick()
        {
            deathList.gameObject.SetActive(!deathList.gameObject.activeSelf);
        }
    }
}

