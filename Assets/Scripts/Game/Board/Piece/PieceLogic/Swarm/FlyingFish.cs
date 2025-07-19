using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Effects.Buffs;
using Game.Board.General;
using static Game.Common.BoardUtils;

namespace Game.Board.Piece.PieceLogic.Swarm
{
    public class FlyingFish: PieceLogic
    {
        public FlyingFish(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }

        private void Quiets(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(pos);
            
            var board = MatchManager.GameState.MainBoard;
            var active = MatchManager.GameState.ActiveBoard;

            for (var rankTo = rank - effectiveMoveRange; rankTo <= rank + effectiveMoveRange; rankTo += effectiveMoveRange)
            {
                if (!VerifyBounds(rankTo)) continue;
                for (var fileTo = file - effectiveMoveRange; fileTo <= file + effectiveMoveRange; fileTo += effectiveMoveRange)
                {
                    if (!VerifyBounds(fileTo)) continue;
                    if (rankTo == rank && fileTo == file) continue;
                    var posTo = IndexOf(rankTo, fileTo);

                    if (board[posTo] == null && active[posTo])
                    {
                        list.Add(new FlyingFishMove(pos, posTo));
                    }
                }
            }
        }

        private void Captures(List<Action.Action> list)
        {
            var board = MatchManager.GameState.MainBoard;

            var ver1 = PushWhite(pos) * attackRange;
            var ver2 = PushBlack(pos) * attackRange;

            if (VerifyUpperIndex(ver1) && board[ver1] != null && board[ver1].color != color)
            {
                list.Add(new NormalCapture(pos, pos, ver1));
            }
            
            if (ver2 > 0 && board[ver2] != null && board[ver2].color != color)
            {
                list.Add(new NormalCapture(pos, pos, ver2));
            }

            var (rank, file) = RankFileOf(pos);
                
            var fileOff1 = file - attackRange;
            var fileOff2 = file + attackRange;

            if (fileOff1 > 0)
            {
                var hoz1 = IndexOf(rank, fileOff1);
                
                if (board[hoz1] != null && board[hoz1].color != color)
                {
                    list.Add(new NormalCapture(pos, pos, hoz1));
                }
            }

            if (VerifyUpperBound(fileOff2)) return;
            
            var hoz2 = IndexOf(rank, fileOff2);
            if (board[hoz2] != null && board[hoz2].color != color)
            {
                list.Add(new NormalCapture(pos, pos, hoz2));
            }


        }

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            Quiets(list);
            Captures(list);

            return list;
        }
    }
}