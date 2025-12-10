using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class EyeshadeSculpin : Commons.PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;

        public EyeshadeSculpin(PieceConfig cfg) : base(cfg, AmbushPredatorMoves.Quiets, AmbushPredatorMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);

                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 4))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        var pOn = PieceOn(index);
                        if (pOn == null || pOn == this || pOn.Color == Color) continue;
                        list.Add(new EyeshadeSculpinActive(Pos, index));
                    }
                }
                else
                {
                    //query for AI in here
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}