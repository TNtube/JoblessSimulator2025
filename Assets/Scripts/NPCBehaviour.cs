using System;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");
    private static readonly int Sit1 = Animator.StringToHash("Sit");

    [SerializeField]
    private Animator animator;
    
    public ChairsManager chairsManager;
    
    Vector3 _target;

    [SerializeField]
    private float animationSpeed = .7f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator.SetFloat(WalkSpeed, animationSpeed);
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

    public void Sit()
    {
        animator.SetTrigger(Sit1);
    }
}
