using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.ScriptableObjects;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace UX.UI.Ingame
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceInfoMenu: MonoBehaviour
    {
        [SerializeField] private GameObject pieceInfo;
        [SerializeField] private UIObject3D pieceImage;
        [SerializeField] private RawImage pieceDemonstration;
        [SerializeField] private TMP_Text pieceName;
        [SerializeField] private TMP_Text posText;
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
            pieceName.text = AssetManager.Ins.PieceData[piece.Type].name;
            posText.text = $"Rank: {BoardUtils.RankOf(piece.Pos)}, File: {BoardUtils.FileOf(piece.Pos)}, Index: {piece.Pos}";
        }
        
        private void LoadPieceModel(PieceInfo info)
        {
            pieceImage.ObjectPrefab = info.prefab.transform;
        }
        
        private void LoadPieceDemonstrations(PieceInfo info)
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