

using Game.Common;
using Game.Tile;
using Game.Piece.PieceLogic;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Managers
{
    public class FormationManager : Singleton<FormationManager>{
        Formation[] enviroments;
        GameObject[] enviromentObjects;
        public void Intialize(){
            enviroments = new Formation[BoardSize];
            enviromentObjects = new GameObject[BoardSize];
        }
        /// <summary>
        /// Nên dùng để tạo Formation
        /// </summary>
        public void SetEnviroment(int pos, Formation env){
            int rank = RankOf(pos);
            int file = FileOf(pos);
            enviromentObjects[pos] = Instantiate(AssetManager.Ins.EnviromentData[env.GetFormationType()], new Vector3(rank, YCoordinate, file), 
            Quaternion.identity, this.transform);
            enviroments[pos] = env;
        }
        /// <summary>
        /// Nên dùng để xóa Formation
        /// </summary>
        public void RemoveEnviroment(int pos){
            enviroments[pos] = null;
            Destroy(enviromentObjects[pos]);
            enviromentObjects[pos] = null;
        }
        /// <summary>
        /// Được gọi bởi GameState.cs khi bất cứ quân nào dẫm phải ô có tọa độ là pos. Hàm này được gọi tự động
        /// </summary>
        public void TriggerEnter(int pos){
            if (enviroments[pos] == null) return;
            enviroments[pos].OnPieceEnter(PieceOn(pos));
        }
        /// <summary>
        /// Được gọi bởi GameState.cs khi bất cứ quân nào di chuyển khỏi ô có tọa độ là pos. Hàm này được gọi tự động
        /// </summary>
        /// <param name="pos"></param>
        public void TriggerExit(int pos, PieceLogic leavePiece){
            if (enviroments[pos] == null) return;
            enviroments[pos].OnPieceExit(leavePiece);
        }
    }
}
