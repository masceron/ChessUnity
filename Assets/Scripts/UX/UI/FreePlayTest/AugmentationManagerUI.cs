using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Game.Save.Army;
using Game.Piece;
using Game.Augmentation;
using Game.ScriptableObjects;
using Game.Managers;
using UX.UI.Army.DesignArmy;
using Game.Common;

namespace UX.UI.FreePlayTest
{
    public class AugmentationManagerUI : Singleton<AugmentationManagerUI>
    {
        public TMP_Text sideText, nameText;
        public Troop SelectedTroop;
        public List<AugmentationIcon> icons = new();
        [HideInInspector] public Dictionary<AugmentationSlot, AugmentationName> equippedAugmentation = new();
        public void Load(Troop selected)
        {
            this.SelectedTroop = selected;
            sideText.text = (SelectedTroop.Side) ? "Enemy" : "Ally";
            nameText.text = AssetManager.Ins.PieceData[selected.PieceType].key;
        }

        public void AddAugmentation(AugmentationInfo aug)
        {
            SelectedTroop.equippedAugmentation[aug.Slot] = aug.Name;
        }
        public void RemoveAugmentation(AugmentationSlot slot)
        {
            SelectedTroop.equippedAugmentation[slot] = AugmentationName.None;
        }
        public void Save()
        {
            SelectedTroop.equippedAugmentation = equippedAugmentation;
        }
        public void ReturnToDesignArmy()
        {
            UIManager.Ins.Load(CanvasID.FreePlayDesignArmy);
        }
    }
} 