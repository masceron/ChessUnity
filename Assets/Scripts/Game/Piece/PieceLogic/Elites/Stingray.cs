using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using static Game.Common.BoardUtils;
using Math = System.Math;

namespace Game.Piece.PieceLogic.Elites
{
    public class Stingray: PieceLogic, IPieceWithSkill
    {
        public Stingray(PieceConfig cfg) : base(cfg)
        {}

        private void MakeMove(List<Action.Action> list, int rank, int file, int rankTo, int fileTo)
        {
            
            if (Pathfinder.LineBlocker(rank, file, rankTo, fileTo).Item1 != -1) return;
                    
            var posTo = IndexOf(rankTo, fileTo);
            if (!IsActive(posTo)) return;

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

        private void Moves(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(Pos);
            var maxRange = Math.Max(AttackRange, EffectiveMoveRange);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, maxRange))
            {
                MakeMove(list, rank, file, rankOff, fileOff);
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

        protected override void MoveToMake(List<Action.Action> list)
        {
            Moves(list);
            Skills(list);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}