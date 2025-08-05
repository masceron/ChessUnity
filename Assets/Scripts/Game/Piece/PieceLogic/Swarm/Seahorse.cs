using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Data.Pieces;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Swarm
{
    public class Seahorse: PieceLogic
    {
        public Seahorse(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }

        private void MakeMove(List<Action.Action> list, int rankTo, int fileTo)
        {
            var posTo = IndexOf(rankTo, fileTo);
            if (!IsActive(posTo)) return;

            var pOn = PieceOn(posTo);

            if (pOn == null)
            {
                if (Distance(Pos, posTo) == EffectiveMoveRange)
                    list.Add(new NormalMove(Pos, posTo));
            }
            else if (pOn.Color != Color && Distance(Pos, posTo) == AttackRange)
            {
                list.Add(new NormalCapture(Pos, posTo));
            }
        }

        private void Moves(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(Pos);
            var maxRange = Math.Max(AttackRange, EffectiveMoveRange);

            for (var range = 1; range <= maxRange; range++)
            {
                //Up
                var rankTo = rank - range;
                if (rankTo >= 0)
                {
                    MakeMove(list, rankTo, file - range + 1);
                    if (range >= 2)
                    {
                        MakeMove(list, rankTo, file + range - 1);
                    }
                }

                //Down
                rankTo = rank + range;
                if (VerifyUpperBound(rankTo))
                {
                    MakeMove(list, rankTo, file - range + 1);
                    if (range >= 2)
                    {
                        MakeMove(list, rankTo, file + range - 1);
                    }
                }

                //Left
                var fileTo = file - range;
                if (fileTo >= 0)
                {
                    MakeMove(list, rank - range + 1, fileTo);
                    if (range >= 2)
                    {
                        MakeMove(list, rank + range - 1, fileTo);
                    }
                }

                //Right
                fileTo = file + range;
                if (VerifyUpperBound(fileTo))
                {
                    MakeMove(list, rank - range + 1, fileTo);
                    if (range >= 2)
                    {
                        MakeMove(list, rank + range - 1, fileTo);
                    }
                }
            }
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Moves(list);
        }
    }
}