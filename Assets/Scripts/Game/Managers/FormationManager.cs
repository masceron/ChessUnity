

using Game.Common;
using Game.Tile;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Managers
{
    public class FormationManager : Singleton<FormationManager>, ISubscriber{
        private Formation[] formations;
        private GameObject[] formationObjects;

        public void Initialize() {
            formations = new Formation[BoardSize];
            formationObjects = new GameObject[BoardSize];
            MatchManager.Ins.GameState.Subscribers.Add(this);
        }
        
        /// <summary>
        /// Gán Formation đã tạo bằng new() vào array, tại vị trí "pos" <br/>
        /// Trước khi sử dụng hàm này, hãy đảm bảo bạn đã: <br/>
        /// +Tạo và gán Prefab vào FormationsData
        /// +Tạo giá trị enum cho Formation đó
        /// +Tạo class kế thừa Formation và lấp đầy implementation cho các hàm cần thiết
        /// </summary>
        public void SetFormation(int pos, Formation env){
            var rank = RankOf(pos);
            var file = FileOf(pos);
            formationObjects[pos] = Instantiate(AssetManager.Ins.EnviromentData[env.GetFormationType()], new Vector3(rank, YCoordinate, file), 
            Quaternion.identity, this.transform);
            formations[pos] = env;
            if (BoardUtils.PieceOn(pos) != null){
                formations[pos].OnPieceEnter(PieceOn(pos));
            }
        }

        public Formation GetFormation(int pos)
        {
            return formations[pos];
        }
        /// <summary>
        /// Nên dùng để xóa Formation
        /// </summary>
        public void RemoveFormation(int pos){
            if (formations[pos] != null) 
            {
                formations[pos] = null;
                Destroy(formationObjects[pos]);
                formationObjects[pos] = null;
            }
        }
        /// <summary>
        /// Được gọi tự động bởi GameState.cs khi bất cứ quân nào dẫm phải ô có tọa độ là pos.
        /// </summary>
        public void TriggerEnter(int pos){
            if (formations[pos] == null) return;
            formations[pos].OnPieceEnter(PieceOn(pos));
        }
        /// <summary>
        /// Được gọi tự động bởi GameState.cs khi bất cứ quân nào di chuyển khỏi ô có tọa độ là pos. 
        /// </summary>
        public void TriggerExit(int oldPos, int newPos){
            if (formations[oldPos] == null) return;
            formations[oldPos].OnPieceExit(PieceOn(newPos));
        }
        /// <summary>
        /// Được gọi tự động bởi GameState.cs khi kết thúc turn, muc đích hiện tại để xử lý duration
        /// </summary>
        public void OnCallEnd(bool endOfSide){
            
            for(var pos = 0; pos < formations.Length; pos++) {
                var format = formations[pos];

                if (format is not { HaveDuration: true } || endOfSide) continue;

                format.SetDuration(format.Duration - 1);
                if (format.Duration <= 0) {
                    RemoveFormation(pos);
                }
            }
        }
        public void OnCall(Action.Action action){}
    }
}
