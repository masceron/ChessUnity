using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Managers;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Thalassos : Commons.PieceLogic, IPieceWithSkill
    {
        public Thalassos(PieceConfig cfg) : base(cfg, ThalassosMoves.Quiets, ThalassosMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ThalassosShielder(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    for (var rankOff = -1; rankOff <= 1; rankOff++)
                    {
                        var rank = RankOf(Pos) + rankOff;
                        if (!VerifyBounds(rank)) continue;

                        for (var fileOff = -1; fileOff <= 1; fileOff++)
                        {
                            if (rankOff == 0 && fileOff == 0) continue;
                            var file = FileOf(Pos) + fileOff;
                            if (!VerifyBounds(file)) continue;
                            var posTo = IndexOf(rank, file);

                            if (PieceOn(posTo) == null) list.Add(new ThalassosResurrectCandidate(this, posTo));
                        }
                    }
                }
                else
                {
                    //query for AI in here
                    if (!excludeEmptyTile)
                    {
                        for (var rankOff = -1; rankOff <= 1; rankOff++)
                        {
                            var rank = RankOf(Pos) + rankOff;
                            for (var fileOff = -1; fileOff <= 1; fileOff++)
                            {
                                if (rankOff == 0 && fileOff == 0) continue;
                                var file = FileOf(Pos) + fileOff;
                                var posTo = IndexOf(rank, file);

                                if (VerifyBounds(rank) && VerifyBounds(file) && IsActive(posTo))
                                    list.Add(new ThalassosResurrectCandidate(this, posTo));
                            }
                        }

                        return;
                    }

                    // captured list for the maker's side
                    var capturedList = Color
                        ? MatchManager.Ins.GameState.BlackCaptured
                        : MatchManager.Ins.GameState.WhiteCaptured;
                    if (capturedList == null || capturedList.Count == 0) return;

                    // Filter candidates: only Common or Swarm rank
                    var candidates = capturedList.ToList()
                        .Where(cfg =>
                        {
                            try
                            {
                                var info = AssetManager.Ins.PieceData[cfg.Type];
                                return info.rank == PieceRank.Common || info.rank == PieceRank.Swarm;
                            }
                            catch
                            {
                                return false;
                            }
                        })
                        .ToList();
                    if (candidates.Count == 0) return;

                    // Find empty squares around Maker within radius 1
                    var (r0, f0) = RankFileOf(Pos);
                    var emptySquares = new List<int>();
                    for (var dr = -1; dr <= 1; dr++)
                    for (var df = -1; df <= 1; df++)
                    {
                        var rr = r0 + dr;
                        var ff = f0 + df;
                        if (!VerifyBounds(rr) || !VerifyBounds(ff)) continue;
                        var idx = IndexOf(rr, ff);
                        if (!IsActive(idx)) continue;
                        if (PieceOn(idx) == null) emptySquares.Add(idx);
                    }

                    if (emptySquares.Count == 0) return;

                    // Pick best candidate
                    var bestScore = candidates.Max(p => PieceMaker.Get(p).GetValueForAI());
                    var top = candidates.Where(c => PieceMaker.Get(c).GetValueForAI() == bestScore).ToList();
                    var chosenPiece = top.Count == 1 ? top[0] : top[Random.Range(0, top.Count)];

                    // Pick random empty square
                    var chosenSquare = emptySquares[Random.Range(0, emptySquares.Count)];

                    // Spawn piece immediately
                    list.Add(new ThalassosResurrect(Pos, chosenSquare, chosenPiece.Type));

                    // Remove the resurrected piece from captured list
                    var toRemove =
                        capturedList.FirstOrDefault(c => c.Type == chosenPiece.Type && c.Color == chosenPiece.Color);
                    if (toRemove != null) capturedList.Remove(toRemove);
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}