using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Common;
using Game.Effects.Traits;
// <-- thêm để dùng LINQ

namespace Game.Action.Skills
{
    public class SurgeWrasseActive : Action, ISkills
    {
        public SurgeWrasseActive(int maker) : base(maker)
        {
            Target = maker;
        }
        protected override void ModifyGameState()
        {
            foreach ((var rank, var file) in MoveEnumerators.AroundUntil(RankOf(Maker), FileOf(Maker), 1))
            {
                var pieceOn = PieceOn(IndexOf(rank, file));
                if (pieceOn != null && pieceOn.Color == PieceOn(Maker).Color)
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new LongReach(pieceOn, 2, 2)));
                }
            }
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
    }
}

