using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class RandomItemGenerator : IInitializable, IDisposable
{
    private readonly Inventory _inventory;
    private readonly ItemDefinitionLoader _itemDefinitionLoader;
    private readonly CancellationTokenSource _cts = new();
    private readonly CompositeDisposable _compositeDisposable = new();

    public RandomItemGenerator(Inventory inventory, ItemDefinitionLoader itemDefinitionLoader)
    {
        _inventory = inventory;
        _itemDefinitionLoader = itemDefinitionLoader;
    }

    public void Initialize()
    {
        Observable
            .EveryValueChanged(Keyboard.current.numpadPlusKey, k => k.isPressed)
            .Where(isPressed => isPressed)
            .Subscribe(async _ =>
            {
                try
                {
                    ItemData itemData = await GenerateRandomItemAsync(_cts.Token);
                    _inventory.TryAddItem(itemData);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            })
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();

        _compositeDisposable.Dispose();
    }

    private async UniTask<ItemData> GenerateRandomItemAsync(CancellationToken token)
    {
        ItemDefinition itemDataSO = await _itemDefinitionLoader.GetRandomItemAsync(token);
        ItemData itemData = new(itemDataSO.name, itemDataSO.Icon);
        return itemData;
    }
}
