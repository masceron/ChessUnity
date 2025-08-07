using Game.Managers;
using Game.Piece;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;

namespace UX.UI.Ingame.ChrysosShop
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChrysosShopItem: MonoBehaviour
    {
        [SerializeField] private TMP_Text pieceName;
        private PieceType pieceType;
        [SerializeField] private UIObject3D pieceModel; 

        public void Load(PieceType type)
        {
            pieceType = type;
            var info = AssetManager.Ins.PieceData[type];
            pieceName.text = info.pieceName;
            pieceModel.ObjectPrefab = info.prefab.transform;
        }

        public void OnClickBuy()
        {
            transform.parent.parent.parent.GetComponent<ChrysosShop>().Buy(pieceType);
        }
    }
}