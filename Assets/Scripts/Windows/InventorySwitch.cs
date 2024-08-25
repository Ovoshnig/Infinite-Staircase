public class InventorySwitch : Window
{
    protected override void InitializeInput() => 
        PlayerInput.Inventory.Switch.performed += OnSwitchPerformed;
}