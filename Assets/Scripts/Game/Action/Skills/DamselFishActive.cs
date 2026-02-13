using MemoryPack;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using UnityEngine;
using Game.Effects.Buffs;
using ZLinq;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class DamselFishActive: Action, ISkills
    {
        [MemoryPackConstructor]
        private DamselFishActive() { }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        public DamselFishActive(int maker) : base(maker)
        {
            Maker = maker;
            Target = maker;
        }

        protected override void ModifyGameState()
        {   
            var pieces = FindPiece<PieceLogic>(PieceOn(Maker).Color);
            var picked = pieces
                .OrderBy(_ => Random.value)  
                .Take(Mathf.Min(3, pieces.Count))                
                .ToList();
            foreach (var piece in picked)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Rally(1, piece), PieceOn(Maker)));
            }
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}