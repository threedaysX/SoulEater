using System;
public class RequestManager : Singleton<RequestManager>
{
    public event Action OnRequestComplete;
    public void requestCompleted()
    {
        if (OnRequestComplete != null)
            OnRequestComplete();
    }
}
