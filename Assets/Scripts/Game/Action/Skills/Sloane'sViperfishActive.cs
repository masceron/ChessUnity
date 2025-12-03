﻿using System.Linq;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;
using Game.AI;

namespace Game.Action.Skills
{
    public class SloaneSViperfishActive : Action, ISkills, IAIAction
    {
        public SloaneSViperfishActive(int maker) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Maker);
            var caller = PieceOn(Maker);

            for (var i = -4; i <= 4; i++)
            {
                if (!VerifyBounds(rank + i)) continue;
                for (var j = -4; j <= 4; j++)
                {
                    if (!VerifyBounds(file + j)) continue;

                    var idx = IndexOf(rank + i, file + j);

                    var p = PieceOn(idx);
                    if (p == null || p == caller || p.Color == caller.Color) continue;

                    var bleeding = p.Effects.Any(t => t.EffectName == "effect_bleeding");

                    ActionManager.ExecuteImmediately(bleeding
                        ? new ApplyEffect(new Poison(1, p))
                        : new ApplyEffect(new Bleeding(5, p)));
                }
            }
        }

        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }
    }
}