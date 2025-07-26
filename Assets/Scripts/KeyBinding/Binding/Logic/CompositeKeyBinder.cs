using System;
using System.Linq;
using UnityEngine.InputSystem;

public abstract class CompositeKeyBinder : KeyBinder
{
    protected CompositeKeyBinder(KeyListeningTracker tracker, SettingsStorage storage, InputAction action)
        : base(tracker, storage, action) { }

    protected abstract string[] DirectionNames { get; }

    protected override string GetWaitingText(int index) =>
        $"Ожидание ввода {DirectionNames[index]} клавиши...";

    protected override void ApplyBindingOverrides(InputControl[] controls)
    {
        InputBinding[] partBindings = InputAction.bindings
            .Where(b => b.isPartOfComposite)
            .ToArray();

        if (partBindings.Length != controls.Length)
            throw new InvalidOperationException("Mismatch between controls and composite parts.");

        for (int i = 0; i < controls.Length; i++)
            InputAction.ApplyBindingOverride(i + 1, controls[i].path);
    }
}
