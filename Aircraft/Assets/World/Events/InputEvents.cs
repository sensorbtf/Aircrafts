using UnityEngine;
using System;

public class InputEvents
{
    public event Action OnSubmitPressed;
    public event Action OnQuestLogTogglePressed;

    public void SubmitPressed()
    {
        OnSubmitPressed?.Invoke();
    }

    public void QuestLogTogglePressed()
    {
        OnQuestLogTogglePressed?.Invoke();
    }
}
