using Game.Common;
using Game.Tile;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "Formation", menuName = "ScriptableObjects/Formation")]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FormationsData : ScriptableObject
    {
        [SerializeField]
        public UDictionary<FormationType, FormationInfo> formationsData;
    }
}