using System;
using System.Collections.Generic;
using UniRx;

public abstract class ScenePresenter<T> : IDisposable
    where T : ISceneData
{
    protected readonly IEnumerable<IGameSavingDataManager> GameDataManagers;
    protected readonly IView View;
    public readonly ISceneModel<T> Model;
    protected readonly CompositeDisposable Disposables = new();

    public ScenePresenter(IEnumerable<IGameSavingDataManager> managers, ISceneModel<T> model, IView view)
    {
        GameDataManagers = managers;
        Model = model;
        View = view;
        View.Init();
    }

    public void Dispose()
    {
        Disposables.Dispose();
        View.Dispose();
    }
}
