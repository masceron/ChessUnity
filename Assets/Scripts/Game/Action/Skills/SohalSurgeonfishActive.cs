using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.AI;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using System.Linq;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SohalSurgeonfishActive: Action, ISkills
    {
        public SohalSurgeonfishActive(int maker) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }
        protected override void ModifyGameState()
        {
            foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Maker), FileOf(Maker), 6))
            {
                var idx = IndexOf(rank, file);
                var pOn = PieceOn(idx);
                if (pOn != null && pOn.Color != PieceOn(Maker).Color && pOn.Effects.Any(e => e.EffectName == "effect_slow"))
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Leashed(pOn, pOn.Pos, 5)));
                }
            }
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}