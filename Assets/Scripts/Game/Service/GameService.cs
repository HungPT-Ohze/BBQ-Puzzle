using com.homemade.save.core;
using Cysharp.Threading.Tasks;
using com.homemade.core;

public class GameService : Container<GameService>
{
    private ISaveService saveService;

    private void AddService()
    {
        // Save
        saveService = new SaveService();
        RegisterService(saveService);
    }

    protected override UniTask OnPreInitialize()
    {
        AddService();

        return base.OnPreInitialize();
    }

    protected override async UniTask OnInitialize()
    {
        await base.OnInitialize();
    }

    protected override UniTask OnPostInitialize()
    {
        return base.OnPostInitialize();
    }
}

