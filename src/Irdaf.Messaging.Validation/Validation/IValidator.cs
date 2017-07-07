namespace Irdaf.Messaging.Validation
{
    public interface IValidator
    {
        void Validate(IPipelineContext context);
    }
}