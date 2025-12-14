using System;
using Utils;

namespace Services.UiService
{
    public interface IUiProvider
    {
        IObservable<StatData> OnStatUp { get; }
        IObservable<int> OnKillCounterUpdate { get; }

        void OnLevelUp();
        void AddKillCounter(int killCount);
    }
}