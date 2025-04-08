using System;
using UnityEngine;
using VContainer.Unity;

[RequireComponent(typeof(SceneButtonView))]
public abstract class SceneButtonViewSceneSwitchMediator : IInitializable, IDisposable
{
    private readonly SceneButtonView _sceneButtonView;
    private readonly SceneSwitch _sceneSwitch;

    public SceneButtonViewSceneSwitchMediator(SceneButtonView sceneButtonView, 
        SceneSwitch sceneSwitch)
    {
        _sceneButtonView = sceneButtonView;
        _sceneSwitch = sceneSwitch;
    }

    protected SceneSwitch SceneSwitch => _sceneSwitch;

    public void Initialize() => _sceneButtonView.ButtonClicked += OnButtonClicked;

    public void Dispose() => _sceneButtonView.ButtonClicked -= OnButtonClicked;

    protected abstract void OnButtonClicked();
}
