using VContainer;
using VContainer.Unity;

public class RaceSceneBootstrapper : IStartable
{
    private readonly IObjectResolver resolver;

    public RaceSceneBootstrapper(IObjectResolver resolver) 
    {
        this.resolver = resolver;
    }

    public void Start()
    {
        resolver.Resolve<RacePresenter>();
    }
}
