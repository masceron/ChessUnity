using Cysharp.Threading.Tasks;

namespace UX.UI.Toolkit.Common
{
    public enum InGameMenuType
    {
        ChrysosShop    
    }
    
    public interface IAwaitableUI<TResult, in TPayload>
    {
        UniTask<TResult> WaitForSelection(TPayload payload);
        
        void Close();
    }
}