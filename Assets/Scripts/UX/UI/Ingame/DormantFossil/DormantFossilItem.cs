using Game.Managers;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;

namespace UX.UI.Ingame.DormantFossil
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DormantFossilItem: MonoBehaviour
    {
        [SerializeField] private TMP_Text pieceName;
        private string pieceType;
        [SerializeField] private UIObject3D pieceModel; 

        public void Load(string type)
        {
            pieceType = type;
            var info = AssetManager.Ins.PieceData[type];
            pieceName.text = Localizer.GetText("piece_name", type, null);
            pieceModel.ObjectPrefab = info.prefab.transform;
        }

        public void OnClickSummon()
        {
            transform.parent.parent.GetComponent<DormantFossilUI>().Choose(pieceType);
        }
    }
}