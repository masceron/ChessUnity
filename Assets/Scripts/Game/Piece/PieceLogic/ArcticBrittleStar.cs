using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Managers;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArcticBrittleStar : Commons.PieceLogic, IPieceWithSkill
    {
        private int _gridSize = 2;
        private int castRange = 3;
        public int GridSize => _gridSize;

        public ArcticBrittleStar(PieceConfig cfg) : base(cfg, KingMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Consume(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 15, this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;

                if (isPlayer)
                {
                    var activator = new ArcticBrittleStarActive(Pos, Pos, _gridSize, castRange);
                    list.Add(activator);
                }
                else
                {
                    // AI: chọn enemy có value cao nhất, đặt AnchorIce tại đó
                    var listPieces = new List<Commons.PieceLogic>();
                    var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(Pos, castRange);
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
                    var aiRank = RankOf(selectedPiece.Pos);
                    var aiFile = FileOf(selectedPiece.Pos);
                    var aiStart = _gridSize / 2;
                    list.Add(new ArcticBrittleStarSkillExecute(Pos, aiRank - aiStart, aiFile - aiStart, _gridSize));
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}