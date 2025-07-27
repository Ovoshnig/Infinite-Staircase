using R3;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class KeyBindingConflictUpdater : IDisposable
{
    private readonly IConflictDetectionStrategy _conflictDetectionStrategy;

    public KeyBindingConflictUpdater(IConflictDetectionStrategy conflictDetectionStrategy) => 
        _conflictDetectionStrategy = conflictDetectionStrategy;

    private readonly Dictionary<InputActionMap, BinderGroup> _groups = new();

    public void AddKeyBinder(InputActionMap map, KeyBinder binder)
    {
        if (!_groups.TryGetValue(map, out BinderGroup group))
        {
            group = new BinderGroup(_conflictDetectionStrategy);
            _groups[map] = group;
        }

        group.Add(binder);
    }

    public void RemoveKeyBinder(InputActionMap map, KeyBinder binder)
    {
        if (_groups.TryGetValue(map, out BinderGroup group))
        {
            group.Remove(binder);

            if (group.IsEmpty)
            {
                group.Dispose();
                _groups.Remove(map);
            }
        }
    }

    public void Dispose()
    {
        foreach (var group in _groups.Values)
            group.Dispose();
        _groups.Clear();
    }

    private class BinderGroup : IDisposable
    {
        private readonly IConflictDetectionStrategy _conflictDetectionStrategy;
        private readonly List<KeyBinder> _binders = new();
        private readonly CompositeDisposable _disposables = new();

        public BinderGroup(IConflictDetectionStrategy conflictDetectionStrategy) => 
            _conflictDetectionStrategy = conflictDetectionStrategy;

        public bool IsEmpty => _binders.Count == 0;

        public void Dispose()
        {
            _disposables.Dispose();
            _binders.Clear();
        }

        public void Add(KeyBinder binder)
        {
            _binders.Add(binder);

            binder.Controls
                .Subscribe(_ => ReevaluateConflicts())
                .AddTo(_disposables);
        }

        public void Remove(KeyBinder binder) => _binders.Remove(binder);

        private void ReevaluateConflicts() => _conflictDetectionStrategy.ApplyConflicts(_binders);
    }
}
