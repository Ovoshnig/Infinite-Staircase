using Cysharp.Threading.Tasks;
using Random = System.Random;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System;
using TMPro;

public class NewGameCreator : MonoBehaviour
{
    private const string SaveCreatedKey = "SaveCreated";
    private const string SeedKey = "Seed";

    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Button _closeGameCreationButton;
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
        _closeGameCreationButton.onClick.AddListener(OnCloseGameCreationButtonClicked);
        _startGameButton.onClick.AddListener(OnStartNewGameButtonClicked);

        _seedInputField.onValueChanged.AddListener(OnSeedInputFieldValueChanged);
    }

    private void OnDisable()
    {
        _closeGameCreationButton.onClick.RemoveListener(OnCloseGameCreationButtonClicked);
        _startGameButton.onClick.RemoveListener(OnStartNewGameButtonClicked);

        _seedInputField.onValueChanged.RemoveListener(OnSeedInputFieldValueChanged);
    }

    private void OnCloseGameCreationButtonClicked()
    {
        _menuPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnStartNewGameButtonClicked()
    {
        int seed;

        if (_seedInputField.text == string.Empty)
            seed = _random.Next();
        else 
            seed = Convert.ToInt32(_seedInputField.text); 

        _saveSaver.SaveData(SaveCreatedKey, true);
        _saveSaver.SaveData(SeedKey, seed);
        _sceneSwitch.LoadFirstLevel().Forget();
    }

    private void OnSeedInputFieldValueChanged(string input)
    {
        if (int.TryParse(input, out int value))
        {
            if (value < 0)
                _seedInputField.text = "0";
        }
        else
        {
            if (long.TryParse(input, out long _))
                _seedInputField.text = int.MaxValue.ToString();

            _seedInputField.text = string.Empty;
        }
    }
}
