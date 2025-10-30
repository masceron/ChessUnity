using Game.Action.Internal;
using Game.Common;
using Game.Effects;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using System.Linq;
using UnityEngine;

namespace Game.Action.Skills
{
    public class HatchetfishActive : Action, ISkills
    {
        public HatchetfishActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target =  target;
        }

        protected override void ModifyGameState()
        {
            //Apply effect Marked
            //ActionManager.EnqueueAction(new ApplyEffect(new Marked(Target)));

            var targetPiece = BoardUtils.PieceOn(Target);
            if (targetPiece == null) return;

            if (targetPiece.Effects.Any(e => e.EffectName == EffectName.Camouflage))
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, targetPiece)));
            }

            BoardUtils.SetCooldown(Maker, ((IPieceWithSkill)BoardUtils.PieceOn(Maker)).TimeToCooldown);
        }
    }
}

