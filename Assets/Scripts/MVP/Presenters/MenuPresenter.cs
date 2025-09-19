using System.Collections.Generic;

public class MenuPresenter : ScenePresenter<MenuSceneData>
{
    public MenuPresenter(IEnumerable<IGameSavingDataManager> managers, ISceneModel<MenuSceneData> model, IMenuView view) : base(managers, model, view) { }
}