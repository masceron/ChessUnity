using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Condition;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChamberedNautilus : Commons.PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;

        public ChamberedNautilus(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BarracudaMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ChamberedNautilusHunger(this)));
            
            Skills = list =>
            {
                if (SkillCooldown != 0) return;

                var (rank, file) = RankFileOf(Pos);

                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var pOn = PieceOn(index);
                    if (pOn == null || pOn == this) continue;
                    if (pOn.Color != Color)
                    {
                        list.Add(new ChamberedNautilusActive(Pos, index));
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