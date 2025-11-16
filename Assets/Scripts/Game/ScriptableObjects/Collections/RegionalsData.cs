using Game.Effects.RegionalEffect;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "RegionalsData", menuName = "ScriptableObjects/RegionalsData")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RegionalsData: ScriptableObject
    {
        public GameObject whirlPoolGameObj;
        public GameObject CreateWhirlPool(Vector3 position)
        {
            return Instantiate(whirlPoolGameObj, position, Quaternion.identity);
        }
        public string GetRegionalName(RegionalEffectType type)
        {
            string result = type switch
            {
                RegionalEffectType.Whirpool => "Whirpool",
                _ => type.ToString()
            };
            return result;
        }
    }
}