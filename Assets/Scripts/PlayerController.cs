using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private Text m_currentHeightText;
    [SerializeField] private Text m_currentStateText;
    
    [SerializeField] private float m_standingHeight;
    [SerializeField] private float m_crouchingHeight;
    [SerializeField] private float m_crouchingThreshold;

    public void CalibrateStandingHeight()
    {
        m_standingHeight = GetHeight();
        ComputeCrouchingThreshold();
    }

    public void CalibrateCrouchingHeight()
    {
        m_crouchingHeight = GetHeight();
        ComputeCrouchingThreshold();
    }

    private void ComputeCrouchingThreshold()
    {
        m_crouchingThreshold = (m_standingHeight + m_crouchingHeight) / 2;
    }

    private float GetHeight() => m_camera.transform.position.y;
    
    public bool IsSitting()
    {
        return m_camera.transform.position.y < m_crouchingThreshold;
    }
    
    private void NotUpdate()
    {
        if (IsSitting())
            Debug.Log("Sitting.x");

        m_currentHeightText.text = GetHeight().ToString();
        m_currentStateText.text = IsSitting() ? "Crouching" : "Standing";
    }
}
