using UnityEngine;
using System.IO; // File
using System.Linq; // byte array concat
using UnityEngine.SceneManagement;
using TMPro;

public class levelGenerator : MonoBehaviour
{
    public string filePath = "/Levels/4.txt";
    public Texture2D texture;
    public GameObject startingPoint, block, Battery;
    public Camera CameraMain;
    public SwipeInput _SwipeInput;
    public RectTransform UIbar;
    [Header("One Battery")]
    public GameObject BatteryGameOject;
    [Header("One Starting Point")]
    public GameObject startGameObject;
    void Start() {
        storeLevelDataToFile(texture, Application.dataPath + filePath);
        loadLevel(PersistentData.SelectedLevelObject);
    }
    private void storeLevelDataToFile(Texture2D tex, string path) {
        byte[] dimensions = new byte[]{(byte)(tex.width), (byte)(tex.height)};
        File.WriteAllBytes(path, (tex.GetRawTextureData()).Concat(dimensions).ToArray());
    }
    private void loadLevel(LevelData levelObj){
        _SwipeInput.setOriginForNewLevel(getLevelDataFromFile(levelObj.LevelBytes));
        
    }
    private Vector2 getLevelDataFromFile(byte[] levelBytes){
        byte width = (byte)(levelBytes[levelBytes.Length-2]);
        byte height = (byte)(levelBytes[levelBytes.Length-1]);
        Vector2 start = Vector2.zero;
        for(int i = 0; i < height; i++) {
            for(int j = 0; j < width; j++) {
                int index = (width * i * 3 + (j * 3));
                print(width + " * " + i + ": " + width * i + " j: " + j + " index: " + index);
                if (levelBytes[index] == 0xff) { Instantiate(block, new Vector2(j*10, i*10),Quaternion.identity); } // red means block
                else if (levelBytes[index] == 0xbf) {
                    BatteryGameOject.transform.position = new Vector2(j*10, i*10);
                }
                else if (levelBytes[index] == 0x7f) {
                    start = new Vector2(j*10, i*10 + 5);
                    startGameObject.transform.position = start;
                }
            }
        }
        centerCameraOnPuzzle(width, height);
        return start;
    }
    private void centerCameraOnPuzzle(byte w, byte h){
        w *= 10;
        h *= 10;
        print("UIbar.rect.height: " + UIbar.rect.height);
        float screenH = Screen.height - UIbar.rect.height; // 125 for top UI bar
        float screenRatio = (float)Screen.width / screenH; 
        float targetRatio = w / h;
        CameraMain.transform.position = new Vector3(w/2 - 5,(h + (h - h*screenH/Screen.height))/2,CameraMain.transform.position.z); // -5 to account for scale of blocks(10)
        if (screenRatio >= targetRatio) { CameraMain.orthographicSize = h/2; }
        else { CameraMain.orthographicSize = h / 2 * targetRatio / screenRatio; }
    }
    
}
