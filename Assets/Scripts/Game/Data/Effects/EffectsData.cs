using Game.Common;
using Game.Effects;
using UnityEngine;

namespace Game.Data.Effects
{
    [CreateAssetMenu(fileName = "EffectsData", menuName = "ScriptableObjects/EffectsData")]
    public class EffectsData: ScriptableObject
    {
        [SerializeField] public UDictionary<EffectName, EffectObject> effectsData;
    }
}