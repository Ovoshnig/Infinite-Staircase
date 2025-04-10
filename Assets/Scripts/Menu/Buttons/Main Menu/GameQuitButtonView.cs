using R3;
using UnityEngine;

public class GameQuitButtonView : ButtonView
{
    protected override void Start()
    {
        base.Start();

        Clicked
            .Skip(1)
            .Subscribe(value => OnButtonClicked())
            .AddTo(this);
    }

    private void OnButtonClicked() => Application.Quit();
}
