using System.Collections.Generic;
using Game.Action.Internal;
using Game.Augmentation;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class RoyalGrammaActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private RoyalGrammaActive()
        {
        }
        List<int> positions;
        public string chosenType;
        public RoyalGrammaActive(int maker, List<int> positions, string type) : base(maker)
        {
            this.positions = positions;
            chosenType = type;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            foreach (int pos in positions)
            {
                PieceLogic pieceOn = PieceOn(pos);
                List<AugmentationName> names = new();
                foreach(var aug in pieceOn.Augmentations)
                {
                    names.Add(aug.Name);
                }
                ActionManager.EnqueueAction(new DestroyPiece(pos));
                ActionManager.EnqueueAction(new SpawnPiece(new Piece.PieceConfig(chosenType, GetMaker().Color, pos, names)));
            }
            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}