using Game.Managers;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;

namespace UX.UI.Ingame.ThalassosResurrector
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ResurrectorItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text pieceName;
        [SerializeField] private UIObject3D pieceModel;
        private string pieceType;

        public void Load(string type)
        {
            pieceType = type;
            var info = AssetManager.Ins.PieceData[type];
            pieceName.text = Localizer.GetText("piece_name", info.key, null);
            pieceModel.ObjectPrefab = info.prefab.transform;
        }

        public void Choose()
        {
            transform.parent.parent.GetComponent<ThalassosResurrector>().Choose(pieceType);
        }
    }
}