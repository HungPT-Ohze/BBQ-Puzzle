using com.homemade.pattern.singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoSingleton<MainGame>
{
    private void Start()
    {
        MonoScene.Instance.LoadGameScene(OnLoadGameDone);
    }

    private void OnLoadGameDone()
    {
        GamePlay.Instance.Setup();
    }
}
