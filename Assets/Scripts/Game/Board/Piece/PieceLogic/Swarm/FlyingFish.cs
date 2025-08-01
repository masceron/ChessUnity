using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Effects.Traits;
using Game.Board.General;
using static Game.Common.BoardUtils;

namespace Game.Board.Piece.PieceLogic.Swarm
{
    public class FlyingFish: PieceLogic
    {
        public FlyingFish(PieceConfig cfg) : base(cfg)
        {
            SkillCooldown = -1;
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }

        private void Quiets(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(Pos);
            
            var board = MatchManager.Ins.GameState.PieceBoard;
            var active = MatchManager.Ins.GameState.ActiveBoard;

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
            var board = MatchManager.Ins.GameState.PieceBoard;

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

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            Quiets(list);
            Captures(list);

            return list;
        }
    }
}