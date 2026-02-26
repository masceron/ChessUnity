using Game.ScriptableObjects;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.Followers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TroopDescriptions : MonoBehaviour
    {
        [SerializeField] private UIObject3D image;
        [SerializeField] private RawImage demonstration;
        [SerializeField] private TMP_Text description;
        private PieceInfo displaying;

        public void Display(PieceInfo obj)
        {
            gameObject.SetActive(true);

            if (displaying == obj) return;
            displaying = obj;

            image.ObjectPrefab = obj.prefab.transform;
            demonstration.texture = obj.movePattern;
            if (obj.hasSkill)
                description.text = Localizer.GetText("piece_skill", obj.key + "_skill", null) + ": " +
                                   Localizer.GetText("piece_skill_description", obj.key + "_skill_description", null);
            else description.text = "";
        }

        public void Undisplay()
        {
            gameObject.SetActive(false);
        }
    }
}