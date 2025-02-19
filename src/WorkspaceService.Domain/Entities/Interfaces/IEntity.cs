namespace DefaultNamespace;

public interface IEntity<T>
{
    public T Id { get; set; }
}