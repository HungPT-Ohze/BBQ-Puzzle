using com.homemade.pattern.singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("Canvas")]
    [SerializeField] private Canvas screenCanvas;
    [SerializeField] private Canvas popupCanvas;
    [SerializeField] private GameObject popupBG;

    [Header("Extra")]
    //[SerializeField] private UILoadingScreen loading;
    //[SerializeField] private UIToastScreen toast;

    private Dictionary<string, BaseScreen> screens = new Dictionary<string, BaseScreen>();
    private Dictionary<string, BasePopup> popups = new Dictionary<string, BasePopup>();

    private BaseScreen currentScreen;

    public TScreen ShowScreen<TScreen>() where TScreen : BaseScreen
    {
        string nameScreen = typeof(TScreen).Name;
        if (!screens.ContainsKey(nameScreen))
        {
            CreateScreen<TScreen>();
        }

        // Close all screen and popup
        if (currentScreen != null)
        {
            currentScreen.Hide();
        }
        CloseAllPopup();

        currentScreen = GetScreen<TScreen>();
        currentScreen.transform.SetAsLastSibling();
        currentScreen.Show();

        return (TScreen)currentScreen;
    }

    private void CreateScreen<TScreen>() where TScreen : BaseScreen
    {
        string nameScreen = typeof(TScreen).Name;
        string screenPath = $"UI/Screen/{nameScreen}";
        GameObject screenObj = Instantiate(ResourceManager.Instance.LoadGameObject(screenPath), screenCanvas.transform);
        TScreen script = screenObj.GetComponent<TScreen>();
        screens.Add(nameScreen, script);

        script.Init();
    }

    private TScreen GetScreen<TScreen>() where TScreen : BaseScreen
    {
        string nameScreen = typeof(TScreen).Name;
        var sc = screens[nameScreen].GetComponent<TScreen>();
        sc.transform.SetAsLastSibling();
        return sc;
    }

    public TScreen GetActiveScreen<TScreen>() where TScreen : BaseScreen
    {
        string nameScreen = typeof(TScreen).Name;
        var sc = screens[nameScreen].GetComponent<TScreen>();
        if (sc.gameObject.activeSelf)
        {
            return sc;
        }
        else
        {
            return null;
        }
    }

    public void DeleteScreen<TScreen>() where TScreen : BaseScreen
    {
        string nameScreen = typeof(TScreen).Name;
        if (screens.ContainsKey(nameScreen))
        {
            screens.Remove(nameScreen);
        }
        else
        {
            Debug.Log($"{nameScreen} is not exsits");
        }
    }

    private TPopup CreatePopup<TPopup>() where TPopup : BasePopup
    {
        string popupName = typeof(TPopup).Name;
        string popupPath = $"UI/Popup/{popupName}";
        GameObject popupObj = Instantiate(ResourceManager.Instance.LoadGameObject(popupPath), popupCanvas.transform);
        TPopup popup = popupObj.GetComponent<TPopup>();
        popups.Add(popupName, popup);

        return popup;
    }

    public void ShowPopup<TPopup>(object obj = null) where TPopup : BasePopup
    {
        popupBG.SetActive(true);
        //currentPopup?.DeActive();

        string popupName = typeof(TPopup).Name;
        BasePopup popup = null;

        if (!popups.ContainsKey(popupName))
        {
            popup = CreatePopup<TPopup>();
        }
        else
        {
            popup = popups[popupName];
        }

        popup.transform.SetAsLastSibling();
        popup.Open(obj);
    }

    public TPopup GetPopup<TPopup>() where TPopup : BasePopup
    {
        string popupName = typeof(TPopup).Name;

        if (popups.ContainsKey(popupName))
        {
            return (TPopup)popups[popupName];
        }

        return null;
    }

    public void ClosePopup<TPopup>() where TPopup : BasePopup
    {
        string popupName = typeof(TPopup).Name;

        if (!popups.ContainsKey(popupName)) return;

        popups[popupName].Close();
    }

    public void CloseAllPopup()
    {
        if (popups.Count == 0) return;

        foreach (var p in popups)
        {
            p.Value.DeActive();
        }

        HidePopupBG();
    }

    public void HidePopupBG()
    {
        if (!popups.Any(p => p.Value.isActiveAndEnabled))
        {
            popupBG.SetActive(false);
        }
    }

    public bool CheckPopupShowing()
    {
        return popups.Any(p => p.Value.isActiveAndEnabled);
    }

    public void CloseCurrentPopup()
    {
        Transform lastPopupTrans = popupCanvas.transform.GetChild(popupCanvas.transform.childCount - 1);
        if (lastPopupTrans != null)
        {
            lastPopupTrans.GetComponent<BasePopup>().Close();

            MoveLastSiblingPopup(lastPopupTrans);
        }
    }

    public void MoveLastSiblingPopup(Transform popup)
    {
        popup.SetSiblingIndex(1);
        popupBG.transform.SetAsFirstSibling();
    }

    public void ShowToast(string message)
    {
        //toast.ShowToast(message);
        //Debug.Log(message);
    }

    public void ShowLoading(Action onLoad = null)
    {
        //loading.Loading(onLoad);
    }

    public void HideLoading()
    {
        //loading.HideLoading();
    }

}
