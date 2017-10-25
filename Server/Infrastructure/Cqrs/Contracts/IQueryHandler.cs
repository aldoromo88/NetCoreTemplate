namespace NetCoreTemplate.Infrastructure.Cqrs.Contracts
{
    public interface IQueryHandler<T> where T: class
    {
        object Handle(T command);
    }
}