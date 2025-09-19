using VContainer;
using VContainer.Unity;

public class MenuLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.UseEntryPoints(builder =>
        {
            builder.Add<MenuSceneBootstrapper>();
        });

        // Register MVP objects
        builder.Register<MenuPresenter>(Lifetime.Singleton)
            .As<ScenePresenter<MenuSceneData>>()
            .AsSelf();
        builder.RegisterComponentInHierarchy<MenuView>()
            .As<IMenuView>()
            .AsSelf();
        builder.Register<MenuSceneModel>(Lifetime.Singleton)
            .As<ISceneModel<MenuSceneData>>()
            .AsSelf();
    }
}
