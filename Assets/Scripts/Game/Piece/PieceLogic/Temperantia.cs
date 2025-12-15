using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Temperantia : Commons.PieceLogic, IPieceWithSkill
    {
        public Temperantia(PieceConfig cfg) : base(cfg, TemperantiaMoves.Quiets, TemperantiaMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SnappingStrike(this)));
            var trait = new UndyingDevotion(this);
            ActionManager.ExecuteImmediately(new ApplyEffect(trait));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0 || FindPiece<Commons.PieceLogic>(!Color).Count == 0) return;
                if (isPlayer)
                {
                    Debug.Log("Temperantia: choose a ally");
                    var allies = FindPiece<Commons.PieceLogic>(Color);
                    list.AddRange(from ally in allies where !ally.Equals(this) select new TemperantiaSwap(Pos, ally.Pos));
                }
                else
                {
                    //query for AI in here
                    if (!excludeEmptyTile)
                    {
                        for (int i = 0; i < BoardSize; ++i)
                        {
                            if (IsActive(i))
                            {
                                list.Add(new TemperantiaSwap(Pos, i));
                            }
                        }
                        return;
                    }
                    // Gather ally and enemy pieces
                    var allies = FindPiece<Commons.PieceLogic>(Color);
                    var enemies = FindPiece<Commons.PieceLogic>(!Color);

                    if (allies == null || allies.Count == 0) return;
                    if (enemies == null || enemies.Count == 0) return;

                    // Count buffs on each piece
                    int CountBuffs(Commons.PieceLogic p) => p.Effects.Count(e => e.Category == Game.Effects.EffectCategory.Buff);

                    // Find minimum buff count among allies
                    var minBuff = allies.Min(CountBuffs);
                    var candidatesA = allies.Where(p => CountBuffs(p) == minBuff).ToList();
                    // Find maximum buff count among enemies
                    var maxBuff = enemies.Max(CountBuffs);
                    var candidatesB = enemies.Where(p => CountBuffs(p) == maxBuff).ToList();

                    if (candidatesA.Count == 0 || candidatesB.Count == 0) return;

                    // Choose selection (random if multiple)
                    Commons.PieceLogic chosenAlly = candidatesA.Count == 1 ? candidatesA[0] : candidatesA[Random.Range(0, candidatesA.Count)];
                    Commons.PieceLogic chosenEnemy = candidatesB.Count == 1 ? candidatesB[0] : candidatesB[Random.Range(0, candidatesB.Count)];
                    TemperantiaSwap.allyIndex = chosenAlly.Pos;
                    TemperantiaSwap.enemyIndex = chosenEnemy.Pos;

                    // Execute effect now
                    list.Add(new TemperantiaSwap(Pos, -1));
                }

            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}