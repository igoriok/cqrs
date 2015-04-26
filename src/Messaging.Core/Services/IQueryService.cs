namespace Irdaf.Messaging.Services
{
    public interface IQueryService
    {
        TResult Query<TResult>(IQuery<TResult> query);
    }
}