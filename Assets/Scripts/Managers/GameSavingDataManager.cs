using System;
using UniRx;

public abstract class GameSavingDataManager<T> : IGameSavingDataManager, IDisposable
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
    
    private readonly ReactiveProperty<T> data = new();

    protected readonly IGameSavingDataSetter<T> Setter;
    protected readonly IGameSavingDataRepository<PlayerData, PlayerData.PlayerDataFields> GameDataRepository;

    public GameSavingDataManager(IGameSavingDataRepository<PlayerData, PlayerData.PlayerDataFields> repository, IGameSavingDataSetter<T> setter)
    {
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
