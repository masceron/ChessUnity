using System;
using Data.UI.UIObject3D.Scripts;
using Game.Data.Pieces;
using Game.Piece;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UX.UI.Army.DesignArmy
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArmyDesignTroop: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] public UIObject3D model;
        [SerializeField] private Image image;
        [NonSerialized] public Transform Parent;
        private Transform oldParent;
        public bool set;
        public int rank = -1;
        public int file = -1;
        public PieceType type;

        public void Load(PieceObject piece)
        {
            model.ObjectPrefab = piece.prefab.transform;
            type = piece.type;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            oldParent = transform.parent;
            transform.SetParent(FindAnyObjectByType<ArmyDesign>().transform);
            image.raycastTarget = false;
            Parent = null;
            FindAnyObjectByType<ArmyDesignBoard>().SetAllowed();

            if (set) return;
            
            var searcher = FindAnyObjectByType<ArmySearcher>();
            var pool = searcher.Pool;
            var idx = pool.IndexOf(this);
            var obj = Instantiate(this, searcher.list);
            obj.transform.SetSiblingIndex(idx);
            obj.GetComponent<Image>().raycastTarget = true;
            pool[idx] = obj;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void Set(int r, int f)
        {
            rank = r;
            file = f;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!Parent)
            {
                if (!set)
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
                set = true;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            if (!set) return;
            
            Destroy(gameObject);
            FindAnyObjectByType<ArmyDesignBoard>().Remove(rank, file);
        }
    }
}