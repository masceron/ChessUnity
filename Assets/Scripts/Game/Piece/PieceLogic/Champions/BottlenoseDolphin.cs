using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Buffs;
using Game.Effects.Condition;
using Game.Effects.Traits;
using Game.Movesets;
using static Game.Common.BoardUtils;
using Game.Managers;

namespace Game.Piece.PieceLogic.Champions
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BottlenoseDolphin: PieceLogic, IPieceWithSkill
    {
        public BottlenoseDolphin(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new QuickReflex(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new BottlenoseDolphinPassive(this)));
            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null) continue;

                    list.Add(new BottlenoseDolphinActive(Pos, piece.Pos));
                }

            };

        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}