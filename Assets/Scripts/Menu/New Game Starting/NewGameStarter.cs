using System;
using Random = System.Random;

public class NewGameStarter
{
    private readonly SaveStorage _saveStorage;
    private readonly WorldGenerationSettings _generationSettings;
    private string _seedText = string.Empty;

    public event Action NewGameStarted;

    public NewGameStarter(SaveStorage saveStorage, WorldGenerationSettings generationSettings)
    {
        _saveStorage = saveStorage;
        _generationSettings = generationSettings;
    }

    public void SetSeedText(string seed) => _seedText = seed;

    public void StartGame()
    {
        int seed;

        if (int.TryParse(_seedText, out int value))
        {
            seed = value;
        }
        else
        {
            Random random = new();
            seed = random.Next(_generationSettings.MinSeed, _generationSettings.MaxSeed);
        }

        _saveStorage.ResetData();
        _saveStorage.Set(SaveConstants.SaveCreatedKey, true);
        _saveStorage.Set(SaveConstants.SeedKey, seed);
        NewGameStarted?.Invoke();
    }
}
