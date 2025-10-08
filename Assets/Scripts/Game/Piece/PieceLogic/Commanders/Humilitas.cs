using Game.Piece.PieceLogic;
using Game.Movesets;
using Game.Action.Skills;
using Game.Action;
using Game.Common;
using System.Collections.Generic;
using static Game.Common.BoardUtils;
using Game.Effects;
using Game.Effects.Others;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Action.Internal.Pending;
using static UX.UI.Ingame.BoardViewer;
using Game.Effects.Traits;

namespace Game.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Humilitas: PieceLogic, IPieceWithSkill
    {
        private int deathDefianceCount = 4 ;
        public Humilitas(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            deathDefianceCount = 4;
            ActionManager.EnqueueAction(new ApplyEffect(new PureMinded(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new Relentless(this, deathDefianceCount)));
            ActionManager.EnqueueAction(new ApplyEffect(new DeathDefiance(this)));
            if(deathDefianceCount <= 2)
            {
                ActionManager.EnqueueAction(new KillPiece(Pos));
            }
            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                foreach (var (rank, file) in MoveEnumerators.Around(RankOf(Pos), FileOf(Pos), 3))
                {
                    var idx = IndexOf(rank, file);
                    var pOn = PieceOn(idx);
                    if (pOn != null && pOn.Color != Color)
                    {
                        list.Add(new HumilitasActive(Pos, idx, 2));
                    }
                }
                // list.Add(new MultiTarget(Pos, targets, EffectName.Stunned));
            };
        }


        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}
