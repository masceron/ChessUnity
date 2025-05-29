using UnityEngine;

namespace BoardLogic
{
    public class Tiles : MonoBehaviour
    {
        public Tile[][] BoardTiles;
        public Material tileMat;
    
        public void GenerateTile(float tileSize, int row, int col, bool active)
        {
            var tile = new GameObject($"Tile {row}, {col}");

            var mesh = new Mesh();
            var script = tile.AddComponent<Tile>();
            script.transform.parent = transform;
            script.x = row;
            script.y = col;

            tile.AddComponent<MeshFilter>().mesh = mesh;
            tile.AddComponent<MeshRenderer>().material = tileMat;

            var vertices = new Vector3[4];
            vertices[0] = new Vector3(row * tileSize, 0, col * tileSize);
            vertices[1] = new Vector3(row * tileSize, 0, (col + 1) * tileSize);
            vertices[2] = new Vector3((row + 1) * tileSize, 0, col * tileSize);
            vertices[3] = new Vector3((row + 1) * tileSize, 0, (col + 1) * tileSize);

            var triangles = new[] { 0, 1, 2, 1, 3, 2 };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            tile.AddComponent<BoxCollider>();
            tile.GetComponent<BoxCollider>().isTrigger = true;
            tile.layer = LayerMask.NameToLayer(active ? "Tile" : "Ignore Raycast");

            BoardTiles[row][col] = script;
        }
        public void Activate(int x, int y)
         {
             BoardTiles[x][y].gameObject.layer = LayerMask.NameToLayer("Tile");
         }

        public void Deactivate(int x, int y)
        {
            BoardTiles[x][y].gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
    
    
}
