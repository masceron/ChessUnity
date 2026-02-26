using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Effects.Buffs;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class BlackPrinceCopepod : Commons.PieceLogic, IPieceWithSkill
    {
        public BlackPrinceCopepod(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Sanity(-1, this)));
            ActionManager.EnqueueAction(new ApplyEffect(new HardenedShield(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.EnqueueAction(new ApplyEffect(new BlackPrinceCopepodPassive(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0 || FindPiece<Commons.PieceLogic>(!Color).Count == 0) return;
                if (isPlayer)
                {
                    var pendingActions = from piece in PieceBoard()
                        where piece != null && !piece.Equals(this)
                        select new BlackPrinceCopepodPending(Pos, piece.Pos);
                    foreach (var action in pendingActions) list.Add(action);
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}