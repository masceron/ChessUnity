using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using System.Linq;
using UnityEngine;
using Game.Effects.Buffs;

namespace Game.Action.Skills
{
    public class DamselFishActive: Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        public DamselFishActive(int maker) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }

        protected override void ModifyGameState()
        {   
            var Pieces = FindPiece<PieceLogic>(PieceOn(Maker).Color);
            var picked = Pieces
                .OrderBy(_ => Random.value)  
                .Take(Mathf.Min(3, Pieces.Count))                
                .ToList();
            foreach (var piece in picked)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Rally(1, piece)));
            }
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}