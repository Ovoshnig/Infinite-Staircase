using R3;

public class PauseMenuSwitch : WindowSwitch
{
    public PauseMenuSwitch(WindowInputHandler windowInputHandler, WindowTracker windowTracker) 
        : base(windowInputHandler, windowTracker)
    {
    }

    protected override ReadOnlyReactiveProperty<bool> WindowSwitchPressed => 
        WindowInputHandler.PauseMenuSwitchPressed;
}
