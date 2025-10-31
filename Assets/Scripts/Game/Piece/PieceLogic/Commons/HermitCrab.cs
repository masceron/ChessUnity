using Game.Movesets;
using Game.Action.Skills;
using static Game.Common.BoardUtils;
using Game.Common;
using System.Collections.Generic;

namespace Game.Piece.PieceLogic.Commons
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HermitCrab: PieceLogic, IPieceWithSkill
    {
        public HermitCrab(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                var (rank, file) = RankFileOf(Pos);

                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
                {
                    MakeSkill(list, IndexOf(rankOff, fileOff));
                }
            };
        }

        private void MakeSkill(List<Action.Action> list, int index)
        {
            var piece = PieceOn(index);
            if (piece == null || piece.Color != Color) return;
            list.Add(new HermitCrabSwap(Pos, index));
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}
