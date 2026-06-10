using System;
using UnityEngine;

public class DoorData : MonoBehaviour 
{
    private bool _isDoorUsed;

    [SerializeField]
    private Transform _doorSnapPointPosition;

    [SerializeField]
    private EnumOrientation _doorDefaultFacingDirection;

    [SerializeField]
    private RoomInstance _parentRoom;

    /// <summary>
    /// Getters and Setters
    /// </summary>
    /// 

    public bool GetIsDoorUsed()
    {
        return _isDoorUsed;
    }

    public void SetIsDoorUsed(bool isDoorUsed)
    {
        _isDoorUsed = isDoorUsed;
    }

    public Transform GetDoorSnapPointPosition()
    {
        return _doorSnapPointPosition;
    }

    public EnumOrientation GetDoorFacingDirection()
    {
        int parentRoomOrientationValue = (int)_parentRoom.GetCurrentRoomOrientation(); // Getting the current orientation of the parent room and converting it to an integer value

        EnumOrientation doorWorldOrientation = (EnumOrientation)(((int)_doorDefaultFacingDirection + parentRoomOrientationValue) % 4);

        print($"Parent room value : {parentRoomOrientationValue} " +
            $"| Door default orientation value : {(int)_doorDefaultFacingDirection} " +
            $"| Door World Orientation value : {(int)doorWorldOrientation}");

        return doorWorldOrientation;
    }


}
