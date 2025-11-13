using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineIguana: PieceLogic, IPieceWithSkill
    {
        public MarineIguana(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, BluffingMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Extremophile(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new FreeMovement(this)));
            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                var (rank, file) = RankFileOf(Pos);
                var piece = PieceOn(Pos);
                var moveRange = piece.AttackRange;
                var color = Color;
                var push = color ? 1 : -1;

                foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange - 1))
                    MakeSkill(list, IndexOf(rankOff, fileOff));
                foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange - 1))
                    MakeSkill(list, IndexOf(rankOff, fileOff));

                var verticalEnumerator = color ? MoveEnumerators.Up(rank, file, moveRange - 1) : MoveEnumerators.Down(rank, file, moveRange - 1);
                foreach (var (rankOff, fileOff) in verticalEnumerator)
                    MakeSkill(list, IndexOf(rankOff, fileOff));

                var startRank = rank + push;
                var endRank = rank + push * moveRange;
                var step = push;
                
                for (var rankOff = startRank; rankOff != endRank + step; rankOff += step)
                {
                
                    var distance = Mathf.Abs(rankOff - rank);
                    for (var fileOff = file - distance; fileOff != file + distance + 1; fileOff += 1)
                    {
                        MakeSkill(list, IndexOf(rankOff, fileOff));
                    }
                }
            };
        }


        private void MakeSkill(List<Action.Action> list, int index)
        {

            var piece = PieceOn(index);
            if (piece == null || piece.Color == Color) return;
            list.Add(new MarinelGuanaAttack(Pos, index));
            
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}