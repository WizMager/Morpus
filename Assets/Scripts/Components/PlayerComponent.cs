using Configs;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using View;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct PlayerComponent : IComponent 
{
    public PlayerView playerView;
    public PlayerConfig playerConfig;
}