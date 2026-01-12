using System;
using System.Collections.Generic;

namespace Game.Action
{

    [Flags]
    public enum ActionFlag : byte
    {
        None = 0,
        Unblockable = 1,
        Undodgeable = 1 << 1,

    }
    public enum ResultFlag
    {
        Success, // Action thành công
        Blocked, // Shield thành công chặn ăn quân
        Miss, // Blinded thành công làm quân ăn trượt
        HardenedBlock, // Hardened shield chặn
        Parry, // Carapace chặn
        Evade, // Evasion chặn
        Incorruptible, //(Trait) Sanity (Aug), Metal Mind thành công chặn hiệu ứng
        Untouchable, // (Aug) Metal Spine, (Aug) Metal Regulator, (Trait) Extremophile, Thành công chặn hiệu ứng
        Unshaken, // (Trait) Free Movement, (Aug) Cold Blooded, Thành công chặn hiệu ứng
        Muted, //(Aug) Crown of Silience thành công chặn Commander địch sử dụng Skill.
        CantApplyEffect, // Không thể áp dụng effect nói chung
        EffectResistance, // Sử dụng trong trường hợp Kháng Effect
        SurvivedHit, // bị ăn nhưng không chết
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Action
    {
        public int Target = -1;
        public int Maker;
        public ResultFlag Result = ResultFlag.Success;
        public ActionFlag Flag = ActionFlag.None;

        protected Action(int maker)
        {
            Maker = maker;
        }
        
        public void Execute()
        {
            Animate();
            ModifyGameState();
        }

        protected virtual void Animate()
        {
            
        }

        public bool IsValid()
        {
            return Result == ResultFlag.Success;
        }
        protected abstract void ModifyGameState();
    }

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ActionComparer: IEqualityComparer<Action>
    {
        public bool Equals(Action x, Action y)
        {
            if (x!.GetType() != y!.GetType()) return false;
            return x.Target == y.Target && x.Maker == y.Maker;
        }

        public int GetHashCode(Action obj)
        {
            return HashCode.Combine(obj.Target, obj.Maker);
        }
    }
}