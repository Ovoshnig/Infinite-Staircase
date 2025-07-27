using System.Collections.Generic;
using System.Linq;

public class FullMatchConflictStrategy : IConflictDetectionStrategy
{
    public void ApplyConflicts(IList<KeyBinder> binders)
    {
        foreach (var binder in binders)
            binder.SetConflict(false);

        IEnumerable<IGrouping<string, KeyBinder>> controlGroups = binders
            .GroupBy(b => string.Join("/", b.Controls.CurrentValue.Select(c => c.path)));

        foreach (var group in controlGroups)
            if (group.Count() > 1)
                foreach (var binder in group)
                    binder.SetConflict(true);
    }
}
