using System;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
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
    public class Barnacle : Commons.PieceLogic, IPieceWithSkill
    {
        private int skillCharges = 1;
        
        public Barnacle(PieceConfig cfg) : base(cfg, ShellfishMoves.Quiets, RookMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0 || skillCharges <= 0) return;

                if (isPlayer)
                {
                    var (_, _) = RankFileOf(Pos);
                    foreach (var piece in PieceBoard())
                    {
                        if (piece == null) continue;
                        if (piece.Color == Color) continue;

                        var hasShield = PieceOn(piece.Pos).Effects.Any(effect =>
                            effect.EffectName is "effect_shield" or "effect_hardened_shield");

                        if (hasShield) list.Add(new BarnacleActive(this, piece, ref skillCharges));
                    }
                }
                else
                {
                    if (excludeEmptyTile)
                    {
                        var allPieces = PieceBoard();
                        var listPieces = allPieces.Where(p => p != null && p.Color != Color &&
                                                              p.Effects.Any(e =>
                                                                  e.EffectName is "effect_shield" or "effect_hardened_shield")).ToList();

                        if (listPieces.Count == 0) return;
                        var maxValue = listPieces.Max(p => p.GetValueForAI());
                        var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
                        if (bestPieces.Count == 0) return;
                        var random = new Random();
                        var selectedPiece = bestPieces[random.Next(bestPieces.Count)];

                        list.Add(new BarnacleActive(this, selectedPiece, ref skillCharges));
                    }
                    else
                    {
                        foreach (var piece in PieceBoard())
                        {
                            if (piece == null) continue;
                            if (piece == this) continue;
                            if (!IsActive(piece.Pos)) continue;
                            list.Add(new BarnacleActive(this, piece, ref skillCharges));
                        }
                    }
                }
                //query for AI in here
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}