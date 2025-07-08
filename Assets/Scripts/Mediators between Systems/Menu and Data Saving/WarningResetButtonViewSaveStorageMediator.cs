using R3;
using System.Linq;
using UnityEngine;

public class WarningResetButtonViewSaveStorageMediator : Mediator
{
    private readonly WarningResetButtonView _warningResetButtonView;
    private readonly SaveStorage _saveStorage;

    public WarningResetButtonViewSaveStorageMediator(WarningResetButtonView warningResetButtonView, 
        SaveStorage saveStorage)
    {
        _warningResetButtonView = warningResetButtonView;
        _saveStorage = saveStorage;
    }

    public override void Initialize()
    {
        _warningResetButtonView.Clicked
            .Subscribe(_ =>
            {
                _saveStorage.ResetData();
                //Debug.Log(_saveStorage.Get(SaveConstants.InventoryKey, Enumerable.Range(0, 15).Select(_ => new Slot()).ToArray()));
            })
            .AddTo(CompositeDisposable);
    }
}
