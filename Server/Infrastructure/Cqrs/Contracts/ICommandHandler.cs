namespace NetCoreTemplate.Infrastructure.Cqrs.Contracts
{
    public interface ICommandHandler<T> where T: class
    {
        object Handle(T command);
    }
}