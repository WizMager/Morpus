using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Utils;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct LevelUpComponent : IComponent 
{
    public StatData statData;
}