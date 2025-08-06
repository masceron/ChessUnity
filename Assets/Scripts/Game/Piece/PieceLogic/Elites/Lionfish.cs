using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Elites
{
    public class Lionfish: PieceLogic, IPieceWithSkill
    {
        public Lionfish(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new LionfishVengeful(this)));
        }

        private void MakeCapture(List<Action.Action> list, int index)
        {
            var piece = PieceOn(index);
            if (piece == null || 
                piece.Color == Color || 
                Pathfinder.LineBlocker(RankOf(Pos), FileOf(Pos), RankOf(index), FileOf(index)).Item1 != -1)
                return;
            list.Add(new NormalCapture(Pos, index));
        }

        private void Captures(List<Action.Action> list)
        {
            var file = FileOf(Pos);
            var rank = RankOf(Pos);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, AttackRange))
            {
                MakeCapture(list, IndexOf(rankOff, fileOff));
            }
        }
        
        private bool MakeMove(List<Action.Action> list, int index, int distance)
        {
            if (!IsActive(index)) return false;
            var pieceOn = PieceOn(index);
            if (pieceOn != null)
            {
                return false;
            }

            if (distance <= EffectiveMoveRange)
            {
                list.Add(new NormalMove(Pos, index));
            }

            return true;
        }

        private void Quiets(List<Action.Action> list)
        {
            var rank = RankOf(Pos);
            var file = FileOf(Pos);
            var maxRange = Math.Max(AttackRange, EffectiveMoveRange);
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, maxRange))
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, maxRange))
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rankOff - rank)) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, maxRange))
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, maxRange))
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rankOff - rank)) break;
            }
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list);
            Captures(list);
            if (SkillCooldown == 0) list.Add(new LionfishActive(Pos));
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}