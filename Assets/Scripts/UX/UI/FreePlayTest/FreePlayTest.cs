
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

namespace UX.UI.FreePlayTest{
    public enum FreePlayScene
    {
        DesignArmy,
        Augmentation,
        RegionalEffect,
        
    }
    public class FreePlayTest : Singleton<FreePlayTest>
    {
        public FreePlayConfig config;
        public List<int> boardSizes;
        [SerializeField] public UDictionary<FreePlayScene, RectTransform> panelDict;
        public ArmyDesignBoard armyDesignBoard;
        public TMP_Text boardSizeText;
        int boardSizeIndex = 0;
        void Awake()
        {

        }
        public void ChangeBoardSize(int value)
        {
            boardSizeText.text = $"Board size: {value}x{value}";
        }

        public void AddEnemy(PieceConfig pieceConfig)
        {
            config.PieceConfigBlack.Add(pieceConfig);
        }
        public void RemoveEnemy(PieceConfig pieceConfig)
        {
            config.PieceConfigWhite.Add(pieceConfig);
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



        // public void ToNextPanel()
        // {
        //     if (panelIndex == panelContainer.Count - 1)
        //     {
        //         MatchManager.Ins.isChoosingPiece = false;
        //         // Load UI cho gameplay và SetActive(false) cho UI FreePlayTest
        //         UIManager.Ins.Load(CanvasID.Ingame);
        //         MatchManager.Ins.StartGame(freePlayConfig);
        //     }
        //     else
        //     {
        //         SetActivePanelIndex(panelIndex + 1);
        //         nextButton.GetChild(0).GetComponent<TMP_Text>().text = (panelIndex == panelContainer.Count - 1) ? "Play" : "Next";
        //     }

        // }
        // public void ToPreviousPanel()
        // {
        //     if (panelIndex == 0)
        //     {
        //         //Trở về màn hình chính
        //         SceneLoader.LoadSceneWithLoadingScreen(0);
        //     }
        //     else
        //     {
        //         SetActivePanelIndex(panelIndex - 1);
        //     }
        // }
        // void SetActivePanelIndex(int panelIndex)
        // {
        //     this.panelIndex = panelIndex;
        //     for(int i = 0; i < panelContainer.Count; i++)
        //     {
        //         Debug.Log($"element's name: {panelContainer[i].name}");
        //         panelContainer[i].gameObject.SetActive(i == panelIndex);
        //     }
        // }
    }
}