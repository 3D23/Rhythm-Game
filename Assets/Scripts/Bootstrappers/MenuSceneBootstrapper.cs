using VContainer;
using VContainer.Unity;

public class MenuSceneBootstrapper : IStartable
{
    private readonly IObjectResolver resolver;
    
    public MenuSceneBootstrapper(IObjectResolver resolver) 
    {
        this.resolver = resolver;
    }

    public void Start()
    {
        resolver.Resolve<MenuPresenter>();
    }
}
