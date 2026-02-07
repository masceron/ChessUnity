using System.Collections.Generic;
using Game.Augmentation;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using UnityEngine;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Managers
{
    public class FormationManager : Singleton<FormationManager>{
        private GameObject[] formationObjects;
        public GameObject defaultPrefab;
        private Formation[] formations;
        public void Initialize() {
            formations = MatchManager.Ins.GameState.formations;
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
            var rank = RankOf(pos);
            var file = FileOf(pos);
            env.SetPositon(pos);
            GameObject prefab = AssetManager.Ins.FormationData[env.GetFormationType()].prefab;
            if (prefab == null){ prefab = defaultPrefab; }
            formationObjects[pos] = Instantiate(prefab, new Vector3(rank, YCoordinate, file),
            Quaternion.identity, transform);
            if (formations[pos] != null)
            {
                RemoveFormation(pos);
            }
            formations[pos] = env;
            BoardUtils.AddEffectObserver(env);
            if (PieceOn(pos) != null){
                formations[pos].OnCreated(PieceOn(pos));
            }
        }
        public void MoveFormation(int from, int to)
        {
            Formation movedFormation = GetFormation(from);
            RemoveFormation(from);
            SetFormation(to, movedFormation);
        }
        
        /// <summary>
        /// Nên dùng để xóa Formation
        /// </summary>
        public void RemoveFormation(int pos){
            if (formations[pos] != null)
            {
                if (PieceOn(pos) != null)
                {
                    formations[pos].OnRemove(PieceOn(pos));
                }
                formations[pos] = null;
                Destroy(formationObjects[pos]);
                formationObjects[pos] = null;
            }
        }
        
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
