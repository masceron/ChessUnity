using Game.Action.Internal;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Internal.Pending;
using UX.UI.Ingame;
using UnityEngine;
using Game.Managers;
using Game.Piece.PieceLogic;
using Game.Common;
using Game.Effects.Traits;
// <-- thêm để dùng LINQ

namespace Game.Action.Skills
{
    public class SurgeWrasseActive : Action, ISkills
    {
        public SurgeWrasseActive(ushort maker) : base(maker)
        {
            Target = maker;
        }
        protected override void ModifyGameState()
        {
            foreach ((int rank, int file) in MoveEnumerators.AroundUntil(RankOf(Maker), FileOf(Maker), 1))
            {
                PieceLogic pieceOn = PieceOn(IndexOf(rank, file));
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

