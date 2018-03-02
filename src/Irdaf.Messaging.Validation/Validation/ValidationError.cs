namespace Irdaf.Messaging.Validation
{
    public class ValidationError
    {
        public string Name { get; }

        public string Description { get; }

        public ValidationError(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}