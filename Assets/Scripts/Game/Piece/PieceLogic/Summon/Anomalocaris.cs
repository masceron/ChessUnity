using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using Game.Effects.Buffs;
using Game.Moves;
using static Game.Common.BoardUtils;
using SnappingStrike = Game.Effects.Traits.SnappingStrike;

namespace Game.Piece.PieceLogic.Summon
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Anomalocaris: PieceLogic, IPieceWithSkill
    {
        public Anomalocaris(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new HardenedShield(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SnappingStrike(this)));

            Skills = list =>
            {
                if (SkillCooldown != 0) return;

                var (rank, file) = RankFileOf(Pos);

                foreach (var (rankOff, fileOff) in MoveEnumerators.Around(rank, file, 5))
                {
                    MakeSkill(list, IndexOf(rankOff, fileOff));
                }
            };
        }

        private void MakeSkill(List<Action.Action> list, int index)
        {
            var piece = PieceOn(index);
            if (piece == null || piece.Color == Color) return;
            list.Add(new AnomalocarisActive(Pos, index));
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list, Pos);
            Captures(list, Pos);
            Skills(list);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}