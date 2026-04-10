using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UnityEngine;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class DamselFishActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private DamselFishActive()
        {
        }

        public DamselFishActive(PieceLogic maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var pieces = FindPiece<PieceLogic>(GetMakerAsPiece().Color);
            var picked = pieces
                .OrderBy(_ => Random.value)
                .Take(Mathf.Min(3, pieces.Count))
                .ToList();
            foreach (var piece in picked)
                ActionManager.EnqueueAction(new ApplyEffect(new Rally(1, piece), GetMakerAsPiece()));
            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}