using Game.Effects.FieldEffect;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "RegionalsData", menuName = "ScriptableObjects/RegionalsData")]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RegionalsData : ScriptableObject
    {
        public GameObject whirlPoolGameObj;

        public GameObject CreateWhirlPool(Vector3 position)
        {
            return Instantiate(whirlPoolGameObj, position, Quaternion.identity);
        }

        public string GetRegionalName(FieldEffectType type)
        {
            var result = type switch
            {
                FieldEffectType.Whirlpool => "Whirpool",
                FieldEffectType.None => "No Regional effect is chosen",
                _ => type.ToString()
            };
            return result;
        }
    }
}