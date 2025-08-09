using Data.UI.UIObject3D.Scripts;
using Game.Managers;
using Game.Piece;
using TMPro;
using UnityEngine;

namespace UX.UI.Ingame.ThalassosResurrector
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ResurrectorItem: MonoBehaviour
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

        public void Choose()
        {
            transform.parent.parent.GetComponent<ThalassosResurrector>().Choose(pieceType);
        }
    }
}