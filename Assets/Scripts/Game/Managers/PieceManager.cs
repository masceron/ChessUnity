using Game.Common;
using Game.Piece;
using static Game.Common.BoardUtils;

namespace Game.Managers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceManager : Singleton<PieceManager>
    {
        private readonly Piece.Piece[] pieces = new Piece.Piece[BoardSize];

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
        ///     Animate <paramref name="parasitePos"/> nhảy lên đỉnh <paramref name="hostPos"/> và thu nhỏ về 0.3f.
        ///     Không cập nhật array pieces (quân ký sinh vẫn giữ nguyên vị trí logic).
        /// </summary>
        public void MoveToParasitic(int parasitePos, int hostPos)
        {
            var hostPiece = pieces[hostPos];
            pieces[parasitePos].MoveToParasitic(hostPiece);
        }

        /// <summary>
        ///     Animate quân tại <paramref name="from"/> di chuyển về <paramref name="to"/> và phóng to về scale 1.
        ///     Cập nhật array pieces giống như Move.
        /// </summary>
        public void MoveToDetach(int from, int to)
        {
            pieces[to] = pieces[from];
            pieces[from] = null;
            pieces[to].MoveToDetach(RankOf(to), FileOf(to));
        }

        public Piece.Piece GetPieceGameObject(int pos)
        {
            return pieces[pos];
        }
    }
}