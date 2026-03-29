using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Common;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineIguana : Commons.PieceLogic, IPieceWithSkill
    {
        private const int SkillRadius = 4;

        public MarineIguana(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, BluffingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Extremophile(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new FreeMovement(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(Pos, SkillRadius);
                    foreach (var target in targets) list.Add(new MarineIguanaPending(Pos, target));
                }
                else
                {
                    var captureTargets = excludeEmptyTile
                        ? SkillRangeHelper.GetActiveEnemyPieceInRadius(Pos, SkillRadius)
                        : SkillRangeHelper.GetActiveCellInRadius(Pos, SkillRadius);

                    if (captureTargets.Contains(Pos)) captureTargets.Remove(Pos);

                    if (captureTargets.Count == 0) return;
                    var firstTarget = captureTargets[0];
                    var secondTarget = -1;
                    var maxValue = int.MinValue;
                    foreach (var target in captureTargets)
                    {
                        var maxSubValue = int.MinValue;
                        var secondSubTarget = -1;
                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(target), FileOf(target),
                                     2))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            var piece = PieceOn(index);
                            if (piece == null || piece.Color != PieceOn(target).Color || piece == PieceOn(target))
                                continue;
                            var value = piece.GetValueForAI();
                            if (value > maxSubValue)
                            {
                                maxSubValue = value;
                                secondSubTarget = index;
                                Debug.Log("maxSubValue: " + maxSubValue + " secondSubTarget: " + secondSubTarget);
                                Debug.Log("piece: " + piece.Type);
                            }
                        }

                        if (secondSubTarget == -1) continue;
                        if (PieceOn(target).GetValueForAI() + maxSubValue > maxValue)
                        {
                            maxValue = PieceOn(target).GetValueForAI() + maxSubValue;
                            firstTarget = target;
                            secondTarget = secondSubTarget;
                        }
                    }

                    if (secondTarget == -1) return;
                    var action = new MarineIguanaPending(Pos, firstTarget);
                    MarineIguanaPending.SecondTarget = PieceOn(secondTarget);
                    list.Add(action);
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}