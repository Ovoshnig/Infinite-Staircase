using R3;
using System;
using VContainer.Unity;

public abstract class SceneButtonViewSceneSwitchMediator : IInitializable, IDisposable
{
    private readonly SceneButtonView _sceneButtonView;
    private readonly SceneSwitch _sceneSwitch;
    private readonly CompositeDisposable _compositeDisposable = new();

    public SceneButtonViewSceneSwitchMediator(SceneButtonView sceneButtonView, 
        SceneSwitch sceneSwitch)
    {
        _sceneButtonView = sceneButtonView;
        _sceneSwitch = sceneSwitch;
    }

    protected SceneSwitch SceneSwitch => _sceneSwitch;

    public void Initialize()
    {
        _sceneButtonView.Clicked
            .Skip(1)
            .Subscribe(_ => OnButtonClicked())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();

    protected abstract void OnButtonClicked();
}
