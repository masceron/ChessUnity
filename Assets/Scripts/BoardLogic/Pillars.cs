using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace BoardLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Pillars : MonoBehaviour
    {
        private Pillar[] _pillarsComponents;
        [SerializeField] private GameObject[] pillarPrefabs;
        private int _maxTileNum;

        private void Awake()
        {
            _maxTileNum = transform.parent.GetComponent<Board>().maxTileNum;
            _pillarsComponents = new Pillar[_maxTileNum * _maxTileNum];
        }

        public void CreatePillar(int row, int col, bool active)
        {
            var pillar = Instantiate((row + col) % 2 == 0 ? pillarPrefabs[0] : pillarPrefabs[1], transform);
            pillar.name = $"Pillar {row}, {col}";
            pillar.transform.parent = transform;
            var script = pillar.AddComponent<Pillar>();
            script.Set(row, col, active);
            pillar.AddComponent<BoxCollider>();

            _pillarsComponents[row * _maxTileNum + col] = script;
        }

        public void Activate(int index)
        {
            _pillarsComponents[index].Activate();
        }

        public void Deactivate(int index)
        {
            _pillarsComponents[index].Deactivate();
        }
    }
}
