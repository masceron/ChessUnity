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
using UX.UI.Army.DesignArmy;
using UnityEngine.EventSystems;

namespace UI.UX.FreePlayTest
{
    public class AllyRelicUI : ArmyDesignRelic
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            Config.relicWhiteConfig = new RelicConfig(Relic.type, false, 5);
        }
    }
}