
using System.Collections.Generic;
using UnityEngine;  

public class MapGenerator : MonoBehaviour
{
    public static event System.Action<EnumOrientation, RoomInstance> OnOrientationChanged;  


    [SerializeField]
    private GameObject[] _availableRooms;

    [SerializeField]
    private MapGenerationSize _mapGenerationSize;

    [SerializeField]
    private ScriptableMapSizeData _smallMapData;

    [SerializeField]
    private ScriptableMapSizeData _mediumMapData;

    [SerializeField]
    private ScriptableMapSizeData _largeMapData;

    private int _roomsToBeGenerated;

    [SerializeField]
    private List<RoomInstance> _generatedRoomsList = new List<RoomInstance>(); //This list will hold the instances of the rooms that are generated. It can be used to keep track of the rooms and their positions, and to check for overlaps or other issues during generation.

    private EnumOrientation tempNewOrientation;

    private int roomIdCounter = 0; // Counter to assign unique IDs to rooms

    private void Awake()
    {
        SelectGenerationSize();
        GeneratingStartingRoom();
        MapGenerationProcess();
    }

    #region Initialization Methods
    private void GeneratingStartingRoom() // This method will be responsible for generating the starting room of the map
    {
        int randomRoomIndex = Random.Range(0, _availableRooms.Length); // Randomly selecting a room from the available rooms array : Predefined Starting room will be added in the future

        GameObject roomObject = Instantiate(_availableRooms[randomRoomIndex], new Vector3(0f, 0f, 0f), GetRandomRotation()); // Creating the first room in the center of the map

        if (roomObject.TryGetComponent<RoomInstance>(out RoomInstance newRoomInstance))
        {
            CreatedARoom(newRoomInstance);
        }

    }

    private void SelectGenerationSize()
    {
        int generatedNumber = Random.Range(0, 3); // Randomly selecting a number between 0 and 2 to determine the map size

        _mapGenerationSize = (MapGenerationSize)generatedNumber; // Casting the generated number to the MapGenerationSize enum

        switch (_mapGenerationSize)
        {
            case MapGenerationSize.Small:

                //print("Small Map Selected");

                _roomsToBeGenerated = Random.Range(_smallMapData.MinimumAmoutOfRooms, _smallMapData.MaximumAmoutOfRooms + 1); // Randomly selecting the amount of rooms to be generated based on the small map data
                break;

            case MapGenerationSize.Medium:

                //print("Medium Map Selected");

                _roomsToBeGenerated = Random.Range(_mediumMapData.MinimumAmoutOfRooms, _mediumMapData.MaximumAmoutOfRooms + 1); // Randomly selecting the amount of rooms to be generated based on the medium map data
                break;

            case MapGenerationSize.Large:

                //print("Large Map Selected");

                _roomsToBeGenerated = Random.Range(_largeMapData.MinimumAmoutOfRooms, _largeMapData.MaximumAmoutOfRooms + 1); // Randomly selecting the amount of rooms to be generated based on the large map data
                break;
        }
    }

    #endregion

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 

    #region Rotation and Direction Methods
    private float GenerateRandomDirection()
    {
        int randomDirection = Random.Range(0, 4);                                   // Randomly selecting a number between 0 and 3 to determine the direction of the new room
        EnumOrientation newRoomDirection = (EnumOrientation)randomDirection;        // Casting the generated number to the EnumOrientation enum
        //print("New Room Direction: " + newRoomDirection);                         // Printing the selected direction for debugging purposes

        float rotationAngle = 0f;                                                   // Initializing the rotation angle variable

        switch (newRoomDirection)
        {
            case EnumOrientation.North:
                rotationAngle = 0f;                                             // No rotation needed for north direction
                break;

            case EnumOrientation.East:
                rotationAngle = 90f;                                            // No rotation needed for north direction
                break;       
                
            case EnumOrientation.South:
                rotationAngle = 180f;                                           // No rotation needed for north direction
                break;
                
            case EnumOrientation.West:
                rotationAngle = 270f;                                           // No rotation needed for north direction
                break;
        }

        tempNewOrientation = newRoomDirection;
                                
        return rotationAngle;
    }

    private Quaternion GetRandomRotation()
    {
       Vector3 newRot = new Vector3(-90f, 0f, GenerateRandomDirection());        // Returning a Vector3 with the Y rotation set to the randomly generated direction

        return Quaternion.Euler(newRot);                                         // Converting the Vector3 to a Quaternion and returning it
    }

    #endregion

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 

    #region Room Generation Methods
    private void MapGenerationProcess()
    {
    
            if (_roomsToBeGenerated == 0)
            {
                print("All rooms have been generated");
                return;
            }

            int randomRoomIndex = Random.Range(0, _generatedRoomsList.Count);
            // Randomly selecting a room from the available rooms array -> this serves as the base room for the new room to be generated from

            int randomDoorIndex = Random.Range(0, _generatedRoomsList[randomRoomIndex].GetAvailableDoors().Length);
            // Randomly selecting a door from the selected room -> this serves as the base door for the new room to be generated from

            RoomInstance tempRoomInstance = _generatedRoomsList[randomRoomIndex]; // Storing the selected room instance in a temporary variable
            DoorData tempDoorData = tempRoomInstance.GetAvailableDoors()[randomDoorIndex]; // Storing the selected door data in a temporary variable

            GenerateRooms(tempRoomInstance, tempDoorData);

            //print("Selected Room: " + tempRoomInstance.name + ", Selected Door: " + tempDoorData.name + " Facing: " + tempDoorData.GetDoorFacingDirection()); // Printing the selected room and door for debugging purposes
     
    }

    private void GenerateRooms(RoomInstance baseRoom, DoorData baseDoor)
    {
        int randomRoomIndex = Random.Range(0, _availableRooms.Length); // Randomly selecting a room from the available rooms array
        
        Vector3 baseDoorSnapPoint = baseDoor.GetDoorSnapPointPosition().position; 

        GameObject newRoomObject = Instantiate(_availableRooms[randomRoomIndex], baseDoorSnapPoint, GetRandomRotation()); 

        if (newRoomObject.TryGetComponent<RoomInstance>(out RoomInstance newRoomInstance))
        {
            CreatedARoom(newRoomInstance);

            LookForMatchingDoor(newRoomObject, newRoomInstance, baseDoor);
        }


    }
    
    private void LookForMatchingDoor(GameObject newRoomObject, RoomInstance newRoomInstance, DoorData baseDoor)
    {
        DoorData[] newRoomDoorsRef = newRoomInstance.GetAvailableDoors();

        EnumOrientation orientationToMatch = OrientationHelper.GetOppositeOrientation(baseDoor.GetDoorFacingDirection()); // Getting the opposite orientation of the base door to determine which door in the new room should be used for alignment

        //print($"Door selected : {baseDoor} from room : {baseRoom.GetRoomID()}");

        //print($"Orientation to match : {orientationToMatch}");

        bool matchingDoorFound = false;

        DoorData matchingDoor = null;

        for (int i = 0; i < newRoomDoorsRef.Length; i++)
        {

            if (newRoomDoorsRef[i].GetDoorFacingDirection() == orientationToMatch)
            {
                //print($"Found a matching door : {newRoomDoorsRef[i].name}. facing : {newRoomDoorsRef[i].GetDoorFacingDirection()}");

                print($"Matching door found : {newRoomDoorsRef[i].name} in room : {newRoomInstance.GetRoomID()}");
                print($"Door direction : {newRoomDoorsRef[i].GetDoorFacingDirection()}");

                matchingDoor = newRoomDoorsRef[i];

                matchingDoorFound = true;

                break;

            }
        }

        if (matchingDoorFound)
        {
            print("Matching door found -> Need to start adjusting room placement");

            AdjustRoomPlacement(newRoomObject, matchingDoor);

        }
        else
        {
            print("No door matches -> need to rotate the room");
        }

    }

    private void AdjustRoomPlacement(GameObject newRoomObject, DoorData matchingDoor)
    {
        float originSnapDiffX = newRoomObject.transform.position.x - matchingDoor.GetDoorFramePosition().position.x; 
        float originSnapDiffZ = newRoomObject.transform.position.z - matchingDoor.GetDoorFramePosition().position.z; 

        newRoomObject.transform.position = new Vector3(newRoomObject.transform.position.x + originSnapDiffX, newRoomObject.transform.position.y, newRoomObject.transform.position.z + originSnapDiffZ); 
    }
    private void CreatedARoom(RoomInstance newRoom)
    { 

        OnOrientationChanged?.Invoke(tempNewOrientation, newRoom); 

        _generatedRoomsList.Add(newRoom); // Adding the newly created room to the list of generated rooms

        _roomsToBeGenerated--; // Decreasing the amount of rooms to be generated by 1 every time a room is created

        foreach (DoorData doors in newRoom.GetAvailableDoors())
        {
            print($"Room : {newRoom.GetRoomID()} has {doors.name} facing {doors.GetDoorFacingDirection()} available"); // Printing the available doors of the newly created room for debugging purposes
        }

    }

    #endregion
    public enum MapGenerationSize // For now, this will only control the number of rooms generated. 
    {
        Small,
        Medium,
        Large
    }

    public static class OrientationHelper
    {

        public static EnumOrientation GetOppositeOrientation(EnumOrientation orientation)
        {
            return (EnumOrientation)(((int)orientation + 2) % 4); // Adding 2 to the current orientation and using modulo 4 to get the opposite orientation
        }

    }
}
