using System.Collections.Generic;

namespace Irdaf.Messaging.Mapping
{
    public interface IContextMapper
    {
        Dictionary<string, string> Map(IPipelineContext source);

        void Map(Dictionary<string, string> headers, IPipelineContext source);
    }
}