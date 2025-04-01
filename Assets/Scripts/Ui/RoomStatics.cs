using UnityEngine;

[System.Serializable]
public class RoomStatics
{
    [TextArea] public string DescriptionRoom;

    public int LevelRoom;
    public int PriseLevelRoom;
    public bool IsOpen;
    public bool IsMedicalUpgrade;
    public bool IsTrainingUpgrade;
    public bool IsCraftingUpgrade;
    public GameObject DoorRoom;
}