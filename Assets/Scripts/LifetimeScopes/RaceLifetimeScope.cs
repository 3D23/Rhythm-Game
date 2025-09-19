using VContainer;
using VContainer.Unity;

public class RaceLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.UseEntryPoints((builder) =>
        {
            builder.Add<RaceSceneBootstrapper>();
        });

        // Register MVP Components
        builder.Register<RacePresenter>(Lifetime.Singleton)
            .As<ScenePresenter<RaceSceneData>>()
            .AsSelf();
        builder.RegisterComponentInHierarchy<RaceView>()
            .As<IRaceView>()
            .AsSelf();
        builder.Register<RaceSceneModel>(Lifetime.Singleton)
            .As<ISceneModel<RaceSceneData>>()
            .AsSelf();

        // Register Another Components
        builder.RegisterComponentInHierarchy<Metronome>()
            .AsSelf();
        builder.RegisterComponentInHierarchy<ZoneController>()
            .AsSelf();
        builder.RegisterComponentInHierarchy<RhythmMovement>()
            .AsSelf();
    }
}
