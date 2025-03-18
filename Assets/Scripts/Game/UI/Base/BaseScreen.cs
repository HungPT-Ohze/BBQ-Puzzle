using UnityEngine;

public abstract class BaseScreen : MonoBehaviour, IScreen
{
    private bool isInit = false;

    public virtual void Init()
    {
        if (isInit) return;

        isInit = true;
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

}
