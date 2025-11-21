using Game.AI;
using Game.Common;
using Game.Piece;
using static Game.Common.BoardUtils;
using Object = UnityEngine.Object;

namespace Game.Managers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceManager : Singleton<PieceManager>
    {
        private readonly Game.Piece.Piece[] pieces = new Game.Piece.Piece[BoardSize];

        public void SpawnPiece(PieceConfig config)
        {
            var pos = config.Index;
            var info = AssetManager.Ins.PieceData[config.Type];
            var prefab = info.prefab;
            var p = Instantiate(prefab, transform).AddComponent<Game.Piece.Piece>();

            pieces[pos] = p;
            p.Spawn(pos, config.Color);
        }

        /// <summary>
        /// Destroy gameObject of that piece
        /// </summary>
        /// <param name="pos"></param>
        public void Destroy(int pos)
        {
            // if (pieces[pos] == null) return;
            Object.Destroy(pieces[pos].gameObject);
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
        
        public Piece.Piece GetPieceGameObject(int pos){
            return pieces[pos];
        }
    }
}
