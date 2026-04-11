using System;
using Game.Effects.Condition;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using Game.Triggers;
// using UnityEngine;

namespace Game.Effects.SpecialAbility
{
    public class BlueChromisPassive : Vengeful, IDeadTrigger
    {
        public BlueChromisPassive(PieceLogic piece) : base(piece, VengefulType.OnDeath, "effect_blue_chromis_passive")
        {

        }
        protected override void OnVengefulTrigger()
        {
            int pos = Piece.Pos;
            var dict = AssetManager.Ins.FormationData;
            var rand = new Random();
            int index = rand.Next(dict.Count);
            var pair = dict.ElementAt(index);
            Formation formation = FormationFactory.CreateInstance(pair.Key, Color, -1);
            FormationManager.Ins.SetFormation(pos, formation);
        }
    }
}