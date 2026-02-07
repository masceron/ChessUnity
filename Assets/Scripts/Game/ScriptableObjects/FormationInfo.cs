using UnityEngine;
using Game.Tile;
using Game.Common;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Formation")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FormationInfo : ScriptableObject
    {
        public FormationCategory formationCategory;
        public ObserverPriority priority;
        public GameObject prefab;
    }
}
