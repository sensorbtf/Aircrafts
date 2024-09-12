using System;

public class GoldEvents
{
    public event Action<int> OnGoldGained;
    public event Action<int> OnGoldChange;

    public void GoldGained(int gold)
    {
        OnGoldGained?.Invoke(gold);
    }

    public void GoldChange(int gold)
    {
        OnGoldChange?.Invoke(gold);
    }
}
