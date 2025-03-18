using System;
using UnityEngine;

[Serializable]
public class UserSave : BaseDataSave
{
    public string userId;
    public string deviceId;

    public bool isNewUser;

    public override void Init()
    {
        base.Init();

        userId = Guid.NewGuid().ToString();

        deviceId = SystemInfo.deviceUniqueIdentifier;

        isNewUser = true;
    }

    public override void Clear()
    {
        base.Clear();

        userId = string.Empty;
        deviceId = string.Empty;
        isNewUser = false;
    }

    public void FirstTimePlayGame()
    {
        isNewUser = false;
        Save();
    }
}
