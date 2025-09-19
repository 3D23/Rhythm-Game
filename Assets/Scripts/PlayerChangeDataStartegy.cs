using System;

public class PlayerChangeDataStartegy : IChangeDataStrategy<RaceSceneData>
{
    private readonly ScenePresenter<RaceSceneData> presenter;
    public PlayerChangeDataStartegy(ScenePresenter<RaceSceneData> presenter) =>
        this.presenter = presenter;

    public RaceSceneData Data => presenter.Model.Data;

    public void SetData(Action<RaceSceneData> setter)
    {
        presenter.ModifyModel(model => setter(model.Data));
    }
}
