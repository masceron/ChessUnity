using System.Collections.Generic;
using System.Linq;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using static Game.Common.BoardUtils;
using Math = System.Math;

namespace Game.Piece.PieceLogic.Swarm
{
    public class SeaStar: PieceLogic, IPieceWithSkill
    {
        private bool skillUsable;
        public SeaStar(PieceConfig cfg) : base(cfg)
        {}
        
        private void MakeMove(List<Action.Action> list, int rank, int file, int rankTo, int fileTo)
        {
            if (Pathfinder.LineBlocker(rank, file, rankTo, fileTo).Item1 != -1) return;
            
            var posTo = IndexOf(rankTo, fileTo);
            if (!IsActive(posTo)) return;

            var pOn = PieceOn(posTo);

            if (pOn == null)
            {
                var distance = Distance(Pos, posTo);
                if (distance <= EffectiveMoveRange)
                {
                    list.Add(new NormalMove(Pos, posTo));
                }

                if (skillUsable && distance <= 1)
                {
                    list.Add(new SeaStarResurrect(Pos, posTo));
                }
            }
            else if (pOn.Color != Color && Distance(Pos, posTo) <= AttackRange)
            {
                list.Add(new NormalCapture(Pos, posTo));
            }
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(Pos);
            var maxRange = Math.Max(AttackRange, EffectiveMoveRange);
            skillUsable = SkillCooldown == 0 &&
                          (!Color ? WhiteCaptured() : BlackCaptured()).Any(p => p.Type == PieceType.SeaStar);

            for (var range = 1; range <= maxRange; range++)
            {

                //Up
                var rankTo = rank - range;
                if (rankTo >= 0)
                {
                    for (var tFileTo = file - range; tFileTo <= file + range; tFileTo++)
                    {
                        if (!VerifyBounds(tFileTo)) continue;
                        MakeMove(list, rank, file, rankTo, tFileTo);
                    }
                }

                //Down
                rankTo = rank + range;
                if (VerifyUpperBound(rankTo))
                {
                    for (var tFileTo = file - range; tFileTo <= file + range - 1; tFileTo++)
                    {
                        if (!VerifyBounds(tFileTo)) continue;
                        MakeMove(list, rank, file, rankTo, tFileTo);
                    }
                }

                //Left
                var fileTo = file - range;
                if (fileTo >= 0)
                {
                    for (var tRankTo = rank - range + 1; tRankTo <= rank + range - 1; tRankTo++)
                    {
                        if (!VerifyBounds(tRankTo)) continue;
                        MakeMove(list, rank, file, tRankTo, fileTo);
                    }
                }

                //Right
                fileTo = file + range;
                if (VerifyUpperBound(fileTo))
                {
                    for (var tRankTo = rank - range + 1; tRankTo <= rank + range; tRankTo++)
                    {
                        if (!VerifyBounds(tRankTo)) continue;
                        MakeMove(list, rank, file, tRankTo, fileTo);
                    }
                }
            }
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}