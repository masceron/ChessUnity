using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Managers;
using Game.Movesets;
using Game.Tile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Game.Common.BoardUtils;
using static Game.Common.MoveEnumerators;
using UX.UI.Ingame;
namespace Game.Piece.PieceLogic.Champions
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
                var color = this.Color;
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