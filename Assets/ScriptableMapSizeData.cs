using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableMapSizeData", menuName = "Scriptable Objects/ScriptableMapSizeData")]
public class ScriptableMapSizeData : ScriptableObject
{
    public int MinimumAmoutOfRooms;

    public int MaximumAmoutOfRooms;

    //In the future, map size will affact how many types of rooms there are, and how many enemies there are in the map. For now, it only affacts the amount of rooms.
}
