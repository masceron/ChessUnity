

using Game.Common;
using Game.Tile;
using Game.Piece.PieceLogic;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Managers
{
    public class FormationManager : Singleton<FormationManager>{
        Formation[] formations;
        GameObject[] formationObjects;
        public void Intialize(){
            formations = new Formation[BoardSize];
            formationObjects = new GameObject[BoardSize];
        }
        /// <summary>
        /// Gán Formation đã tạo bằng new() vào array, tại vị trí "pos" <br/>
        /// Trước khi sử dụng hàm này, hãy đảm bảo bạn đã: <br/>
        /// +Tạo và gán Prefab vào FormationsData
        /// +Tạo giá trị enum cho Formation đó
        /// +Tạo class kế thừa Formation và lấp đầy implementation cho các hàm cần thiết
        /// </summary>
        public void SetFormation(int pos, Formation env){
            int rank = RankOf(pos);
            int file = FileOf(pos);
            formationObjects[pos] = Instantiate(AssetManager.Ins.EnviromentData[env.GetFormationType()], new Vector3(rank, YCoordinate, file), 
            Quaternion.identity, this.transform);
            formations[pos] = env;
        }
        /// <summary>
        /// Nên dùng để xóa Formation
        /// </summary>
        public void RemoveFormation(int pos){
            formations[pos] = null;
            Destroy(formationObjects[pos]);
            formationObjects[pos] = null;
        }
        /// <summary>
        /// Được gọi bởi GameState.cs khi bất cứ quân nào dẫm phải ô có tọa độ là pos. Hàm này được gọi tự động
        /// </summary>
        public void TriggerEnter(int pos){
            if (formations[pos] == null) return;
            formations[pos].OnPieceEnter(PieceOn(pos));
        }
        /// <summary>
        /// Được gọi bởi GameState.cs khi bất cứ quân nào di chuyển khỏi ô có tọa độ là pos. Hàm này được gọi tự động
        /// </summary>
        /// <param name="pos"></param>
        public void TriggerExit(int pos, PieceLogic leavePiece){
            if (formations[pos] == null) return;
            formations[pos].OnPieceExit(leavePiece);
        }
    }
}
