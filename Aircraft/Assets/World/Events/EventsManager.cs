using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance { get; private set; }

    public InputEvents InputEvents;
    public PlayerEvents PlayerEvents;
    public GoldEvents GoldEvents;
    public MiscEvents MiscEvents;
    public QuestEvents QuestEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        Instance = this;

        // initialize all events
        InputEvents = new InputEvents();
        PlayerEvents = new PlayerEvents();
        GoldEvents = new GoldEvents();
        MiscEvents = new MiscEvents();
        QuestEvents = new QuestEvents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            InputEvents.QuestLogTogglePressed();
        }
    }
}
