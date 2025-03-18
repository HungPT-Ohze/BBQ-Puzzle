using System.Collections;
using UnityEngine;

public abstract class BasePopup : MonoBehaviour, IPopup
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private bool useAnimator;

    [Header("Content")]
    [SerializeField] protected GameObject content;

#if UNITY_EDITOR
    public virtual void OnValidate()
    {
        if (TryGetComponent<Animator>(out Animator ani))
        {
            this.animator = ani;
        }
        else
        {
            animator = gameObject.AddComponent<Animator>();
        }
    }
#endif

    public virtual void Open(object obj = null)
    {
        gameObject.SetActive(true);

        animator.enabled = useAnimator;
    }

    public virtual void Close()
    {
        StartCoroutine(CloseCoroutine());
    }

    private IEnumerator CloseCoroutine()
    {
        float timeClose = 0f;

        if (useAnimator)
        {
            animator.SetTrigger("Close");
            timeClose = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        }

        yield return new WaitForSeconds(timeClose);
        gameObject.SetActive(false);

        UIManager.Instance.MoveLastSiblingPopup(this.transform);
        UIManager.Instance.HidePopupBG();
    }

    public void DeActive()
    {
        gameObject.SetActive(false);
    }

    public void DestroyPopup()
    {
        Destroy(gameObject);
    }

    public virtual void ShowHideContent(bool isShow)
    {
        content?.SetActive(isShow);
    }
}
