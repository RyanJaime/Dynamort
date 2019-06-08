using UnityEngine;

public static class PersistentData
{
    private static int selectedLevel = 0;
    private static LevelData selectedLevelObject;
    public static int SelectedLevel{
        get { return selectedLevel; }
        set { selectedLevel = value; }
    }
    public static LevelData SelectedLevelObject{
        get { return selectedLevelObject; }
        set { selectedLevelObject = value; }
    }
}
