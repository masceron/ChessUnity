using System.Linq;
using Game.Augmentation;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
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
            env.SetPositon(pos);
            formationObjects[pos] = Instantiate(AssetManager.Ins.EnvironmentData[env.GetFormationType()], new Vector3(rank, YCoordinate, file),
            Quaternion.identity, transform);
            if (formations[pos] != null)
            {
                RemoveFormation(pos);
            }
            formations[pos] = env;
            if (PieceOn(pos) != null){
                formations[pos].OnCreated(PieceOn(pos));
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
                if (PieceOn(pos) != null)
                {
                    formations[pos].OnDestroyed(PieceOn(pos));
                }
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

                if (format is not { HaveDuration: true } || endOfSide != format.GetColor()) continue;

                format.SetDuration(format.Duration - 1);
                if (format.Duration <= 0) {
                    RemoveFormation(pos);
                }
            }
        }
        public void OnCall(Action.Action action) { }
        public bool IsHideByFog(int pos, bool sideToMove)
        {
            Formation formation = GetFormation(pos);
            PieceLogic pieceInPos = MatchManager.Ins.GameState.PieceBoard[pos];
            if (pieceInPos == null){ return false; }
            bool haveAbyssalTapetum = false;
            foreach(PieceLogic pieceLogic in MatchManager.Ins.GameState.PieceBoard)
            {
                if (pieceLogic != null && pieceLogic.Color == sideToMove && pieceLogic.HasAugmentation(AugmentationName.AbyssalTapetum))
                {
                    haveAbyssalTapetum = true;
                    break;
                }
            }
            if (formation != null && formation.GetFormationType() == FormationType.FogOfWar 
                && formation.GetColor() != sideToMove && !haveAbyssalTapetum 
                && pieceInPos.Effects.All(e => e.EffectName != "effect_marked"))
            {
                return true;
            }
            return false;
        }
    }
}
