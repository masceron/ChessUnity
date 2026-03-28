using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArcticBrittleStar : Commons.PieceLogic, IPieceWithSkill
    {
        public ArcticBrittleStar(PieceConfig cfg) : base(cfg, KingMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Consume(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 15, this)));
            Skills = (list, isPlayer, _) =>
            {
                if (SkillCooldown > 0) return;

                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);

                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        list.Add(new ArcticBrittleStarActive(Pos, index));
                    }
                }
                else
                {
                    //query for AI in here

                    var listPieces = new List<Commons.PieceLogic>();

                    var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(Pos, 3);
                    foreach (var target in targets)
                    {
                        if (PieceOn(target).Effects.Any(e => e.EffectName == "effect_extremophile")) continue;
                        listPieces.Add(PieceOn(target));
                    }

                    if (listPieces.Count == 0) return;
                    var maxValue = listPieces.Max(p => p.GetValueForAI());
                    var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
                    if (bestPieces.Count == 0) return;

                    var random = new Random();
                    var selectedPiece = bestPieces[random.Next(bestPieces.Count)];

                    list.Add(new ArcticBrittleStarActive(Pos, selectedPiece.Pos));
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}