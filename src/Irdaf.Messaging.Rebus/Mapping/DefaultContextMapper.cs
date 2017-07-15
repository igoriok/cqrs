using System.Collections.Generic;

namespace Irdaf.Messaging.Mapping
{
    public class DefaultContextMapper : IContextMapper
    {
        public Dictionary<string, string> Map(IPipelineContext source)
        {
            return new Dictionary<string, string>();
        }

        public void Map(Dictionary<string, string> headers, IPipelineContext source)
        {
        }
    }
}