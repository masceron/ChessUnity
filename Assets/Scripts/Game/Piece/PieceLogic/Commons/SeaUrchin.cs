using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Data.Pieces;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Commons
{
    public class SeaUrchin: PieceLogic
    {
        public SeaUrchin(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Carapace(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Blinded(-1, 50, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Demolisher(this)));
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            var push = !Color ? -MaxLength : MaxLength;
            
            for (var i = 1; i <= Math.Max(EffectiveMoveRange, AttackRange); i++)
            {
                var posTo = Pos + push * i;

                if (!VerifyIndex(posTo) ||
                    !IsActive(posTo)) continue;
                
                var pieceAt = PieceOn(posTo);
                
                if (pieceAt == null)
                {
                    if (EffectiveMoveRange >= i)
                        list.Add(new NormalMove(Pos, posTo));
                }
                else if (pieceAt.PieceRank == PieceRank.Construct && AttackRange >= i)
                {
                    list.Add(new DestroyConstruct(Pos, (ushort)posTo));
                }
            }
        }
    }
}