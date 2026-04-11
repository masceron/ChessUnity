using System.Collections.Generic;
using Game.Common;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Managers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceManager : Singleton<PieceManager>
    {
        private readonly Piece.Piece[] pieces = new Piece.Piece[BoardSize];

        /// <summary>
        ///     Lưu cặp (host PieceLogic → parasite Piece) để quản lý animation ký sinh.
        /// </summary>
        private readonly Dictionary<PieceLogic, Piece.Piece> _parasiteMap =
            new Dictionary<PieceLogic, Piece.Piece>();

        /// <summary>
        ///     Map (formation position → adhesive Piece) cho trường hợp Adhesive bám vào Formation.
        /// </summary>
        private readonly Dictionary<int, Piece.Piece> _adhesiveFormationMap = new();

        public void SpawnPiece(PieceConfig config)
        {
            var pos = config.Index;
            var info = AssetManager.Ins.PieceData[config.Type];
            var prefab = info.prefab;
            var p = Instantiate(prefab, transform).AddComponent<Piece.Piece>();

            pieces[pos] = p;
            p.Spawn(pos, config.Color);
        }

        /// <summary>
        ///     Destroy gameObject of that piece
        /// </summary>
        /// <param name="pos"></param>
        public void Destroy(int pos)
        {
            // if (pieces[pos] == null) return;
            Destroy(pieces[pos].gameObject);
            pieces[pos] = null;
        }

        public void Move(int from, int to)
        {
            pieces[to] = pieces[from];
            pieces[from] = null;
            pieces[to].Move(RankOf(to), FileOf(to));
        }

        public void Swap(int a, int b)
        {
            (pieces[a], pieces[b]) = (pieces[b], pieces[a]);
            pieces[a].Move(RankOf(a), FileOf(a));
            pieces[b].Move(RankOf(b), FileOf(b));
        }

        /// <summary>
        ///     Animate quân ký sinh tại <paramref name="parasitePos"/> nhảy lên đỉnh host
        ///     tại <paramref name="hostPos"/> và thu nhỏ về 40%.
        ///     Lưu cặp (hostLogic → parasitePiece) vào <see cref="_parasiteMap"/>.
        /// </summary>
        public void MoveToParasitic(int parasitePos, int hostPos, PieceLogic hostLogic)
        {
            var parasitePiece = pieces[parasitePos];
            var hostPiece     = pieces[hostPos];

            // Lưu vào map để MoveToDetach có thể tìm đúng parasite Piece
            _parasiteMap[hostLogic] = parasitePiece;

            parasitePiece.MoveToParasitic(hostPiece);
        }

        /// <summary>
        ///     Tìm parasite Piece theo <paramref name="hostLogic"/>, animate về <paramref name="to"/>
        ///     và phóng to về scale 1. Xóa entry khỏi map sau khi detach.
        /// </summary>
        public void MoveToDetach(PieceLogic hostLogic, int to)
        {
            if (!_parasiteMap.TryGetValue(hostLogic, out var parasitePiece)) return;

            pieces[to] = _parasiteMap[hostLogic];
            _parasiteMap.Remove(hostLogic);
            parasitePiece.MoveToDetach(RankOf(to), FileOf(to));
        }

        /// <summary>
        ///     Destroy cả data lẫn visual của quân ký sinh gắn với <paramref name="hostLogic"/>.
        ///     Dùng khi không có vị trí nào để Detach.
        /// </summary>
        public void DestroyParasite(PieceLogic hostLogic)
        {
            if (!_parasiteMap.TryGetValue(hostLogic, out var parasitePiece)) return;

            _parasiteMap.Remove(hostLogic);

            // Destroy gameObject (visual)
            Destroy(parasitePiece.gameObject);
        }

        // -----------------------------------------------------------------------
        // Adhesive → Formation API
        // -----------------------------------------------------------------------

        /// <summary>
        ///     Animate quân Adhesive tại <paramref name="parasitePos"/> bay lên đỉnh ô Formation
        ///     tại <paramref name="formationPos"/> rồi thu nhỏ về 0.4f. Không parent vào ai.
        ///     Lưu cặp (formationPos → adhesivePiece) vào <see cref="_adhesiveFormationMap"/>.
        /// </summary>
        public void MoveToAdhesiveFormation(int parasitePos, int formationPos)
        {
            var parasitePiece = pieces[parasitePos];

            _adhesiveFormationMap[formationPos] = parasitePiece;

            parasitePiece.MoveToAdhesiveFormation(RankOf(formationPos), FileOf(formationPos));
        }

        /// <summary>
        ///     Tìm adhesive Piece theo <paramref name="formationPos"/>, animate về <paramref name="to"/>
        ///     và phóng to về scale 1. Xóa entry khỏi map sau khi detach.
        /// </summary>
        public void MoveToDetachFromFormation(int formationPos, int to)
        {
            if (!_adhesiveFormationMap.TryGetValue(formationPos, out var adhesivePiece)) return;

            pieces[to] = adhesivePiece;
            _adhesiveFormationMap.Remove(formationPos);
            adhesivePiece.MoveToDetach(RankOf(to), FileOf(to));
        }

        /// <summary>
        ///     Destroy cả data lẫn visual của quân Adhesive gắn với Formation tại <paramref name="formationPos"/>.
        /// </summary>
        public void DestroyAdhesiveOnFormation(int formationPos)
        {
            if (!_adhesiveFormationMap.TryGetValue(formationPos, out var adhesivePiece)) return;

            _adhesiveFormationMap.Remove(formationPos);
            Destroy(adhesivePiece.gameObject);
        }

        public Piece.Piece GetPieceGameObject(int pos)
        {
            return pieces[pos];
        }
    }
}