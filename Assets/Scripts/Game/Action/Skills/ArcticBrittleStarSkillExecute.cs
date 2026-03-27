using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    /// <summary>
    /// Commit bởi ArcticBrittleStarSkillPending.
    /// Nhận startRank/startFile (góc trên-trái của lưới NxN) từ hover callback –
    /// giống cách TileManager.MarkTileInRange tính để preview khớp chính xác.
    /// </summary>
    [MemoryPackable]
    public partial class ArcticBrittleStarSkillExecute : Action
    {
        [MemoryPackInclude] private int _gridSize;
        [MemoryPackInclude] private int _startRank;
        [MemoryPackInclude] private int _startFile;

        [MemoryPackConstructor]
        private ArcticBrittleStarSkillExecute() { }

        /// <param name="maker">Vị trí quân ArcticBrittleStar</param>
        /// <param name="startRank">Rank đầu tiên (top) của lưới NxN — từ TileManager</param>
        /// <param name="startFile">File đầu tiên (left) của lưới NxN — từ TileManager</param>
        /// <param name="gridSize">Kích thước N</param>
        public ArcticBrittleStarSkillExecute(int maker, int startRank, int startFile, int gridSize)
            : base(maker)
        {
            _startRank = startRank;
            _startFile = startFile;
            _gridSize  = gridSize;
        }

        protected override void ModifyGameState()
        {
            var maker = PieceOn(Maker);
            if (maker == null) return;

            // Lặp từ startRank/startFile (top-left), khớp với TileManager preview
            for (var r = _startRank; r < _startRank + _gridSize; r++)
            for (var f = _startFile; f < _startFile + _gridSize; f++)
            {
                if (!VerifyBounds(r) || !VerifyBounds(f)) continue;
                var idx = IndexOf(r, f);
                if (!IsActive(idx) || HasFormation(idx)) continue;

                Formation anchorIce = new AnchorIce(maker.Color);
                anchorIce.SetDuration(3);
                FormationManager.Ins.SetFormation(idx, anchorIce);
            }
        }
    }
}
