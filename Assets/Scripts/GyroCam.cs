using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GyroCam : MonoBehaviour {

    public GameObject GyroTarget;

    void Start ()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
	}
	
	void Update ()
    {
        if (Input.gyro.enabled)
        {
            GyroTarget.transform.localRotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        }
    }
}
