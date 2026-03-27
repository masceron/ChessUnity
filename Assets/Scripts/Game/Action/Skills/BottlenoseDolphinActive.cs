using System.Collections.Generic;
using Game.Action.Internal;
using Game.AI;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class BottlenoseDolphinActive : Action, ISkills, IAIAction
    {
        [MemoryPackConstructor]
        private BottlenoseDolphinActive()
        {
        }

        public BottlenoseDolphinActive(int maker, int target) : base(maker, target)
        {
        }

        public void CompleteActionForAI()
        {
            var a = new List<(int pos, int deltaCooldown)>();
            var b = new List<(int pos, int deltaCooldown)>();
            for (var i = 0; i < BoardSize; ++i)
            {
                var piece = PieceOn(i);
                if (piece is not IPieceWithSkill) continue;

                var deltaCooldown = ((IPieceWithSkill)PieceOn(i)).TimeToCooldown - piece.SkillCooldown;
                if (piece.Color == PieceOn(Maker).Color)
                    a.Add((i, deltaCooldown));
                else
                    b.Add((i, deltaCooldown));
            }

            if (a.Count > 0 && b.Count > 0)
            {
                var type = Random.Range(0, 1);
                if (type == 0)
                {
                    var idx = Random.Range(0, a.Count - 1);
                    SetCooldown(PieceOn(a[idx].pos).Pos, 0);
                }
                else
                {
                    var idx = Random.Range(0, b.Count - 1);
                    ActionManager.EnqueueAction(new ApplyEffect(new Silenced(PieceOn(b[idx].pos)), PieceOn(Maker)));
                }
            }
            else if (a.Count > 0)
            {
                var idx = Random.Range(0, a.Count - 1);
                SetCooldown(PieceOn(a[idx].pos).Pos, 0);
            }
            else if (b.Count > 0)
            {
                var idx = Random.Range(0, b.Count - 1);
                ActionManager.EnqueueAction(new ApplyEffect(new Silenced(PieceOn(b[idx].pos)), PieceOn(Maker)));
            }

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            if (PieceOn(Target).Color != PieceOn(Maker).Color)
                ActionManager.EnqueueAction(new ApplyEffect(new Silenced(PieceOn(Target)), PieceOn(Maker)));
            else
                SetCooldown(Target, 0);
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}