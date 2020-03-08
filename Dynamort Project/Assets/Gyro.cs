using UnityEngine;

public class Gyro : MonoBehaviour
{
    [Header("Logic")]
    private UnityEngine.Gyroscope gyro;
    private Quaternion rotation;
    private bool gyroPresent; // some phones don't have
    public Quaternion getRotation(){
        return rotation;
    }

    public void EnableGyro(){
        if(gyroPresent) return;
        if(SystemInfo.supportsGyroscope) {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
        gyroPresent = gyro.enabled;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gyroPresent) {
            rotation = gyro.attitude;
        }
    }
}
