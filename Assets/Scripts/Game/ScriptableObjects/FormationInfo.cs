using Game.Tile;
using UnityEngine;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Formation")]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FormationInfo : ScriptableObject
    {
        public FormationType type;
        public string key;
        public FormationCategory formationCategory;
        public GameObject prefab;
    }
}