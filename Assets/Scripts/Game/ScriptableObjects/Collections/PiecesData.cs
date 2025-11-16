using System.Collections.Generic;
using ScrutableObjects;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "PiecesData", menuName = "ScriptableObjects/PiecesData")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PiecesData: ScriptableObject
    {
        [ShowProperties(LockObjectAtRuntime = true)]
        [SerializeField] 
        public List<PieceInfo> piecesData;
    }
}