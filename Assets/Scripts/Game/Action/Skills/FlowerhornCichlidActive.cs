using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class FlowerhornCichlidActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private FlowerhornCichlidActive()
        {
        }

        public FlowerhornCichlidActive(PieceLogic maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var maker = GetMakerAsPiece();
            var direction = maker.Color ? 1 : -1;

            var (rank, file) = RankFileOf(GetFrom());

            for (var r = rank + direction; r != rank + maker.GetStat(SkillStat.Range) * direction; r += direction)
            {
                var pieceOn = PieceOn(IndexOf(r, file));
                if (pieceOn != null)
                {
                    if (r - direction != rank)
                    {
                        ActionManager.EnqueueAction(new NormalMove(maker, IndexOf(r - direction, file)));
                    }
                    PushPiece(pieceOn, direction);

                    ActionManager.EnqueueAction(new CooldownSkill(maker));

                    return;
                }
            }

            ActionManager.EnqueueAction(new NormalMove(maker, IndexOf(rank + maker.GetStat(SkillStat.Range) * direction, file)));
            ActionManager.EnqueueAction(new CooldownSkill(maker));

        }

        private void PushPiece(PieceLogic piece, int direction)
        {
            var (rank, file) = RankFileOf(piece.Pos);

            if (VerifyBounds(rank + direction) && VerifyIndex(IndexOf(rank + direction, file)) && PieceOn(IndexOf(rank + direction, file)) == null) 
            {
                rank += direction;
            }

            if (VerifyBounds(rank + direction) && VerifyIndex(IndexOf(rank + direction, file)) && PieceOn(IndexOf(rank + direction, file)) == null)
            {
                rank += direction;
            }

            if (piece.Pos != IndexOf(rank, file))
                ActionManager.EnqueueAction(new NormalMove(piece, IndexOf(rank, file)));

            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, piece)));
        }

    }
}