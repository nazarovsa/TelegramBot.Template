namespace ProjectName.Domain;

public interface IRepository<TAggregate>
{
    void Create(TAggregate aggregate);

    ValueTask<TAggregate?> GetById(long id, CancellationToken cancellationToken = default);

    void Update(TAggregate aggregate);
}