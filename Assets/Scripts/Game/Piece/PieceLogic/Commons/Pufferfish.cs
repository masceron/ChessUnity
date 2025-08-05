using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Data.Pieces;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Commons
{
    public class Pufferfish: PieceLogic, IPieceWithSkill
    {
        public Pufferfish(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Dominator(this)));
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            var range = Math.Max(AttackRange, EffectiveMoveRange);
            var push = !Color ? -1 : 1;

            for (var rank = RankOf(Pos) - (range - 1) * push; rank != RankOf(Pos) + (range + 1) * push; rank += push)
            {
                if (!VerifyBounds(rank)) continue;
                for (var file = FileOf(Pos) - 1; file <= FileOf(Pos) + 1; file++)
                {
                    if (!VerifyBounds(file)) continue;
                    var idx = IndexOf(rank, file);
                    if (!IsActive(idx)) continue;
                    
                    var piece = PieceOn(idx);
                    
                    if (piece == null)
                    {
                        list.Add(new NormalMove(Pos, idx));
                    }
                    else if (piece.Color != Color && piece.PieceRank < PieceRank)
                    {
                        list.Add(new NormalCapture(Pos, idx));
                    }
                }
            }

            list.Add(new PufferfishExplode(Pos));
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}