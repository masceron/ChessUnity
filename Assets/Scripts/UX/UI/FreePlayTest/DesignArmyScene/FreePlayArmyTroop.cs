using System;
using Game.Augmentation;
using Game.ScriptableObjects;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UX.UI.FreePlayTest;
using UX.UI.Tooltip;

namespace UX.UI.Army.DesignArmy
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FreePlayArmyTroop: ArmyDesignTroop, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public bool Removable = true;
        public new void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right
                && UIManager.Ins.GetCanvasID() == CanvasID.FreePlayDesignArmy)
            {
                Debug.Log("Go to Augmentation");
                UIManager.Ins.Load(CanvasID.Augmentation);
                var troopSide = ArmyDesign.Ins.board.GetComponent<FreePlayArmyBoard>().GetTroopByCoordinate(Rank, File);
                AugmentationManagerUI.Ins.Load(troopSide.troop, troopSide.side);

                return;
            }
            else if (!Placed) return;
            else if (!Removable) return;
            Destroy(gameObject);
            FindAnyObjectByType<ArmyDesignBoard>().Remove(Rank, File);
        }
    }
}