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
using UX.UI.Followers;

namespace UX.UI.FreePlayTest
{
    public class AugmentationManagerUI : Singleton<AugmentationManagerUI>
    {
        public TMP_Text sideText, nameText;
        public Troop SelectedTroop;
        public TroopDescriptions troopDescriptions;
        public List<AugmentationIcon> icons = new();
        [HideInInspector] public Dictionary<AugmentationSlot, AugmentationName> equippedAugmentation = new();
        public void Load(Troop selected)
        {
            this.SelectedTroop = selected;
            equippedAugmentation = SelectedTroop.equippedAugmentation;
            foreach(var icon in icons)
            {
                if (equippedAugmentation.ContainsKey(icon.slot))
                {
                    icon.Load(equippedAugmentation[icon.slot]);
                }
                else
                {
                    icon.Load(AugmentationName.None);
                }
            }
            sideText.text = (SelectedTroop.Side) ? "Enemy" : "Ally";
            nameText.text = AssetManager.Ins.PieceData[selected.PieceType].key;
            troopDescriptions.Display(AssetManager.Ins.PieceData[selected.PieceType]);
        }

        public void AddAugmentation(AugmentationInfo aug)
        {
            equippedAugmentation[aug.Slot] = aug.Name;
        }
        public void RemoveAugmentation(AugmentationSlot slot)
        {
            equippedAugmentation[slot] = AugmentationName.None;
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