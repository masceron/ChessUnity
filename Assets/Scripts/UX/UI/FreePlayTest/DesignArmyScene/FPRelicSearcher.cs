using UX.UI.Army.DesignArmy;
using System.Collections.Generic;
using System.Linq;
using System;
using Game.Common;
using Game.Relics;
using Game.Save.Relics;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FPRelicSearcher: ArmyRelicSearcher
    {
        public override void SelectRelic()
        {
            FreePlayArmyDesign.Ins.SelectRelic(selecting);
            relicText.text = description.nameText.text;
            Toggle();
        }
    }
}