using System;
using UniRx;

public abstract class GameDataManager<T> : IDisposable
{
    private bool _disposed = false;

    public ReactiveProperty<T> Data
    {
        get => data;
        set
        {
            UpdateData(value.Value);
            data.Value = GetData();
        }
    }
    
    private readonly ReactiveProperty<T> data;

    protected readonly IGameDataSetter<T> Setter;
    protected readonly IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> GameDataRepository;

    public GameDataManager(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> repository, IGameDataSetter<T> setter)
    {
        Data = new();
        Setter = setter;
        GameDataRepository = repository;
        Data.Value = GetData();
    }

    protected abstract T GetData();

    protected abstract void UpdateData(T value);

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Data?.Dispose();
        GC.SuppressFinalize(this);
    }
}