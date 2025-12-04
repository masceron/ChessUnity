using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using System.Linq;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineIguana : Commons.PieceLogic, IPieceWithSkill
    {
        public MarineIguana(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, BluffingMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Extremophile(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new FreeMovement(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var listActions = new List<Action.Action>();
                    BluffingMoves.Captures(listActions, Pos, isPlayer: true);
                    if (listActions.Count > 1)
                    {
                        listActions = listActions.Distinct(new ActionComparer()).ToList();
                    }
                    var captureTargets = listActions.OfType<ICaptures>()
                    .Select(c => ((Action.Action)c).Target)
                    .ToList();
                    foreach (var target in captureTargets)
                    {
                        list.Add(new MarineIguanaActive(Pos, target));
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