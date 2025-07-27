using System.Collections.Generic;

public interface IConflictDetectionStrategy
{
    void ApplyConflicts(IList<KeyBinder> binders);
}
