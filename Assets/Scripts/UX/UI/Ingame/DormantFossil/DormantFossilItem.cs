using Game.Managers;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;

namespace UX.UI.Ingame.DormantFossil
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DormantFossilItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text pieceName;
        [SerializeField] private UIObject3D pieceModel;
        private string _pieceType;

        public void Load(string type)
        {
            _pieceType = type;
            var info = AssetManager.Ins.PieceData[type];
            pieceName.text = Localizer.GetText("piece_name", type, null);
            pieceModel.ObjectPrefab = info.prefab.transform;
        }

        public void OnClickSummon()
        {
            transform.parent.parent.GetComponent<DormantFossilUI>().Choose(_pieceType);
        }
    }
}