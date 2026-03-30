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

        private readonly List<int> _positions;
        private readonly string _chosenType;
        public RoyalGrammaActive(PieceLogic maker, List<int> positions, string type) : base(maker)
        {
            _positions = positions;
            _chosenType = type;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            foreach (int pos in _positions)
            {
                var pieceOn = PieceOn(pos);
                List<AugmentationName> names = new();
                foreach(var aug in pieceOn.Augmentations)
                {
                    names.Add(aug.Name);
                }
                ActionManager.EnqueueAction(new DestroyPiece(pieceOn));
                ActionManager.EnqueueAction(new SpawnPiece(new Piece.PieceConfig(_chosenType, GetMakerAsPiece().Color, pos, names)));
            }
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}