using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class PencilUrchin : Commons.PieceLogic, IPieceWithSkill
    {
        private int _gridSize  = 1; // kích thước NxN mặc định 1x1
        private int _castRange = 3; // bán kính cast range M

        public int GridSize  => _gridSize;
        public int CastRange => _castRange;

        public PencilUrchin(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Adaptation(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new FreeMovement(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    // PencilUrchinActive (ISkills + IDontEndTurn): đăng ký hover preview
                    list.Add(new PencilUrchinActive(Pos, Pos, _gridSize, _castRange));
                }
                else
                {
                    // AI: chọn enemy có value cao nhất trong cast range
                    var listPieces = new List<Commons.PieceLogic>();
                    var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(Pos, _castRange);
                    foreach (var target in targets)
                    {
                        if (PieceOn(target).Effects.Any(e => e.EffectName == "effect_extremophiles")) continue;
                        listPieces.Add(PieceOn(target));
                    }

                    if (listPieces.Count == 0) return;
                    var maxValue  = listPieces.Max(p => p.GetValueForAI());
                    var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
                    if (bestPieces.Count == 0) return;

                    var selected  = bestPieces[Random.Range(0, bestPieces.Count)];
                    var aiRank    = RankOf(selected.Pos);
                    var aiFile    = FileOf(selected.Pos);
                    var aiStart   = _gridSize / 2;
                    list.Add(new PencilUrchinSkillExecute(Pos, aiRank - aiStart, aiFile - aiStart, _gridSize));
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; } = 8;
        public SkillsDelegate Skills { get; set; }
    }
}