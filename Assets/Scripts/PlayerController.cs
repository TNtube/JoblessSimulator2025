using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private Text m_currentHeightText;
    [SerializeField] private Text m_currentStateText;
    [SerializeField] private Text m_onChairText;
    
    [SerializeField] private float m_standingHeight;
    [SerializeField] private float m_crouchingHeight;
    [SerializeField] private float m_crouchingThreshold;

    private bool m_isOnChair = false;
    
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
        return GetHeight() < m_crouchingThreshold && IsOnChair();
    }

    private bool IsOnChair()
    {
        return m_isOnChair;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chair"))
            m_isOnChair = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Chair"))
            m_isOnChair = false;
    }

    private void Update()
    {
        if (IsSitting())
            Debug.Log("Sitting.");

        if (m_currentHeightText) m_currentHeightText.text = GetHeight().ToString();
        if (m_currentStateText) m_currentStateText.text = IsSitting() ? "Crouching" : "Standing";
        if (m_onChairText) m_onChairText.text = IsOnChair() ? "Yes" : "No";
    }
}
