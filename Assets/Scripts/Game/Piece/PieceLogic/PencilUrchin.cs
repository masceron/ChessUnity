using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Piece.PieceLogic
{
    public class PencilUrchin : Commons.PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;

        public PencilUrchin(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Adaptation(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new FreeMovement(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
                    {
                        list.Add(new PencilUrchinActive(Pos, IndexOf(rankOff, fileOff)));
                    }
                }
                else
                {
                    //query for AI in here
                    if (excludeEmptyTile)
                    {

                    }
                    else
                    {

                    }
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