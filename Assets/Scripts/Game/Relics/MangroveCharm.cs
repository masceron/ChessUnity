using System.Collections.Generic;
using Game.Action.Relics;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MangroveCharm : RelicLogic
    {
        public MangroveCharm(RelicConfig config) : base(config)
        {
            Type = config.Type;
            Color = config.Color;
            TimeCooldown = config.TimeCooldown; // Cooldown in turns
            CurrentCooldown = 0;
        }

        private PieceLogic BestNextPiece(PieceLogic p)
        {
            var (rank, file) = RankFileOf(p.Pos);
            var potentialPiece = new List<PieceLogic>();
            for (var x = -1; x <= 1; ++x)
            for (var y = -1; y <= 1; ++y)
            {
                var nextPos = IndexOf(rank + x, file + y);
                if (!VerifyIndex(nextPos)) continue;
                var nextPiece = PieceOn(nextPos);
                if (nextPiece == null || nextPiece.Color != p.Color) continue;

                potentialPiece.Add(nextPiece);
            }

            if (potentialPiece.Count == 0) return null;

            if (Color)
                potentialPiece.Sort((a, b) => RankOf(a.Pos).CompareTo(RankOf(b.Pos)));
            else
                potentialPiece.Sort((a, b) => RankOf(b.Pos).CompareTo(RankOf(a.Pos)));

            return potentialPiece[0];
        }

        public override void Activate(List<Action.Action> actions)
        {
            throw new System.NotImplementedException();
        }

        public override void ActiveForAI()
        {
            var bestRank = Color ? 0 : 999999;
            var bestDuo = new List<(int pos1, int pos2)>();
            for (var i = 0; i < BoardSize; ++i)
            {
                var (_, _) = RankFileOf(i);
                var piece = PieceOn(i);

                if (piece == null || piece.Color != Color) continue;

                var nextPiece = BestNextPiece(piece);
                if (nextPiece == null) continue;

                var curRank = Color
                    ? Mathf.Max(RankOf(piece.Pos), RankOf(nextPiece.Pos))
                    : Mathf.Min(RankOf(piece.Pos), RankOf(nextPiece.Pos));

                if (Color)
                {
                    if (curRank > bestRank)
                    {
                        bestRank = curRank;
                        bestDuo.Clear();
                    }
                }
                else
                {
                    if (curRank < bestRank)
                    {
                        bestRank = curRank;
                        bestDuo.Clear();
                    }
                }

                if (curRank == bestRank) bestDuo.Add((piece.Pos, nextPiece.Pos));
            }

            if (bestDuo.Count == 0) return;

            var bestDuoNoExtremofiles = bestDuo.Where(a =>
                PieceOn(a.pos1).Effects.Any(e => e.EffectName != "effect_extremophile")
                && PieceOn(a.pos2).Effects.Any(e => e.EffectName != "effect_extremophile")).ToList();

            if (bestDuoNoExtremofiles.Count == 0)
            {
            //     var best = bestDuo[Random.Range(0, bestDuo.Count - 1)];
            //     var pending = new MangroveCharmPending(this, best.pos1);
            //     MangroveCharmPending.FirstTarget = PieceOn(best.pos1);
            //     MangroveCharmPending.SecondTarget = PieceOn(best.pos2);
            //     BoardViewer.Ins.ExecuteAction(pending);
            // }
            // else
            // {
            //     var best = bestDuoNoExtremofiles[Random.Range(0, bestDuoNoExtremofiles.Count - 1)];
            //     var pending = new MangroveCharmPending(this, best.pos1);
            //     MangroveCharmPending.FirstTarget = PieceOn(best.pos1);
            //     MangroveCharmPending.SecondTarget = PieceOn(best.pos2);
            //     BoardViewer.Ins.ExecuteAction(pending);
            }
            //Làm lại
        }
    }
}