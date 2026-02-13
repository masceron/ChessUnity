using Game.Common;
using Game.Tile;
using ScrutableObjects;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "Formation", menuName = "ScriptableObjects/Formation")]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FormationsData : ScriptableObject
    {
        [ShowProperties(LockObjectAtRuntime = true)] [SerializeField]
        public UDictionary<FormationType, FormationInfo> formationsData;
    }
}