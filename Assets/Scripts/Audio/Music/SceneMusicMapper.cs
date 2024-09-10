using System.Collections.Generic;
using UnityEngine;

public class SceneMusicMapper : ISceneMusicMapper
{
    private readonly Dictionary<SceneSwitch.SceneType, MusicCategory> _sceneToMusicCategory = new()
    {
        { SceneSwitch.SceneType.MainMenu, MusicCategory.MainMenu },
        { SceneSwitch.SceneType.GameLevel, MusicCategory.GameLevel },
        { SceneSwitch.SceneType.Credits, MusicCategory.Credits }
    };

    public MusicCategory GetMusicCategory(SceneSwitch.SceneType sceneType)
    {
        if (_sceneToMusicCategory.TryGetValue(sceneType, out var category))
            return category;

        Debug.LogWarning($"No suitable music category for {sceneType} scene type");

        return MusicCategory.MainMenu;
    }
}