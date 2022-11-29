namespace GuideBook.Repository.Interfaces;

public interface IEntity { }
public interface IEntityBase : IEntity<Guid> { }

public interface IEntity<out TKey> : IEntity where TKey : IEquatable<TKey>
{
    public TKey Id { get; }
}
