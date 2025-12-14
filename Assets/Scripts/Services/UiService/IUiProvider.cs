using System;

namespace Services.UiService
{
    public interface IUiProvider : IDisposable
    {
        IObservable<StatData> OnStatUp { get; }
        IObservable<int> OnKillCounterUpdate { get; }

        void OnLevelUp();
        void AddKillCounter(int killCount);
    }
}