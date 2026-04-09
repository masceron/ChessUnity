using Cysharp.Threading.Tasks;

namespace UX.UI.Toolkit.Common
{
    public enum InGameMenuType
    {
        ChrysosShop,
        ChrysosShopItem,
        ThalassosShop,
        ThalassosItem,
    }
    
    public interface IAwaitableUI<in TPayload, TResult>
    {
        UniTask<TResult> WaitForSelection(TPayload payload);

        void Confirm(TResult result);
        void Cancel();
    }
}