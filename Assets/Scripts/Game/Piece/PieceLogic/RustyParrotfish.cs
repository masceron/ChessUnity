using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Managers;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    public class RustyParrotfish : Commons.PieceLogic, IPieceWithSkill
    {
        public RustyParrotfish(PieceConfig cfg) : base(cfg, ElectricEelMoves.Quiets, None.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Demolisher(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    for (int i = 0; i < BoardUtils.BoardSize; ++i)
                    {
                        var piece = BoardUtils.PieceOn(i);
                        if (piece == null || piece.Color == Color) continue;
                        if (FormationManager.Ins.HasFormation(i) == false) continue;

                        list.Add(new RustyParrotfishActive(Pos, piece.Pos));
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