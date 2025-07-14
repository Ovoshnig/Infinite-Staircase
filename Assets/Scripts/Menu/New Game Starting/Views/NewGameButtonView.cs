using UnityEngine;

public class NewGameButtonView : ButtonView
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _resetWarningPanel;
    [SerializeField] private GameObject _gameCreationPanel;

    public void SetActiveResetWarningPanel(bool value) => _resetWarningPanel.SetActive(value);

    public void SetActiveGameCreationPanel(bool value) => _gameCreationPanel.SetActive(value);

    public void SetActiveMenuPanel(bool value) => _menuPanel.SetActive(value);
}
