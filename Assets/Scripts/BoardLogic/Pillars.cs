using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace BoardLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Pillars : MonoBehaviour
    {
        public Pillar[] pillarsComponents;
        public GameObject[] pillarPrefabs;
        public int maxTileNum;
    
        public void CreatePillar(float tileSize, int row, int col, bool active)
        {
            var pillar = Instantiate((row + col) % 2 == 0 ? pillarPrefabs[0] : pillarPrefabs[1], transform);
            pillar.name = $"Pillar {row}, {col}";
            pillar.transform.parent = transform;
            var script = pillar.AddComponent<Pillar>();
            script.posX = row;
            script.posY = col;
            script.size = tileSize;
            script.maxTileNum = maxTileNum;
            script.active = active;
            pillar.AddComponent<BoxCollider>();

            pillarsComponents[row * maxTileNum + col] = script;
        }

        public void Activate(int index)
        {
            pillarsComponents[index].Activate();
        }

        public void Deactivate(int index)
        {
            pillarsComponents[index].Deactivate();
        }
    }
}
