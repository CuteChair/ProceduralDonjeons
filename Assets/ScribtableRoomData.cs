using UnityEngine;

[CreateAssetMenu(fileName = "ScribtableRoomData", menuName = "Scriptable Objects/ScribtableRoomData")]
public class ScribtableRoomData : ScriptableObject
{
    public RoomType roomType;

    public EnumOrientation DefaultRoomOrientation;
}

public enum RoomType
{
   Enter,
   Exit,
   Small,
   Medium,
   Large,
   Hallway 
}
