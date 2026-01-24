
using Game.Common;
using TMPro;
using System.Collections.Generic;
using Game.Piece;
using UX.UI.Army.DesignArmy;
using UnityEngine;
using Game.ScriptableObjects.Collections;
using UX.UI.Followers;

namespace UX.UI.FreePlayTest{
    public enum FreePlayScene
    {
        DesignArmy,
        Augmentation,
        RegionalEffect,
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
        public void AddRegionalEffect(RegionalsData regionalsData)
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