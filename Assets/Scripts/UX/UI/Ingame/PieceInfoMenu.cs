using Data.UI.UIObject3D.Scripts;
using Game.Data.Pieces;
using Game.Managers;
using Game.Piece.PieceLogic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace UX.UI.Ingame
{
    public class PieceInfoMenu: MonoBehaviour
    {
        [SerializeField] private GameObject pieceInfo;
        [SerializeField] private UIObject3D pieceImage;
        [SerializeField] private RawImage pieceDemonstration;
        
        private bool seeingCapture;

        private void Start()
        {
            pieceInfo.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void SetUpPieceInfo(PieceLogic piece)
        {
            pieceInfo.SetActive(true);
            var pieceInformation = AssetManager.Ins.PieceData[piece.Type];
            LoadPieceModel(pieceInformation);
            LoadPieceDemonstrations(pieceInformation);
        }
        
        private void LoadPieceModel(PieceObject info)
        {
            pieceImage.ObjectPrefab = info.prefab.transform;
        }
        
        private void LoadPieceDemonstrations(PieceObject info)
        {
            if (!seeingCapture)
            {
                if (info.movePattern)
                {
                    pieceDemonstration.texture = info.movePattern;
                    pieceDemonstration.color = Color.white;
                    return;
                }
            }
            else
            {
                if (info.capturePattern)
                {
                    pieceDemonstration.texture = info.capturePattern;
                    pieceDemonstration.color = Color.white;
                    return;
                }
            }
            
            pieceDemonstration.texture = null;
            pieceDemonstration.color = new Color(0, 0, 0, 0);
        }
        
        public void ToggleDemonstrations(InputAction.CallbackContext context)
        {
            if (!context.performed || !pieceInfo.activeSelf) return;
            
            seeingCapture = !seeingCapture;
            LoadPieceDemonstrations(AssetManager.Ins.PieceData[BoardViewer.Hovering.Type]);
        }
    }
}