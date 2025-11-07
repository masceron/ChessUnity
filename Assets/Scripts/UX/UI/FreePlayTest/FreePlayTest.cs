
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
        int boardSizeIndex = 0;
        protected override void Awake()
        {
            base.Awake();
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
        public void ToGameScene()
        {
            Config.boardSize = ArmyDesign.Ins.army.BoardSize;
            Config.PieceConfigWhite.Clear();
            Config.PieceConfigBlack.Clear();
            foreach (Troop troop in ArmyDesignBoard.Ins.Troops)
            {
                var augNameLst = troop.equippedAugmentation.Values.ToList();
                List<AugmentationInfo> infos = new();
                foreach (var name in augNameLst)
                {
                    infos.Add(AssetManager.Ins.AugmentationData[name]);
                }
                PieceConfig pieceConfig = new PieceConfig(troop.PieceType, troop.Side,
                    (ushort)(troop.Rank*Config.boardSize + troop.File), null);
                Debug.Log($"{BoardUtils.IndexOf(troop.Rank, troop.File)}, {troop.Rank}, {troop.File}");
                if (pieceConfig.Color == false)
                {
                    Config.PieceConfigWhite.Add(pieceConfig);
                }
                else
                {
                    Config.PieceConfigBlack.Add(pieceConfig);
                }
            }
            
            Debug.Log($"BoardSize: {ArmyDesign.Ins.army.BoardSize}");
            SceneLoader.LoadSceneWithLoadingScreen(1);
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