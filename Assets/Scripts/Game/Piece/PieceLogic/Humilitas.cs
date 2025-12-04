using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Others;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Skills;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Humilitas : Commons.PieceLogic, IPieceWithSkill
    {
        private int deathDefianceCount;
        public Humilitas(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            deathDefianceCount = 4;
            ActionManager.EnqueueAction(new ApplyEffect(new PureMinded(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new Relentless(this, deathDefianceCount)));
            ActionManager.EnqueueAction(new ApplyEffect(new DeathDefiance(this, deathDefianceCount)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 5))
                    {
                        var idx = IndexOf(rank, file);
                        var pOn = PieceOn(idx);
                        if (pOn != null && pOn.Color != Color)
                        {
                            list.Add(new HumilitasActive(Pos, idx));
                        }
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