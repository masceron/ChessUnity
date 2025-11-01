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
        public new void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right
                && UIManager.Ins.GetCanvasID() == CanvasID.FreePlayDesignArmy)
            {
                Debug.Log("Go to Augmentation");
                UIManager.Ins.Load(CanvasID.Augmentation);
                AugmentationManagerUI.Ins.Load(ArmyDesignBoard.Ins.GetTroopByCoordinate(Rank, File));
                return;
            }
            else if (!Placed) return;

            Destroy(gameObject);
            FindAnyObjectByType<ArmyDesignBoard>().Remove(Rank, File);
        }
    }
}