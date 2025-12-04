using System.Collections.Generic;
using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HermitCrab : Commons.PieceLogic, IPieceWithSkill
    {
        public HermitCrab(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);

                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
                    {
                        MakeSkill(list, IndexOf(rankOff, fileOff));
                    }
                }
                else
                {
                    //query for AI in here
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
