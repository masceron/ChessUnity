using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using Game.Effects; 
using Game.Effects.Buffs;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class QuillbackRockfishActive : Action, ISkills
    {

        [MemoryPackConstructor]
        private QuillbackRockfishActive()
        {
        }

        public QuillbackRockfishActive(int maker) : base(maker)
        {
            Maker = maker;
            Target = maker;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return -25;
        }

        protected override void ModifyGameState()
        {
            var makerPiece = PieceOn(Maker);
            if (makerPiece == null) return;
            int BleedingStack = makerPiece.GetStat(SkillStat.Stack);
            var (rank, file) = RankFileOf(Maker);
            var push = makerPiece.Color ? 1 : -1;


            var frontRank = rank + push;
            if (VerifyBounds(frontRank))
            {
                var frontIndex = IndexOf(frontRank, file);
                if (VerifyIndex(frontIndex) && IsActive(frontIndex))
                {
                    var enemy = PieceOn(frontIndex);
                    if (enemy != null && enemy.Color != makerPiece.Color)
                        ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(BleedingStack, enemy), makerPiece));
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
                        ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(BleedingStack, enemy), makerPiece));
                }
            }

            ActionManager.EnqueueAction(new ApplyEffect(new Petrified(-1, makerPiece), makerPiece));

            SetCooldown(Maker, ((IPieceWithSkill)makerPiece).TimeToCooldown);
        }
    }
}

