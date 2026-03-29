using System.Collections.Generic;
using Game.Action.Internal;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SiphonophoreActive : Action, ISkills
    {
        [MemoryPackInclude] private List<int> _spawnPositions;

        [MemoryPackConstructor]
        private SiphonophoreActive()
        {
        }

        public SiphonophoreActive(int maker, List<int> spawnPositions) : base(maker)
        {
            _spawnPositions = spawnPositions ?? new List<int>();
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var makerPiece = GetMaker() as PieceLogic;
            if (makerPiece == null) return;

            // Spawn MiniSiphonophore ở các ô đã chọn
            foreach (var spawnPos in _spawnPositions)
            {
                if (!VerifyIndex(spawnPos) || !IsActive(spawnPos)) continue;
                if (PieceOn(spawnPos) != null) continue; // Đảm bảo ô vẫn còn trống

                ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(
                    "piece_mini_siphonophore",
                    makerPiece.Color,
                    spawnPos)));
            }

            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)makerPiece).TimeToCooldown);
        }
    }
}
