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

        public Piece.Piece GetPieceGameObject(int pos)
        {
            return pieces[pos];
        }
    }
}