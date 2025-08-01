using Game.Board.Piece;
using Simple_Tooltip.Assets.Scripts;
using UI.UIObject3D.Scripts;
using UnityEngine;
using static Game.Board.General.MatchManager;

namespace Game.UX.UI
{
    public class CapturedUI: MonoBehaviour
    {
        public PieceConfig PieceInfo;

        public void Load(PieceConfig config)
        {
            PieceInfo = config;
            var info = assetManager.PieceData[config.Type];
            transform.GetChild(0).GetComponent<UIObject3D>().ObjectPrefab = info.prefab.transform;
            transform.GetComponent<SimpleTooltip>().infoLeft = "`" + (info.pieceName == "" ? config.Type.ToString() : info.pieceName);
        }
    }
}