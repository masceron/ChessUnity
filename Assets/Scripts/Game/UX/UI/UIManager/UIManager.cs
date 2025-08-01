///
/// Create by linh soi - Abi Game studio
/// mentor Minh tito - CTO Abi Game studio
/// 
/// Manage list UI canvas for easy to use
/// Member nen inherit UI canvas
/// 
/// Update: 09-10-2020 
///             manage UI with Generic
///         09-10-2021 
///             Open, Close UI with Typeof(T)
///         28/11/2022
///             Close All UI
///             Close delay time
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    //dict for quick query UI prefab
    //dict dung de luu thong tin prefab canvas truy cap cho nhanh
    private Dictionary<UIID, UICanvas> uiCanvasPrefab = new Dictionary<UIID, UICanvas>();

    //list from resource
    //list load ui resource
    private UICanvas[] uiResources;

    //dict for UI active
    //dict luu cac ui dang dung
    private Dictionary<UIID, UICanvas> uiCanvas = new Dictionary<UIID, UICanvas>();

    //canvas container, it should be a canvas - root
    //canvas chua dung cac canvas con, nen la mot canvas - root de chua cac canvas nay
    public Transform CanvasParentTF;

    public enum UIID
    {
        MainMenu = 0,
        Win = 1,
        Lose = 2,
        GamePlay = 3,
        SkinShop = 4,
        WeaponShop = 5,
    }

    #region Canvas

    //open UI
    //mo UI canvas
    public T OpenUI<T>(UIID ID) where T : UICanvas
    {
        UICanvas canvas = GetUI<T>(ID);

        canvas.Setup();
        canvas.Open();

        return canvas as T;
    }

    //close UI directly
    //dong UI canvas ngay lap tuc
    public void CloseUI<T>(UIID ID) where T : UICanvas
    {
        if (IsOpened<T>(ID))
        {
            GetUI<T>(ID).CloseDirectly();
        }
    }   
    
    //close UI with delay time
    //dong ui canvas sau delay time
    public void CloseUI<T>(float delayTime, UIID ID) where T : UICanvas
    {
        if (IsOpened<T>(ID))
        {
            GetUI<T>(ID).Close(delayTime);
        }
    }

    //check UI is Opened
    //kiem tra UI dang duoc mo len hay khong
    public bool IsOpened<T>(UIID ID) where T : UICanvas
    {
        return IsLoaded<T>(ID) && uiCanvas[ID].gameObject.activeInHierarchy;
    }

    //check UI is loaded
    //kiem tra UI da duoc khoi tao hay chua
    public bool IsLoaded<T>(UIID ID) where T : UICanvas
    {
        UIID type = ID;
        return uiCanvas.ContainsKey(type) && uiCanvas[type] != null;
    }

    //Get component UI 
    //lay component cua UI hien tai
    public T GetUI<T>(UIID ID) where T : UICanvas
    {
        if (!IsLoaded<T>(ID))
        {
            UICanvas canvas = Instantiate(GetUIPrefab<T>(ID), CanvasParentTF);
            uiCanvas[ID] = canvas;
        }

        return uiCanvas[ID] as T;
    }

    //Close all UI
    //dong tat ca UI ngay lap tuc -> tranh truong hop dang mo UI nao dong ma bi chen 2 UI cung mot luc
    public void CloseAll()
    {
        foreach (var item in uiCanvas)
        {
            if (item.Value != null && item.Value.gameObject.activeInHierarchy)
            {
                item.Value.CloseDirectly();
            }
        }
    }

    //Get prefab from resource
    //lay prefab tu Resources/UI 
    private T GetUIPrefab<T>(UIID ID) where T : UICanvas
    {
        if (!uiCanvasPrefab.ContainsKey(ID))
        {
            if (uiResources == null)
            {
                uiResources = Resources.LoadAll<UICanvas>("UI/");
            }

            for (int i = 0; i < uiResources.Length; i++)
            {
                if (uiResources[i] is T)
                {
                    uiCanvasPrefab[ID] = uiResources[i];
                    break;
                }
            }
        }

        return uiCanvasPrefab[ID] as T;
    }


    #endregion

    #region Back Button

    private Dictionary<UICanvas, UnityAction> BackActionEvents = new Dictionary<UICanvas, UnityAction>();
    private List<UICanvas> backCanvas = new List<UICanvas>();
    UICanvas BackTopUI {
        get
        {
            UICanvas canvas = null;
            if (backCanvas.Count > 0)
            {
                canvas = backCanvas[backCanvas.Count - 1];
            }

            return canvas;
        }
    }


    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Escape) && BackTopUI != null)
        {
            BackActionEvents[BackTopUI]?.Invoke();
        }
    }

    public void PushBackAction(UICanvas canvas, UnityAction action)
    {
        if (!BackActionEvents.ContainsKey(canvas))
        {
            BackActionEvents.Add(canvas, action);
        }
    }

    public void AddBackUI(UICanvas canvas)
    {
        if (!backCanvas.Contains(canvas))
        {
            backCanvas.Add(canvas);
        }
    }

    public void RemoveBackUI(UICanvas canvas)
    {
        backCanvas.Remove(canvas);
    }

    /// <summary>
    /// CLear backey when comeback index UI canvas
    /// </summary>
    public void ClearBackKey()
    {
        backCanvas.Clear();
    }

    #endregion
}
