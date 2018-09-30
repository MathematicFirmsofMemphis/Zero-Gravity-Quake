using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
    public Vector3 rotateAround;
    public float speed;
    public float rotX;
    public float rotY;

    // Use this for initialization
    void Start()
    {
        if (!GetComponent<NetworkView>().isMine)
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<NetworkView>().isMine)
        {
            rotX = (2 - Mathf.PingPong(Time.time, 3)) * 3;
            rotY = (1 - Mathf.PingPong(Time.time, 2)) * 6;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(rotX, rotY, 0)), Time.deltaTime * speed);
        }
    }
}
