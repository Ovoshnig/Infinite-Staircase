using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer.Unity;

public class ResolutionTuner : IInitializable
{
    public List<(int width, int height, RefreshRate refreshRate)> Resolutions { get; private set; }
    public int CurrentResolutionNumber { get; private set; }

    private (int width, int height, RefreshRate refreshRate) CurrentResolution
    {
        get
        {
            if (Screen.fullScreen)
                return (Screen.currentResolution.width, 
                    Screen.currentResolution.height, 
                    Screen.currentResolution.refreshRateRatio);
            else
                return (Screen.width, 
                    Screen.height, 
                    Screen.currentResolution.refreshRateRatio);
        }
    }

    public void Initialize()
    {
        var resolution = CurrentResolution;
        var resolutions = Screen.resolutions
            .Select(x => (x.width, x.height, x.refreshRateRatio))
            .ToList();
        Resolutions = resolutions;

        if (resolutions.Contains(resolution))
        {
            CurrentResolutionNumber = resolutions.IndexOf(resolution);
        }
        else
        {
            int index = resolutions.BinarySearch(resolution);

            if (index < 0)
                index = ~index;

            resolutions.Insert(index, resolution);
            CurrentResolutionNumber = index;
        }
    }

    public void SetResolution(int number)
    {
        if (number < 0 || number >= Resolutions.Count)
        {
            Debug.LogError($"Resolution with index {number} not found");
            return;
        }

        var (width, height, refreshRate) = Resolutions[number];
        Screen.SetResolution(width, height, Screen.fullScreenMode, refreshRate);
        CurrentResolutionNumber = number;
    }
}
