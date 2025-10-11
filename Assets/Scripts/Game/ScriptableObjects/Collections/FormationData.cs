using UnityEngine;
using Game.Tile;
using Game.Common;
using Game.Effects;
using ScrutableObjects;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Enviroment", menuName = "ScriptableObjects/Enviroment")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FormationsData : ScriptableObject{
        [ShowProperties(LockObjectAtRuntime = true)]
        [SerializeField] 
        public UDictionary<FormationType, GameObject> enviromentsData;
    }

}