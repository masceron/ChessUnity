using System.Collections.Generic;
using ScrutableObjects;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "EffectsData", menuName = "ScriptableObjects/EffectsData")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EffectsData: ScriptableObject
    {
        [ShowProperties(LockObjectAtRuntime = true)]
        [SerializeField]
        public List<EffectInfo> effectsData;
    }
}