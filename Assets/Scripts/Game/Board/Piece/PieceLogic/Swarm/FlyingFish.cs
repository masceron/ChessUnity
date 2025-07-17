using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Effects.Buffs;
using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.PieceLogic.Swarm
{
    public class FlyingFish: PieceLogic
    {
        public FlyingFish(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }

        private void Quiets(List<Action.Action> list)
        {
            var maxLen = MatchManager.MaxLength;
            var rank = pos / maxLen;
            var file = pos % maxLen;
            var board = MatchManager.GameState.MainBoard;
            var active = MatchManager.GameState.ActiveBoard;

            for (var rankTo = rank - moveRange; rankTo <= rank + moveRange; rankTo += moveRange)
            {
                if (rankTo < 0 || rankTo >= maxLen) continue;
                for (var fileTo = file - moveRange; fileTo <= file + moveRange; fileTo += moveRange)
                {
                    if (fileTo < 0 || fileTo >= maxLen) continue;
                    if (rankTo == rank && fileTo == file) continue;
                    var posTo = rankTo * maxLen + fileTo;

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
            var maxLen = MatchManager.MaxLength;
            var boardSize = maxLen * maxLen;

            var ver1 = pos + maxLen * attackRange;
            var ver2 = pos - maxLen * attackRange;

            if (ver1 < boardSize && board[ver1] != null && board[ver1].color != color)
            {
                list.Add(new NormalCapture(pos, pos, ver1));
            }
            
            if (ver2 > 0 && board[ver2] != null && board[ver2].color != color)
            {
                list.Add(new NormalCapture(pos, pos, ver2));
            }

            var rank = pos / maxLen;
            var file = pos % maxLen;
            var fileOff1 = file - attackRange;
            var fileOff2 = file + attackRange;

            if (fileOff1 > 0)
            {
                var hoz1 = rank * maxLen + fileOff1;
                if (board[hoz1] != null && board[hoz1].color != color)
                {
                    list.Add(new NormalCapture(pos, pos, hoz1));
                }
            }

            if (fileOff2 >= maxLen) return;
            
            var hoz2 = rank * maxLen + fileOff2;
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