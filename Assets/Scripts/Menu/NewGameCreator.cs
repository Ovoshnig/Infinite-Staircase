using Cysharp.Threading.Tasks;
using Random = System.Random;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System;
using TMPro;

public class NewGameCreator : MonoBehaviour
{
    private const string SeedKey = "Seed";

    [SerializeField] private Button _closeCreateGamePanelButton;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TMP_InputField _seedInputField;

    private readonly Random _random = new();
    private SceneSwitch _sceneSwitch;
    private SaveSaver _saveSaver;

    [Inject]
    private void Construct(SceneSwitch sceneSwitch, SaveSaver saveSaver)
    {
        _sceneSwitch = sceneSwitch;
        _saveSaver = saveSaver;
    }

    private void OnEnable()
    {
        _closeCreateGamePanelButton.onClick.AddListener(CloseGameCreationPanel);
        _startGameButton.onClick.AddListener(StartNewGame);
        _seedInputField.onValueChanged.AddListener(OnSeedInputFieldValueChanged);
    }

    private void OnDisable()
    {
        _closeCreateGamePanelButton.onClick.RemoveListener(CloseGameCreationPanel);
        _startGameButton.onClick.RemoveListener(StartNewGame);
        _seedInputField.onValueChanged.RemoveListener(OnSeedInputFieldValueChanged);
    }

    private void CloseGameCreationPanel() => gameObject.SetActive(false);

    private void OnSeedInputFieldValueChanged(string input)
    {
        if (int.TryParse(input, out int value))
        {
            if (value < 0)
                _seedInputField.text = "0";
        }
        else
        {
            if (long.TryParse(input, out long longValue))
                _seedInputField.text = int.MaxValue.ToString();

            _seedInputField.text = string.Empty;
        }
    }

    private void StartNewGame()
    {
        int seed;

        if (_seedInputField.text == string.Empty)
            seed = _random.Next();
        else 
            seed = Convert.ToInt32(_seedInputField.text); 

        _saveSaver.SaveData(SeedKey, seed);
        _sceneSwitch.LoadFirstLevel().Forget();
    }
}
