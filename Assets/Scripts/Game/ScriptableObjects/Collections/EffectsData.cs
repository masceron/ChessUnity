using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "EffectsData", menuName = "ScriptableObjects/EffectsData")]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EffectsData : ScriptableObject
    {
        [SerializeField]
        public List<EffectInfo> effectsData;
    }
}