using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDataSaver : IGameDataSaver
{
    private readonly IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> _dataRepository;

    public PlayerDataSaver(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> repository)
    {
        _dataRepository = repository;
        Application.quitting += OnQuitGame;
    }

    private async void OnQuitGame()
    {
        await Save();
    }

    public async Task Save()
    {
        await _dataRepository.Save();
    }

    public void Dispose()
    {
        Application.quitting -= OnQuitGame;
        GC.SuppressFinalize(this);
    }
}