using System.Collections.Generic;
using System.Linq;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Swarm
{
    public class SeaStar: PieceLogic, IPieceWithSkill
    {
        private bool skillUsable;
        public SeaStar(PieceConfig cfg) : base(cfg)
        {}
        
        private void MakeMove(List<Action.Action> list, int rank, int file, int rankTo, int fileTo)
        {
            var posTo = IndexOf(rankTo, fileTo);
            if (!IsActive(posTo)) return;

            var pOn = PieceOn(posTo);

            if (pOn != null) return;
            if (Pathfinder.LineBlocker(rank, file, rankTo, fileTo).Item1 != -1) return;
                
            list.Add(new NormalMove(Pos, posTo));
            if (skillUsable && Distance(Pos, posTo) <= 1)
            {
                list.Add(new SeaStarResurrect(Pos, posTo));
            }
        }
        
        private bool MakeCapture(List<Action.Action> list, int index)
        {
            var piece = PieceOn(index);

            if (piece == null) return true;
            
            if (piece.Color != Color &&
                Pathfinder.LineBlocker(RankOf(Pos), FileOf(Pos), RankOf(index), FileOf(index)).Item1 == -1)
            {
                list.Add(new NormalCapture(Pos, index));
            }
            return false;
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            var (rank, file) = RankFileOf(Pos);
            skillUsable = SkillCooldown == 0 &&
                          (!Color ? WhiteCaptured() : BlackCaptured()).Any(p => p.Type == PieceType.SeaStar);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, EffectiveMoveRange))
            {
                MakeMove(list, rank, file, rankOff, fileOff);
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, AttackRange))
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }

            foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, AttackRange))
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, AttackRange))
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, AttackRange))
            {
                if (!MakeCapture(list, IndexOf(rankOff, fileOff))) break;
            }
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}