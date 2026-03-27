using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Debuffs;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Assets.Scripts.Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class QuillbackRockfishActive : global::Game.Action.Action, ISkills
    {

        [MemoryPackConstructor]
        private QuillbackRockfishActive()
        {
        }

        public QuillbackRockfishActive(int maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return -25;
        }

        protected override void ModifyGameState()
        {
            var makerPiece = GetMaker();
            if (makerPiece == null) return;
            var bleedingStack = makerPiece.GetStat(SkillStat.Stack);
            var (rank, file) = RankFileOf(GetMakerPos());
            var push = makerPiece.Color ? 1 : -1;


            var frontRank = rank + push;
            if (VerifyBounds(frontRank))
            {
                var frontIndex = IndexOf(frontRank, file);
                if (VerifyIndex(frontIndex) && IsActive(frontIndex))
                {
                    var enemy = PieceOn(frontIndex);
                    if (enemy != null && enemy.Color != makerPiece.Color)
                        ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(bleedingStack, enemy), makerPiece));
                }
            }

            var backRank = rank - push;
            if (VerifyBounds(backRank))
            {
                var backIndex = IndexOf(backRank, file);
                if (VerifyIndex(backIndex) && IsActive(backIndex))
                {
                    var enemy = PieceOn(backIndex);
                    if (enemy != null && enemy.Color != makerPiece.Color)
                        ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(bleedingStack, enemy), makerPiece));
                }
            }

            ActionManager.EnqueueAction(new ApplyEffect(new Petrified(-1, makerPiece), makerPiece));

            SetCooldown(makerPiece, ((IPieceWithSkill)makerPiece).TimeToCooldown);
        }
    }
}

