using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "PiecesData", menuName = "ScriptableObjects/PiecesData")]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PiecesData : ScriptableObject
    {
        [SerializeField]
        public List<PieceInfo> piecesData;
    }
}