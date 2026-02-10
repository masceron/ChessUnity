using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Skills;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class RedtailParrotfish : Commons.PieceLogic, IPieceWithSkill
    {
        public RedtailParrotfish(PieceConfig cfg) : base(cfg, ElectricEelMoves.Quiets, ElectricEelMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Demolisher(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new RedtailParrotfishPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0){ return; }
                if (isPlayer)
                {
                    for(var i = 0; i < BoardSize; ++i)
                    {
                        if (!IsActive(i)) { continue; }
                        var formation = GetFormation(i);
                        if (formation != null)
                        {
                            list.Add(new RedtailParrotfishPending(Pos, i));
                        }
                    }
                }
                else
                {
                    var listA = new System.Collections.Generic.List<Formation>();
                    var listB = new System.Collections.Generic.List<int>();
                    var bestValue = int.MinValue;

                    for (int i = 0; i < BoardSize; ++i)
                    {
                        if (!IsActive(i)) { continue; }

                        bool isOurSide = IsOnBlackSide(i) == Color;
                        Formation formation = GetFormation(i);

                        if (formation == null)
                        {
                            if (isOurSide)
                            {
                                listB.Add(i);
                            }
                            continue;
                        }

                        if (isOurSide) { continue; }
                        if (formation.category != FormationCategory.Positive) { continue; }

                        int value = formation.GetValueForAI();
                        if (value > bestValue)
                        {
                            bestValue = value;
                            listA.Clear();
                            listA.Add(formation);
                        }
                        else if (value == bestValue)
                        {
                            listA.Add(formation);
                        }
                    }

                    if (listA.Count == 0 || listB.Count == 0) { return; }

                    var chosenFormation = listA.Count == 1
                        ? listA[0]
                        : listA[Random.Range(0, listA.Count)];

                    int chosenTarget = listB.Count == 1
                        ? listB[0]
                        : listB[Random.Range(0, listB.Count)];

                    list.Add(new RedtailParrotfishActive(Pos, chosenFormation.Pos, chosenTarget));
                }
            };
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}

