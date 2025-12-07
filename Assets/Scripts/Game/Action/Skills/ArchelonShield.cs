using Game.Action.Internal;
using Game.AI;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using System.Collections.Generic;
using System.Linq;
using UX.UI.Ingame;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArchelonShield: Action, ISkills, IAIAction
    {
        public ArchelonShield(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }

        public void CompleteActionForAI()
        {
            var makerPiece = PieceOn(Maker);
            if (makerPiece == null) return;

            var rank = RankOf(Maker);
            var file = FileOf(Maker);

            // Gather allies within radius 3
            var candidates = new List<PieceLogic>();
            for (int dr = -3; dr <= 3; dr++)
            {
                for (int df = -3; df <= 3; df++)
                {
                    int r = rank + dr;
                    int f = file + df;
                    if (!VerifyBounds(r) || !VerifyBounds(f)) continue;

                    var piece = PieceOn(IndexOf(r, f));
                    if (piece == null || piece.Color != makerPiece.Color) continue;

                    // Filter: no Shield, Hardened Shield, or Extremophile effect
                    if (piece.Effects.Any(e => e.EffectName == "effect_shield" || 
                                                e.EffectName == "effect_hardened_shield" || 
                                                e.EffectName == "effect_extremophile")) 
                    {
                        continue;
                    }

                    candidates.Add(piece);
                }
            }

            if (candidates.Count == 0) return;

            // Find minimum value
            var minValue = candidates.Min(p => p.GetValueForAI());
            var lowest = candidates.Where(p => p.GetValueForAI() == minValue).ToList();

            // Pick one (random if multiple)
            var chosen = lowest.Count == 1 ? lowest[0] : lowest[Random.Range(0, lowest.Count)];

            Target = (ushort)chosen.Pos;
            BoardViewer.Ins.ExecuteAction(this);
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(PieceOn(Target))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}