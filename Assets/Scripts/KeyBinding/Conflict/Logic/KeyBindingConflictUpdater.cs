using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class KeyBindingConflictUpdater : IDisposable
{
    private readonly Dictionary<InputActionMap, BinderGroup> _groups = new();

    public void AddKeyBinder(InputActionMap map, KeyBinder binder)
    {
        if (!_groups.TryGetValue(map, out BinderGroup group))
        {
            group = new BinderGroup();
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
        private readonly List<KeyBinder> _binders = new();
        private readonly CompositeDisposable _disposables = new();

        public bool IsEmpty => _binders.Count == 0;

        public void Add(KeyBinder binder)
        {
            _binders.Add(binder);

            binder.Controls
                .Subscribe(_ => ReevaluateConflicts())
                .AddTo(_disposables);
        }

        public void Remove(KeyBinder binder) => _binders.Remove(binder);

        private void ReevaluateConflicts()
        {
            IEnumerable<IGrouping<string, KeyBinder>> controlGroups = _binders
                .GroupBy(b => string.Join('/', b.Controls.CurrentValue
                    .OrderBy(c => c.path)
                    .Select(c => c.path)));

            foreach (var binder in _binders)
                binder.SetConflict(false);

            foreach (var controlGroup in controlGroups)
                if (controlGroup.Count() > 1)
                    foreach (var binder in controlGroup)
                        binder.SetConflict(true);
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _binders.Clear();
        }
    }
}
