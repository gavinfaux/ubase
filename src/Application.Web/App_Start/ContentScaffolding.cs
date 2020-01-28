using Umbraco.Core.Composing;

namespace Application.Web
{
    public class ContentScaffoldingComposer : ComponentComposer<CachePurging>
    {
    }


    public class ContentScaffoldingComponent : IComponent
    {
        public void Initialize()
        {
        }

        public void Terminate()
        {
        }

        public void Compose(Composition composition)
        {
        }
    }
}