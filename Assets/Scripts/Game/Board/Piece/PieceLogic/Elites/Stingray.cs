using System.Collections.Generic;
using Game.Board.Action.Captures;
using Game.Board.Action.Quiets;
using Game.Board.Action.Skills;
using Game.Common;
using static Game.Common.BoardUtils;
using Math = System.Math;

namespace Game.Board.Piece.PieceLogic.Elites
{
    public class Stingray: PieceLogic, IPieceWithSkill
    {
        public Stingray(PieceConfig cfg) : base(cfg)
        {}

        private void Moves(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(Pos);
            var maxRange = Math.Max(AttackRange, EffectiveMoveRange);
            
            for (var rankTo = rank - maxRange; rankTo <= rank + maxRange; rankTo++)
            {
                if (!VerifyBounds(rankTo)) continue;
                for (var fileTo = file - maxRange; fileTo <= file + maxRange; fileTo++)
                {
                    if (!VerifyBounds(fileTo)) continue;
                    if (rankTo == rank && fileTo == file) continue;
                    if (Pathfinder.LineBlocker(rank, file, rankTo, fileTo, PieceBoard()).Item1 != -1) continue;
                    
                    var posTo = IndexOf(rankTo, fileTo);
                    if (!IsActive(posTo)) continue;

                    var pOn = PieceOn(posTo);

                    if (pOn == null)
                    {
                        if (Distance(Pos, posTo) <= EffectiveMoveRange)
                            list.Add(new NormalMove(Pos, posTo));
                    }
                    else if (pOn.Color != Color && Distance(Pos, posTo) <= AttackRange)
                    {
                        list.Add(new NormalCapture(Pos, posTo));
                    }
                }
            }
        }

        private void Skills(List<Action.Action> list)
        {
            if (SkillCooldown != 0) return;
            
            var (rank, file) = RankFileOf(Pos);
            
            var board = PieceBoard();
            var active = ActiveBoard();

            for (var rankTo = rank - 2; rankTo <= rank + 2; rankTo += 2)
            {
                if (!VerifyBounds(rankTo)) continue;
                for (var fileTo = file - 2; fileTo <= file + 2; fileTo += 2)
                {
                    if (!VerifyBounds(fileTo)) continue;
                    if (rankTo == rank && fileTo == file) continue;
                    var posTo = IndexOf(rankTo, fileTo);

                    if (board[posTo] == null && active[posTo])
                    {
                        list.Add(new StingrayDash(Pos, posTo));
                    }
                }
            }
        }

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();

            Moves(list);
            Skills(list);
            return list;
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}