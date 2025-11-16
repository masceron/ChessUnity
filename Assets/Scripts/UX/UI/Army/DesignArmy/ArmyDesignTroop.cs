using System;
using Game.ScriptableObjects;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UX.UI.Tooltip;

namespace UX.UI.Army.DesignArmy
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArmyDesignTroop: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] public UIObject3D model;
        [SerializeField] private Image image;
        [SerializeField] private TooltipTrigger trigger;
        [SerializeField] private Image greyMask;
        [NonSerialized] public Transform Parent;
        private Transform oldParent;
        public bool Placed;
        [NonSerialized] public bool isGreyOut = false;
        [NonSerialized] public int Rank = -1;
        [NonSerialized] public int File = -1;
        public PieceInfo Piece;
        
        public void Load(PieceInfo piece, bool isGreyOut = false)
        {
            if (piece == null)
            {
                Debug.LogError("ArmyDesignTroop::Load(piece) : piece is null");
            }
            this.isGreyOut = isGreyOut;
            Piece = piece;
            if (Piece.prefab == null)
            {
                Debug.LogError($"Piece : {Piece.name} has null prefab");
            }
            else
            {
                model.ObjectPrefab = Piece.prefab.transform;
            }
            SetTooltip();
            if (isGreyOut)
            {
                greyMask.color = UnityEngine.Color.black;

            }
            else
            {
                greyMask.color = UnityEngine.Color.yellow;
            }
        }

        private void SetTooltip()
        {
            var pieceName = Localizer.GetText("piece_name", Piece.key, null);
            var pieceDescriptions = "";
            if (Piece.hasSkill)
            {
                pieceDescriptions += Localizer.GetText("piece_skill", Piece.key + "_skill", null) + ": " +
                                     Localizer.GetText("piece_skill_description", Piece.key + "_skill_description", null);
            }

            trigger.SetText(pieceName, "", pieceDescriptions);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isGreyOut){ return; }
            oldParent = transform.parent;
            transform.SetParent(FindAnyObjectByType<Canvas>().transform);
            image.raycastTarget = false;
            Parent = null;
            FindAnyObjectByType<ArmyDesignBoard>().SetAllowed(Piece.rank == Game.Piece.PieceRank.Construct);
            trigger.enabled = false;
            TooltipManager.Ins.Disable();

            if (Placed) return;
            
            var searcher = FindAnyObjectByType<ArmySearcher>();
            var pool = searcher.Pool;
            var idx = pool.IndexOf(this);
            ArmyDesignTroop obj = Instantiate(this, searcher.list);
            
            obj.transform.SetSiblingIndex(idx);
            obj.GetComponent<Image>().raycastTarget = true;
            obj.trigger.enabled = true;
            
            obj.Load(Piece);
            pool[idx] = obj;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isGreyOut){ return; }
            transform.position = eventData.position;
        }

        public void Set(int r, int f)
        {
            Rank = r;
            File = f;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (isGreyOut){ return; }
            FindAnyObjectByType<ArmyDesignBoard>().UnSet();
            trigger.enabled = true;
            TooltipManager.Ins.Enable();
            if (!Parent)
            {
                if (!Placed)
                {
                    Destroy(gameObject);
                }
                else
                {
                    transform.SetParent(oldParent);
                    image.raycastTarget = true;
                }
            }
            else
            {
                transform.SetParent(Parent);

                var size = Parent.transform.GetComponent<GridLayoutGroup>().cellSize;
                GetComponent<RectTransform>().sizeDelta = size;
                
                image.raycastTarget = true;
                Placed = true;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            if (!Placed) return;
            
            Destroy(gameObject);
            FindAnyObjectByType<ArmyDesignBoard>().Remove(Rank, File);
        }
    }
}