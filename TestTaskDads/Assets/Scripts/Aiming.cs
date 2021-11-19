using UnityEngine;

public class Aiming : MonoBehaviour
{
    [SerializeField] private float _turnSpeed = 15;
    private new Camera camera;

    void Awake()
    {
        camera = Camera.main;
    }

    void FixedUpdate()
    {
        float yaw = camera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yaw, 0), _turnSpeed * Time.fixedDeltaTime);
    }
}
