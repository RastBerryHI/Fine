using Cinemachine;
using UnityEngine;

public class CameraAttention : MonoBehaviour
{
    public static CameraAttention s_instance;

    [SerializeField] private CinemachineFreeLook _thirdPersonCamera;
    [SerializeField] private Transform _menu;
    [SerializeField] private Transform _vikingTarget;

    private float xAxis, yAxis;

    public void SetVikingTarget()
    {
        _thirdPersonCamera.LookAt = _vikingTarget;
        _thirdPersonCamera.m_XAxis.m_MaxSpeed = xAxis;
        _thirdPersonCamera.m_YAxis.m_MaxSpeed = yAxis;
    }

    public void SetMenuTarget()
    {
        _thirdPersonCamera.LookAt = _menu;
        _thirdPersonCamera.m_XAxis.m_MaxSpeed = 0;
        _thirdPersonCamera.m_YAxis.m_MaxSpeed = 0;
    }

    void Awake()
    {
        xAxis = _thirdPersonCamera.m_XAxis.m_MaxSpeed;
        yAxis = _thirdPersonCamera.m_YAxis.m_MaxSpeed;

        if (s_instance == null)
        {
            s_instance = this;
        }
        else if (s_instance != null)
        {
            Destroy(s_instance.gameObject);
        }
    }
}
