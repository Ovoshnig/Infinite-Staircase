using UnityEngine;

public class GameQuitButtonView : ButtonView
{
    protected override void Start()
    {
        base.Start();

        ButtonClicked += OnButtonClicked;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        ButtonClicked -= OnButtonClicked;
    }

    private void OnButtonClicked() => Application.Quit();
}
