using System;
using UnityEngine;

public class MainGameEventsManager : MonoBehaviour
{
    public static MainGameEventsManager Instance { get; private set; }

    public InputEvents inputEvents;
    public PlayerEvents playerEvents;
    public GoldEvents goldEvents;
    public MiscEvents miscEvents;
    public QuestEvents questEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        Instance = this;

        // initialize all events
        inputEvents = new InputEvents();
        playerEvents = new PlayerEvents();
        goldEvents = new GoldEvents();
        miscEvents = new MiscEvents();
        questEvents = new QuestEvents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inputEvents.QuestLogTogglePressed();
        }
    }
}
