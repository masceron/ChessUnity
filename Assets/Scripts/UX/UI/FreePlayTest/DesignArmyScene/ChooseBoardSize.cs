using Game.Common;
using TMPro;
using System.Collections.Generic;
using UX.UI.Loader;
using Game.Relics;
using Game.Save.Army;
using Game.Save.Relics;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.InputSystem;
using Game.Managers;
using GameConfig = Game.Save.Stage.GameConfig;
using LineupConfig = Game.Save.Stage.LineupConfig;

namespace UX.UI.FreePlayTest
{
    public class ChooseBoardSize : MonoBehaviour
    {
        public List<Button> sizeButtons;
        public Button buttonPrefab;
        void Awake()
        {
            foreach (int size in FreePlayTest.Ins.boardSizes)
            {
                Button newButton = Instantiate(buttonPrefab, this.transform);
                newButton.gameObject.SetActive(true);
                newButton.transform.GetChild(0).GetComponent<TMP_Text>().text = $"{size}x{size}";
                newButton.onClick.AddListener(() => {
                    MatchManager.Ins.Init(new GameConfig(false, false, new Vector2Int(size, size), true));
                    FreePlayTest.Ins.ChangeBoardSize(size);
                });
            }
        }

    }
}