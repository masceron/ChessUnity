using System.Linq;
using Game.Augmentation;
using Game.Common;
using Game.Tile;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Managers
{
    public class FormationManager : Singleton<FormationManager>
    {
        public GameObject defaultPrefab;
        private GameObject[] _formationObjects;
        private Formation[] _formations;

        public void Initialize()
        {
            _formations = GetFormations();
            _formationObjects = new GameObject[BoardSize];
        }

        /// <summary>
        ///     Gán Formation đã tạo bằng new() vào array, tại vị trí "pos" <br />
        ///     Trước khi sử dụng hàm này, hãy đảm bảo bạn đã: <br />
        ///     +Tạo và gán Prefab vào FormationsData
        ///     +Tạo giá trị enum cho Formation đó
        ///     +Tạo class kế thừa Formation và lấp đầy implementation cho các hàm cần thiết
        /// </summary>
        public void SetFormation(int pos, Formation env)
        {
            var rank = RankOf(pos);
            var file = FileOf(pos);
            env.SetPosition(pos);
            var prefab = AssetManager.Ins.FormationData[env.GetFormationType()].prefab;
            if (!prefab) prefab = defaultPrefab;
            _formationObjects[pos] = Instantiate(prefab, new Vector3(rank, YCoordinate, file),
                Quaternion.identity, transform);
            if (_formations[pos] != null) RemoveFormation(_formations[pos]);
            _formations[pos] = env;
            if (PieceOn(pos) != null) _formations[pos].OnCreated(PieceOn(pos));
            AddToEntityList(env);
        }

        public void MoveFormation(int from, int to)
        {
            var movedFormation = GetFormation(from);
            RemoveFormation(movedFormation);
            SetFormation(to, movedFormation);
        }

        /// <summary>
        ///     Nên dùng để xóa Formation
        /// </summary>
        public void RemoveFormation(Formation formation)
        {
            if (formation == null) return;
            formation.OnRemove(PieceOn(formation.Pos)); // luôn gọi, dù ô không có Piece (truyền null)
            _formations[formation.Pos] = null;
            Destroy(_formationObjects[formation.Pos]);
            _formationObjects[formation.Pos] = null;
        }

        public static bool IsHideByFog(int pos, bool sideToMove)
        {
            var formation = GetFormation(pos);
            var pieceInPos = PieceBoard()[pos];
            if (pieceInPos == null) return false;
            var haveAbyssalTapetum = PieceBoard().Any(pieceLogic =>
                pieceLogic != null && pieceLogic.Color == sideToMove &&
                pieceLogic.HasAugmentation(AugmentationName.AbyssalTapetum));

            return formation != null && formation.GetFormationType() == FormationType.FogOfWar
                                     && formation.GetColor() != sideToMove && !haveAbyssalTapetum
                                     && pieceInPos.Effects.All(e => e.EffectName != "effect_marked");
        }
    }
}