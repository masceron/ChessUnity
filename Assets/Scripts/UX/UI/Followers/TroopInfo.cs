using Game.Data.Pieces;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.Followers
{
    public class TroopInfo: MonoBehaviour
    {
        [SerializeField] private UIObject3D image;
        [SerializeField] private RawImage demonstration;
        [SerializeField] private TMP_Text description;
        
        public void Display(PieceObject obj)
        {
            gameObject.SetActive(true);
            image.ObjectPrefab = obj.prefab.transform;
            description.text = obj.skillDescription;
            demonstration.texture = obj.movePattern;
        }

        public void Undisplay()
        {
            gameObject.SetActive(false);
        }
    }
}