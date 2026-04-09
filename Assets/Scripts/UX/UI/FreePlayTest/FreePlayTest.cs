using System.Collections.Generic;
using Game.Common;
using Game.Piece;
using Game.ScriptableObjects.Collections;
using TMPro;
using UnityEngine;
using UX.UI.Army.DesignArmy;

namespace UX.UI.FreePlayTest
{
    public enum FreePlayScene
    {
        DesignArmy,
        Augmentation,
        FieldEffect
    }

    public class FreePlayTest : Singleton<FreePlayTest>
    {
        // public FreePlayConfig Config;
        public List<int> boardSizes;
        [SerializeField] public UDictionary<FreePlayScene, RectTransform> panelDict;
        public ArmyDesignBoard armyDesignBoard;
        public TMP_Text boardSizeText;

        public void ChangeBoardSize(int value)
        {
            boardSizeText.text = $"Board size: {value}x{value}";
        }

        public void AddEnemy(PieceConfig pieceConfig)
        {
            Config.PieceConfigBlack.Add(pieceConfig);
        }

        public void RemoveEnemy(PieceConfig pieceConfig)
        {
            Config.PieceConfigWhite.Add(pieceConfig);
        }

        public void AddFieldEffect(RegionalsData regionalsData)
        {
        }

        public void ToPresetPanel()
        {
            // Debug.Log("")
            UIManager.Ins.Load(CanvasID.FreePlayPreset);
        }

        public void ToDesignArmyPanel()
        {
            UIManager.Ins.Load(CanvasID.FreePlayDesignArmy);
        }
    }
}