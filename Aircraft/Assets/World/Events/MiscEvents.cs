using System;

public class MiscEvents
{
    public event Action OnCoinCollected;
    public event Action OnGemCollected;

    public void CoinCollected() 
    {
        OnCoinCollected?.Invoke();
    }

    public void GemCollected() 
    {
        OnGemCollected?.Invoke();
    }
}
