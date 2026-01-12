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
        Success = 0, // Action thành công
        Blocked = 1, // Shield thành công chặn ăn quân
        HardenedBlock = 2, // Hardened shield chặn
        Parry = 3, // Carapace chặn
        Miss = 4, // Blinded thành công làm quân ăn trượt
        Evade = 5, // Evasion chặn
        Incorruptible = 6, //(Trait) Sanity (Aug), Metal Mind thành công chặn hiệu ứng
        Untouchable = 7, // (Aug) Metal Spine, (Aug) Metal Regulator, (Trait) Extremophile, Thành công chặn hiệu ứng
        Unshaken = 8, // (Trait) Free Movement, (Aug) Cold Blooded, Thành công chặn hiệu ứng
        Muted = 9, //(Aug) Crown of Silience thành công chặn Commander địch sử dụng Skill.
        CantApplyEffect = 10, // Không thể áp dụng effect nói chung
        EffectResistance = 11, // Sử dụng trong trường hợp Kháng Effect
        SurvivedHit = 12, // bị ăn nhưng không chết
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