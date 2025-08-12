using static Game.Common.BoardUtils;
using System.Collections.Generic;
using System.Collections.Specialized;
using Game.Data.Pieces;
using UnityEngine;

namespace UX.UI.Ingame
{
    public class CapturedMenu: MonoBehaviour
    {
        [SerializeField] private RectTransform allyCaptured;
        [SerializeField] private RectTransform enemyCaptured;
        [SerializeField] private GameObject capturedUI;
        
        private readonly List<GameObject> allyCapturedList = new();
        private readonly List<GameObject> enemyCapturedList = new();

        private void Start()
        {
            WhiteCaptured().CollectionChanged += ReloadCaptureList;
            BlackCaptured().CollectionChanged += ReloadCaptureList;
        }

        private void ReloadCaptureList(object o, NotifyCollectionChangedEventArgs e)
        {
            var color = OurSide();
            var collection = o == WhiteCaptured() ? 
                !color ? allyCapturedList : enemyCapturedList :
                color ? allyCapturedList : enemyCapturedList;

            var obj = collection == allyCapturedList ? allyCaptured : enemyCaptured;

            if (e.OldItems != null)
            {
                var removed = collection[e.OldStartingIndex];
                collection.Remove(removed);
                Destroy(removed);
            }
            else
            {
                var newObj = Instantiate(capturedUI, obj);
                newObj.GetComponent<CapturedUI>().Load((PieceConfig)e.NewItems[0]);
                collection.Add(newObj);
            }
        }
    }
}