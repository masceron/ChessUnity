using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using System.Linq;
using Game.Common;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Skills;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineIguana : Commons.PieceLogic, IPieceWithSkill
    {
        public MarineIguana(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, BluffingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Extremophile(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new FreeMovement(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var listActions = new List<Action.Action>();
                    BluffingMoves.Captures(listActions, Pos, excludeEmptyTile: true);
                    if (listActions.Count > 1)
                    {
                        listActions = listActions.Distinct(new ActionComparer()).ToList();
                    }
                    var captureTargets = listActions.OfType<ICaptures>()
                    .Select(c => ((Action.Action)c).Target)
                    .ToList();
                    foreach (var target in captureTargets)
                    {
                        list.Add(new MarineIguanaPending(Pos, target));
                    }
                }
                else
                {
                    var listActions = new List<Action.Action>();
                        BluffingMoves.Captures(listActions, Pos, excludeEmptyTile: true);
                        if (listActions.Count > 1)
                        {
                            listActions = listActions.Distinct(new ActionComparer()).ToList();
                        }
                        var captureTargets = listActions.OfType<ICaptures>()
                        .Select(c => ((Action.Action)c).Target)
                        .ToList();
                        
                    if (!excludeEmptyTile)
                    {
                        foreach (var target in captureTargets)
                        {
                            list.Add(new MarineIguanaPending(Pos, target));
                        }
                        return;
                    }

                    if (captureTargets.Count == 0) return;
                    int firstTarget = captureTargets[0];
                    int secondTarget = -1;
                    int MaxValue = int.MinValue;
                    foreach (var target in captureTargets)
                    {
                        int maxSubValue = int.MinValue;
                        int secondSubTarget = -1;
                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(target), FileOf(target), 2))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            var piece = PieceOn(index);
                            if (piece == null || piece.Color != PieceOn(target).Color || piece == PieceOn(target)) continue;
                            var value = piece.GetValueForAI();
                            if (value > maxSubValue)
                            {
                                maxSubValue = value;
                                secondSubTarget = index;
                                UnityEngine.Debug.Log("maxSubValue: " + maxSubValue + " secondSubTarget: " + secondSubTarget);
                                UnityEngine.Debug.Log("piece: " + piece.Type);
                            }
                        }
                        if (secondSubTarget == -1) continue;
                        if (PieceOn(target).GetValueForAI() + maxSubValue > MaxValue)
                        {
                            MaxValue = PieceOn(target).GetValueForAI() + maxSubValue;
                            firstTarget = target;
                            secondTarget = secondSubTarget;
                        }
                    }
                    if (secondTarget == -1) return;
                    var action = new MarineIguanaPending(Pos, firstTarget);
                    MarineIguanaActive.SecondTarget = secondTarget;
                    list.Add(action);
                    // ActionManager.EnqueueAction(new MarinelKill(Maker, firstTarget, secondTarget));
                }

            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}