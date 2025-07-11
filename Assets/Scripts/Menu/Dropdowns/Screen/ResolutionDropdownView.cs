using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ResolutionDropdownView : DropdownView
{
    public void SetResolutionOptions(List<(int width, int height, RefreshRate refreshRate)> resolutions)
    {
        List<TMP_Dropdown.OptionData> options = resolutions
            .Select(r => new TMP_Dropdown.OptionData($"{r.width}x{r.height}@{r.refreshRate.value:F2}"))
            .ToList();
        SetOptions(options);
    }
}
