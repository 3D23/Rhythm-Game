using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

public class Bootstrapper : IStartable
{
    [Inject] private readonly MoneySaver moneySaveLoader;
    private List<ISaver> _loaders;

    public void Start()
    {
        _loaders = new()
        {
            moneySaveLoader
        };
    }
}
