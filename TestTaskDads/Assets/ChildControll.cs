using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildControll : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, gameObject.transform.rotation.y * -1f, 0);
    }
}
