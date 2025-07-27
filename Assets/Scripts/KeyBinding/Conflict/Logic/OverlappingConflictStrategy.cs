using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class OverlappingConflictStrategy : IConflictDetectionStrategy
{
    public void ApplyConflicts(IList<KeyBinder> binders)
    {
        foreach (var binder in binders)
            binder.SetConflict(false);

        IEnumerable<IGrouping<InputControl, (InputControl control, KeyBinder binder)>> conflictGroups = binders
            .SelectMany(b => b.Controls.CurrentValue.Select(c => (control: c, binder: b)))
            .GroupBy(cb => cb.control)
            .Where(g => g.Count() > 1);

        foreach (var group in conflictGroups)
            foreach (var (control, binder) in group)
                binder.SetConflict(true);
    }
}
