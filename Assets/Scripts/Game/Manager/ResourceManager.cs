using com.homemade.pattern.singleton;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<string, GameObject> dicGameObject = new Dictionary<string, GameObject>();

    private Dictionary<string, Sprite> dicSp = new Dictionary<string, Sprite>();
    private Dictionary<string, SpriteAtlas> dicAtlas = new Dictionary<string, SpriteAtlas>();
    private Dictionary<string, TMP_SpriteAsset> dicSpriteAssets = new Dictionary<string, TMP_SpriteAsset>();


    public void Clear()
    {
        dicGameObject.Clear();
        dicSp.Clear();
        dicAtlas.Clear();
        dicSpriteAssets.Clear();
    }

    private T Load<T>(string path, Dictionary<string, T> dic, bool save = true, bool check = true) where T : Object
    {
        if (save)
        {
            if (!dic.ContainsKey(path))
            {
                T t = Resources.Load<T>(path);

                if (t) dic.Add(path, t);
                else if (check) Debug.LogError("Resource Null: " + path);
            }

            if (dic.ContainsKey(path)) return dic[path];
            return default;
        }
        else
        {
            return Resources.Load<T>(path);
        }
    }

    public Sprite LoadSprite(string path, bool save = true)
    {
        return Load($"Textures/{path}", dicSp, save);
    }

    public SpriteAtlas LoadSpriteAtlas(string path, bool save = true)
    {
        return Load($"Atlas/{path}", dicAtlas, save);
    }

    public TMP_SpriteAsset LoadSpriteAsset(string path, bool save = true)
    {
        return (TMP_SpriteAsset)Load($"Sprite Assets/{path}", dicSpriteAssets, save);
    }

    public Sprite LoadSprite(string atlas, string name, bool save = true)
    {
        var rs = LoadSpriteAtlas(atlas);

        if (save)
        {
            string pathSp = $"{atlas}/{name}";

            if (!dicSp.ContainsKey(pathSp))
            {
                Sprite t = rs.GetSprite(name);
                if (t) dicSp.Add(pathSp, t);
            }

            if (dicSp.ContainsKey(pathSp)) return dicSp[pathSp];
            return default;
        }
        else
        {
            return rs.GetSprite(name);
        }
    }

    public GameObject LoadGameObject(string path, bool save = true)
    {
        return Load(path, dicGameObject, save);
    }

}
