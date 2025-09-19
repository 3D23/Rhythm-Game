using System;

public interface IChangeDataStrategy<T> 
    where T : ISceneData
{
    T Data { get; }

    void SetData(Action<T> setter);
}
