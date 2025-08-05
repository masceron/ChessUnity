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
    public class FlyingFish: PieceLogic
    {
        public FlyingFish(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }

        private void Quiets(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(Pos);
            
            var board = PieceBoard();
            var active = ActiveBoard();

            for (var rankTo = rank - EffectiveMoveRange; rankTo <= rank + EffectiveMoveRange; rankTo += EffectiveMoveRange)
            {
                if (!VerifyBounds(rankTo)) continue;
                for (var fileTo = file - EffectiveMoveRange; fileTo <= file + EffectiveMoveRange; fileTo += EffectiveMoveRange)
                {
                    if (!VerifyBounds(fileTo)) continue;
                    if (rankTo == rank && fileTo == file) continue;
                    var posTo = IndexOf(rankTo, fileTo);

                    if (board[posTo] == null && active[posTo])
                    {
                        list.Add(new FlyingFishMove(Pos, posTo));
                    }
                }
            }
        }

        private void Captures(List<Action.Action> list)
        {
            var board = PieceBoard();

            var ver1 = PushWhite(Pos) * AttackRange;
            var ver2 = PushBlack(Pos) * AttackRange;

            if (VerifyUpperIndex(ver1) && board[ver1] != null && board[ver1].Color != Color)
            {
                list.Add(new NormalCapture(Pos, ver1));
            }
            
            if (ver2 > 0 && board[ver2] != null && board[ver2].Color != Color)
            {
                list.Add(new NormalCapture(Pos, ver2));
            }

            var (rank, file) = RankFileOf(Pos);
                
            var fileOff1 = file - AttackRange;
            var fileOff2 = file + AttackRange;

            if (fileOff1 > 0)
            {
                var hoz1 = IndexOf(rank, fileOff1);
                
                if (board[hoz1] != null && board[hoz1].Color != Color)
                {
                    list.Add(new NormalCapture(Pos, hoz1));
                }
            }

            if (!VerifyUpperBound(fileOff2)) return;
            
            var hoz2 = IndexOf(rank, fileOff2);
            if (board[hoz2] != null && board[hoz2].Color != Color)
            {
                list.Add(new NormalCapture(Pos, hoz2));
            }
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list);
            Captures(list);
        }
    }
}