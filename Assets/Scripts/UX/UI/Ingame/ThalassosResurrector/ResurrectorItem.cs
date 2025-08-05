using Game.Managers;
using Game.Piece;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;

namespace UX.UI.Ingame.ThalassosResurrector
{
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