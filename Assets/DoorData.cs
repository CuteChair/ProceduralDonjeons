using UnityEngine;

public class DoorData : MonoBehaviour 
{
    private bool _isDoorUsed;

    [SerializeField]
    private Transform _doorSnapPointPosition;

    [SerializeField]
    private EnumOrientation _doorFacingDirection;

    /// <summary>
    /// Getters and Setters
    /// </summary>
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

    public EnumOrientation GetDoorFacingDirection() => _doorFacingDirection;
}
