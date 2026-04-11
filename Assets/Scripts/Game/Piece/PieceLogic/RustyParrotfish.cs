using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using NUnit.Framework;
using System.Collections.Generic;
using ZLinq;

namespace Game.Piece.PieceLogic
{
    public class RustyParrotfish : Commons.PieceLogic, IPieceWithSkill
    {
        public RustyParrotfish(PieceConfig cfg) : base(cfg, ElectricEelMoves.Quiets, None.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Demolisher(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new RustyParrotfishPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    for (var i = 0; i < BoardUtils.BoardSize; ++i)
                    {
                        if (!BoardUtils.HasFormation(i)) continue;

                        list.Add(new RustyParrotfishActive(BoardUtils.PieceOn(Pos), i));
                    }
                }
                else
                {
                    //query for AI in here
                    var listA = new List<Tile.Formation>();
                    for (int i = 0; i < BoardUtils.BoardSize; ++i)
                    {
                        if (!BoardUtils.HasFormation(i)) continue;
                        
                        listA.Add(BoardUtils.GetFormation(i));
                    }

                    // Sort listA based on the value for AI, descending
                    

                    var maxValue = listA.Max(f => f.GetValueForAI());
                    var listAWithMaxValue = listA.Where(f => f.GetValueForAI() == maxValue).ToList();

                    if (listAWithMaxValue.Count > 0)
                    {
                        var idx = UnityEngine.Random.Range(0, listAWithMaxValue.Count);
                        var targetFormation = listAWithMaxValue[idx];
                        list.Add(new RustyParrotfishActive(BoardUtils.PieceOn(Pos), targetFormation.Pos));
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }

        public SkillsDelegate Skills { get; set; }
    }
}