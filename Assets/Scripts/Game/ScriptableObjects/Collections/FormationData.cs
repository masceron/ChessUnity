using Game.Common;
using Game.Tile;
using ScrutableObjects;
using UnityEngine;
using Game.ScriptableObjects;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "Enviroment", menuName = "ScriptableObjects/Enviroment")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FormationsData : ScriptableObject{
        [ShowProperties(LockObjectAtRuntime = true)]
        [SerializeField] 
        public UDictionary<FormationType, FormationInfo> enviromentsData;
    }

}