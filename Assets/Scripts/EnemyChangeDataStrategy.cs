using System;

public class EnemyChangeDataStrategy : IChangeDataStrategy<RaceSceneData>
{
    public RaceSceneData Data => new();

    public void SetData(Action<RaceSceneData> setter)
    {
        setter(Data);
    }
}
