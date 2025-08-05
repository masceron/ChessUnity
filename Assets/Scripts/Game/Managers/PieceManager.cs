using Game.Common;
using Game.Data.Pieces;
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

        public void Destroy(int pos)
        {
            Object.Destroy(pieces[pos].gameObject);
            pieces[pos] = null;
        }

        public void Move(int from, int to)
        {
            pieces[to] = pieces[from];
            pieces[from] = null;
            pieces[to].Move(RankOf(to), FileOf(to));
        }
    }
}
