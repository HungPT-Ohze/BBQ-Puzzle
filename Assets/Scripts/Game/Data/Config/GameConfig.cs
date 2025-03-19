using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game", order = 1)]
public class GameConfig : ScriptableObject
{
    [FoldoutGroup("Level")]
    [VerticalGroup("Level/Element")] public int LevelCount = 1;
}

