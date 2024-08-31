using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ActivityPaint.Integration.Database.Comparers;

public class CollectionValueComparer<T> : ValueComparer<ICollection<T>> where T : notnull
{
    public CollectionValueComparer() : base(
        (a, b) => a != null && b != null && Enumerable.SequenceEqual(a, b),
        c => c.Aggregate(0, (s, v) => HashCode.Combine(s, v.GetHashCode())),
        c => c.ToList()
    ) { }
}
