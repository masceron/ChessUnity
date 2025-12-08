using System.Runtime.CompilerServices;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Condition;
using Game.Effects.Traits;
using Game.Managers;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BottlenoseDolphin : Commons.PieceLogic, IPieceWithSkill
    {
        public BottlenoseDolphin(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new QuickReflex(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new BottlenoseDolphinPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                    {
                        if (piece == null) continue;

                        list.Add(new BottlenoseDolphinActive(Pos, piece.Pos));
                    }
                }
                else
                {
                    //query for AI in here
                }

            };
        }
        
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}