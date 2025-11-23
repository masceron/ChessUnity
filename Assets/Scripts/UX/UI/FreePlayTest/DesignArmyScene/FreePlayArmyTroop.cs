using UnityEngine;
using UnityEngine.EventSystems;
using UX.UI.Army.DesignArmy;
using UX.UI.FreePlayTest.AugmentationScene;
using UX.UI.Tooltip;
using UnityEngine.UI;

namespace UX.UI.FreePlayTest.DesignArmyScene
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
                var troopSide = FreePlayArmyDesign.Ins.board.GetComponent<FreePlayArmyBoard>().GetTroopByCoordinate(Rank, File);
                AugmentationManagerUI.Ins.Load(troopSide.troop, troopSide.side);

                return;
            }
            else if (!Placed) return;
            else if (!Removable) return;
            Destroy(gameObject);
            FindAnyObjectByType<FreePlayArmyBoard>().Remove(Rank, File);
        }
        public new void OnBeginDrag(PointerEventData eventData)
        {
            if (isGreyOut){ return; }
            oldParent = transform.parent;
            transform.SetParent(FindAnyObjectByType<Canvas>().transform);
            image.raycastTarget = false;
            Parent = null;
            FindAnyObjectByType<FreePlayArmyBoard>().SetAllowed(Piece.rank == Game.Piece.PieceRank.Construct);
            trigger.enabled = false;
            TooltipManager.Ins.Disable();

            if (Placed) return;
            
            var searcher = FindAnyObjectByType<FPArmySearcher>();
            var pool = searcher.Pool;
            var idx = pool.IndexOf(this);
            var obj = Instantiate(this, searcher.list);
            
            obj.transform.SetSiblingIndex(idx);
            obj.GetComponent<Image>().raycastTarget = true;
            obj.trigger.enabled = true;
            
            obj.Load(Piece);
            pool[idx] = obj;
        }

    }
}