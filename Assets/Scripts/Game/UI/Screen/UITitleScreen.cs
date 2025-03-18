using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITitleScreen : BaseScreen
{
    [Header("Container")]
    [SerializeField] private GameObject holder;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private EventSystem eventSystem;

    private bool isTapped = false;

    public async override void Hide()
    {
        await UniTask.WaitForSeconds(0.5f);
        holder.SetActive(false);
    }

    public void OnTapToStart()
    {
        if (isTapped) return;

        isTapped = true;

        eventSystem.gameObject.SetActive(false);
        audioListener.enabled = false;

        MonoScene.Instance.LoadMainScene(Hide);
    }
}
