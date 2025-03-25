using UnityEngine;

[System.Serializable]
public class RoomStatics
{
    [TextArea] public string DescriptionRoom;

    public int LevelRoom;
    public bool IsOpen;
    public bool IsMedicalUpgrade;
    public bool IsTrainingUpgrade;
    public GameObject DoorRoom;
}