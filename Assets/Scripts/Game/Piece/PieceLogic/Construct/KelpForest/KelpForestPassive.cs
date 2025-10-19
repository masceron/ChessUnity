using Game.Effects;
using Game.Action;
using Game.Common;
using Game.Managers;
using System.Linq;
using System.Collections.Generic;
using Game.Tile;
using UnityEngine;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Effects.Buffs;

namespace Game.Piece.PieceLogic.Construct.KelpForest
{

    // này vẫn còn thiếu trường hợp nếu kelp xuất hiện chỗ đã có piece thì không camouflage ngay được
    // do chưa dùng được enqueueaction mà để executeimmediately thì nó lại lỗi
    // chắc là do nếu mà để enqueueaction thì nó sẽ không apply effect vì nó bị cho là internal action nên không apply chắc vậy
    public class KelpForestPassive : Effect, IEndTurnEffect 
    {
        private List<int> randomPos = new List<int>();

        public KelpForestPassive(PieceLogic piece, sbyte countToSpawn) : base(countToSpawn, 1, piece, EffectName.KelpForestPassive)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
            Duration = (sbyte)(countToSpawn / 2);
            getRandomPos();
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            getRandomPos();
        }

        private void getRandomPos() 
        {
            
            int pos = new System.Random().Next(1, MatchManager.Ins.startingSize.x * MatchManager.Ins.startingSize.y);
            while (randomPos.Contains(pos))
            {
                pos = new System.Random().Next(1, MatchManager.Ins.startingSize.x * MatchManager.Ins.startingSize.y);
            }
            randomPos.Add(pos);
            if (randomPos.Count > 6) return;
            int mappedPos = BoardUtils.PosMap(pos, MatchManager.Ins.startingSize);
            FormationManager.Ins.SetFormation(mappedPos, new Kelp());
            
        }

    }
}