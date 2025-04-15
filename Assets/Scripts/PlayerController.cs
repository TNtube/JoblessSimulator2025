using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    
    [SerializeField] private float m_standingHeight;
    [SerializeField] private float m_crouchingHeight;
    [SerializeField] private float m_crouchingThreshold;

    public void CalibrateStandingHeight()
    {
        m_standingHeight = m_camera.transform.position.y;
        ComputeCrouchingThreshold();
    }

    public void CalibrateCrouchingHeight()
    {
        m_crouchingHeight = m_camera.transform.position.y;
        ComputeCrouchingThreshold();
    }

    private void ComputeCrouchingThreshold()
    {
        m_crouchingThreshold = (m_standingHeight + m_crouchingHeight) / 2;
    }

    public bool IsSitting()
    {
        return m_camera.transform.position.y < m_crouchingThreshold;
    }

    private void Update()
    {
        if (IsSitting())
            Debug.Log("Sitting.x");
    }
}
