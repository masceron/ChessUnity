using Game.Common;
using Game.Effects;
using UnityEngine;

namespace Game.Data.Effects
{
    [CreateAssetMenu(fileName = "EffectsData", menuName = "ScriptableObjects/EffectsData")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EffectsData: ScriptableObject
    {
        [SerializeField] public UDictionary<EffectName, EffectObject> effectsData;
    }
}