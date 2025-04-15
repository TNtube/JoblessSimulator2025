using System;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");

    [SerializeField]
    private Animator animator;
    
    public ChairsManager chairsManager;
    
    Vector3 _target;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator.SetFloat(WalkSpeed, .7f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_target, 0.1f);
    }

    public void SetTarget(Vector3 target)
    {
        _target.y = 0;
        _target = target;
    }

    // Update is called once per frame
    void Update()
    {
        // move to the target
        transform.LookAt(_target);
        transform.position = _target;
    }
}
