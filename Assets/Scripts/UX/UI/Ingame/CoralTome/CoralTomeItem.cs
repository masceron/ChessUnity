using Game.Managers;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;

namespace UX.UI.Ingame.CoralTome
{
    public class CoralTomeItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text pieceName;
        [SerializeField] private UIObject3D pieceModel;
        private string pieceType;

        public void Load(string type)
        {
            pieceType = type;
            var info = AssetManager.Ins.PieceData[type];
            pieceName.text = Localizer.GetText("piece_name", type, null);
            pieceModel.ObjectPrefab = info.prefab.transform;
        }

        public void OnClickSummon()
        {
            transform.parent.parent.GetComponent<CoralTomeUI>().Choose(pieceType);
        }
    }
}