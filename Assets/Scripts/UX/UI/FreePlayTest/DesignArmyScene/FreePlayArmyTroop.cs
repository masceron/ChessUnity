using UnityEngine;
using UnityEngine.EventSystems;
using UX.UI.FreePlayTest;

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
            FindAnyObjectByType<FreePlayArmyBoard>().Remove(Rank, File);
        }
    }
}