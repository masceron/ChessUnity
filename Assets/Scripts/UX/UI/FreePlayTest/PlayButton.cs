
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

namespace UX.UI.FreePlayTest
{
    public class PlayButton : MonoBehaviour
    {
        public void OnClick()
        {
            if (ArmyDesign.Ins.TrySave())
            {
                SceneLoader.LoadSceneWithLoadingScreen(1);
            }

        }
    }
}