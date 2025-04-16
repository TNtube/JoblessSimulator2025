using System;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public Transform NpcTarget;
    public Renderer ChairRenderer;
    
    public bool IsOccupied = false;

    public float baseLenght = 0;

    private void Awake()
    {
        baseLenght = ChairRenderer.bounds.size.x;
    }
}