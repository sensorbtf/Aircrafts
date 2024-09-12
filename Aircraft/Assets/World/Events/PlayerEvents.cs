using System;

public class PlayerEvents
{
    public event Action OnDisablePlayerMovement;
    public event Action OnEnablePlayerMovement;
    public event Action<int> OnPlayerLevelChange;
    public event Action<int> OnPlayerExperienceChange;
    public event Action<int> OnExperienceGained;

    public void DisablePlayerMovement()
    {
        OnDisablePlayerMovement?.Invoke();
    }

    public void EnablePlayerMovement()
    {
        OnEnablePlayerMovement?.Invoke();
    }

    public void ExperienceGained(int experience) 
    {
        OnExperienceGained?.Invoke(experience);
    }

    public void PlayerLevelChange(int level) 
    {
        OnPlayerLevelChange?.Invoke(level);
    }

    public void PlayerExperienceChange(int experience) 
    {
        OnPlayerExperienceChange?.Invoke(experience);
    }
}
