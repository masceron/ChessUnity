using Game.Effects;
using UnityEngine;
using Game.Tile;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Formation")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FormationInfo : ScriptableObject
    {
        public string key = "no_key";
        public FormationCategory formationCategory;
        public GameObject prefab;
    }
}
