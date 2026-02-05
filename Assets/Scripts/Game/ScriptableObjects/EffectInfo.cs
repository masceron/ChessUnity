using Game.Common;
using Game.Effects;
using UnityEngine;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Effect")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EffectInfo : ScriptableObject
    {
        [SerializeField] public string key;
        [SerializeField] public EffectCategory category;
        [SerializeField] public EffectStack stack;
        [SerializeField] public ObserverPriority priority;
        [SerializeField] public Texture2D icon;
        [SerializeField] public UDictionary<EffectStat, int> otherStats;
    }
}
