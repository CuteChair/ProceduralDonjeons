using Unity.VisualScripting;
using UnityEngine;

public class RoomInstance : MonoBehaviour
{
    [SerializeField]
    private ScribtableRoomData _roomData;

    [SerializeField]
    private EnumOrientation _currentRoomOrientation;

    [SerializeField]
    private DoorData[] _availableDoors;

    public static int RoomIDCounter;

    [SerializeField]
    private int _roomID;

    public DoorData[] GetAvailableDoors() { return _availableDoors; }

    public EnumOrientation GetCurrentRoomOrientation() { return _currentRoomOrientation; }

    private void Awake()
    {
        _currentRoomOrientation = _roomData.DefaultRoomOrientation;

        RoomIDCounter++; // Incrementing the static RoomIDCounter to assign a unique ID to each room instance

        _roomID = RoomIDCounter; // Assigning the current value of RoomIDCounter to the _roomID field of the room instance
    }

    private void OnEnable()
    {
        MapGenerator.OnOrientationChanged += OnChangedRoomOrientation; // Subscribing to the OnOrientationChanged event from the MapGenerator class
    }

    private void OnDisable()
    {
        MapGenerator.OnOrientationChanged -= OnChangedRoomOrientation; // Unsubscribing from the OnOrientationChanged event to prevent memory leaks and unintended behavior when the room instance is disabled or destroyed
    }
    public void OnChangedRoomOrientation(EnumOrientation newOrientation, RoomInstance currentRoom)
    {
        if (currentRoom != this)
        {
            return;
        }
            _currentRoomOrientation = newOrientation; // Updating the current room orientation to the new orientation received from the event
    }

    public int GetRoomID() => _roomID;
}
