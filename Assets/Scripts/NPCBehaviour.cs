using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");

    [SerializeField]
    private Animator animator;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator.SetFloat(WalkSpeed, 0.5f);
        
    }

    public void SetTarget(Vector3 target)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
