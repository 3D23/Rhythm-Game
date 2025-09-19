using Cysharp.Threading.Tasks;
using System;

public interface IGameDataSaver : IDisposable
{
    UniTask Save();
}