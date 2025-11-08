
using Game.Common;
using TMPro;
using System.Collections.Generic;
using UX.UI.Loader;
using Game.Piece;
using UX.UI.Army.DesignArmy;
using Game.Relics;
using Game.Save.Army;
using Game.Save.Relics;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Managers;
using LineupConfig = Game.Save.Stage.LineupConfig;
using Game.ScriptableObjects.Collections;
using Game.Common;
using System.Linq;
using Game.ScriptableObjects;
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
        void Start()
        {
            SavedArmies.Ins.Load();
        }
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
        public void ToRegionalEffectPanel()
        {
            UIManager.Ins.Load(CanvasID.RegionalEffect);
        }
    }
}