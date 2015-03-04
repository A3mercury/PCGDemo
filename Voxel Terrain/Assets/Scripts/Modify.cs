using UnityEngine;
using System.Collections;

public class Modify : MonoBehaviour
{

    Vector2 rot;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
        else if (Input.GetKey(KeyCode.S))
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        else if (Input.GetKey(KeyCode.A))
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        else if (Input.GetKey(KeyCode.D))
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);



        rot = new Vector2(
            rot.x + Input.GetAxis("Mouse X") * 3,
            rot.y + Input.GetAxis("Mouse Y") * 3);

        transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

        //transform.position += transform.forward * Input.GetAxis("Vertical");
        //transform.position += transform.right * Input.GetAxis("Horizontal");
    }
}