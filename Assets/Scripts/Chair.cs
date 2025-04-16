using UnityEngine;

public class Chair : MonoBehaviour
{
    public Transform NpcTarget;
    public Renderer ChairRenderer;
    
    public bool IsOccupied = false;

    public float baseLenght = 0;

    public void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        if(other.GetComponentInParent<PlayerController>().IsSitting())
            IsOccupied = true;
        else
            IsOccupied = false;
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            IsOccupied = false;
    }

    private void Awake()
    {
        baseLenght = ChairRenderer.bounds.size.x;
    }
}