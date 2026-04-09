using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    /// <summary>
    /// Đặt UrchinField trên lưới NxN từ startRank/startFile.
    /// Khớp chính xác với MarkTileInRange preview.
    /// </summary>
    [MemoryPackable]
    public partial class PencilUrchinSkillExecute : Action
    {
        [MemoryPackInclude] private int _gridSize;
        [MemoryPackInclude] private int _startRank;
        [MemoryPackInclude] private int _startFile;

        [MemoryPackConstructor]
        private PencilUrchinSkillExecute() { }

        public PencilUrchinSkillExecute(PieceLogic maker, int startRank, int startFile, int gridSize)
            : base(maker, -1)
        {
            _startRank = startRank;
            _startFile = startFile;
            _gridSize  = gridSize;
        }

        protected override void ModifyGameState()
        {
            var maker = GetMakerAsPiece();
            if (maker == null) return;

            for (var r = _startRank; r < _startRank + _gridSize; r++)
            for (var f = _startFile; f < _startFile + _gridSize; f++)
            {
                if (!VerifyBounds(r) || !VerifyBounds(f)) continue;
                var idx = IndexOf(r, f);
                if (!IsActive(idx) || HasFormation(idx)) continue;

                var urchinField = new UrchinField(true, maker.Color);
                urchinField.SetDuration(3);
                FormationManager.Ins.SetFormation(idx, urchinField);
            }
        }
    }
}
