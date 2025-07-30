using Game.Board.Piece;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;
using static Game.Board.General.MatchManager;

namespace Game.UX.UI.ThalassosResurrector
{
    public class ResurrectorItem: MonoBehaviour
    {
        [SerializeField] private TMP_Text pieceName;
        private PieceType pieceType;
        [SerializeField] private UIObject3D pieceModel;
        
        public void Load(PieceType type)
        {
            pieceType = type;
            var info = assetManager.PieceData[type];
            pieceName.text = info.pieceName != "" ? info.pieceName : type.ToString();
            pieceModel.ObjectPrefab = info.prefab.transform;
        }

        public void Choose()
        {
            transform.parent.parent.GetComponent<ThalassosResurrector>().Choose(pieceType);
        }
    }
}