using UnityEngine;
using System;
using Unity.VisualScripting;

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
