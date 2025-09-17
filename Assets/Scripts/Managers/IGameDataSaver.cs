using System;
using System.Threading.Tasks;

public interface IGameDataSaver : IDisposable
{
    Task Save();
}