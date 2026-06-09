using UnityEngine;

public class RoomInstance : MonoBehaviour
{
    [SerializeField]
    private ScribtableRoomData _roomData;

    [SerializeField]
    private DoorData[] _availableDoors;

    public DoorData[] GetAvailableDoors() { return _availableDoors; }
}
