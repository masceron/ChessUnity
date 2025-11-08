using System;
using Game.Augmentation;
using Game.ScriptableObjects;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UX.UI.FreePlayTest;
using UX.UI.Tooltip;

namespace UX.UI.Army.FreePlayTest
{
    public class EnemyRelicUI : MonoBehaviour
    {
        public TMP_Text tmp;
        void Awake()
        {
            tmp.text = Config.relicBlackConfig.Type.ToString();
        }
    }
}