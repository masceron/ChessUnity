using Data.UI.UIObject3D.Scripts;
using Game.Data.Pieces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.Followers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TroopInfo: MonoBehaviour
    {
        [SerializeField] private UIObject3D image;
        [SerializeField] private RawImage demonstration;
        [SerializeField] private TMP_Text description;
        private PieceObject displaying;
        
        public void Display(PieceObject obj)
        {
            gameObject.SetActive(true);
            
            if (displaying == obj) return;
            displaying = obj;
            
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