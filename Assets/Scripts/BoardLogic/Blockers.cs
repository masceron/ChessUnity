using System.Collections.Generic;
using UnityEngine;

namespace BoardLogic
{
    public class Blockers : MonoBehaviour
    {
        private Dictionary<int, Blocker> _blockers;
        
        [SerializeField] private GameObject blockerPrefab;

        public void Init()
        {
            _blockers = new Dictionary<int, Blocker>();
        }

        public void Block(int row, int col, int index)
        {
            var blocker = Instantiate(blockerPrefab).AddComponent<Blocker>();
            blocker.name = $"Blocker {row}, {col}";
            blocker.Set(row, col);
            
            blocker.transform.parent = transform;
            
            _blockers.Add(index, blocker);
        }

        public void Unblock(int index)
        {
            var blocker = _blockers[index];
            Destroy(blocker);
            _blockers.Remove(index);
        }
    }
}
