using R3;

public class PauseMenuWindow : Window
{
    public PauseMenuWindow(WindowInputHandler windowInputHandler, WindowTracker windowTracker) 
        : base(windowInputHandler, windowTracker)
    {
    }

    protected override ReadOnlyReactiveProperty<bool> WindowSwitchPressed => 
        WindowInputHandler.PauseMenuSwitchPressed;
}
