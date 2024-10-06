using Resources;
using Resources.Scripts;
using System;

public class MiscEvents
{
    public event Action<ResourceInUnit> OnResourceCollected;
    public event Action OnGemCollected;

    public void ResourceCollected(ResourceInUnit p_resource) 
    {
        OnResourceCollected?.Invoke(p_resource);
    }

    public void GemCollected() 
    {
        OnGemCollected?.Invoke();
    }
}
