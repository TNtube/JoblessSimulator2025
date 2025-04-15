using System;
using System.Collections.Generic;
using UnityEngine;

public class ChairsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chairPrefab;
    
    [SerializeField]
    private int chairsNumber = 5;
    
    
    List<GameObject> _chairs = new List<GameObject>();
    List<NPCBehaviour> _npcs = new List<NPCBehaviour>();
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateChairs();
        UpdateNPCsTarget();
    }
    
    
    void UpdateChairs()
    {
        if (_chairs.Count < chairsNumber)
        {
            for (var i = _chairs.Count; i < chairsNumber; i++)
            {
                _chairs.Add(Instantiate(chairPrefab, transform));
            }
        }
        else if (_chairs.Count > chairsNumber)
        {
            for (var i = _chairs.Count - 1; i >= chairsNumber; i--)
            {
                Destroy(_chairs[i]);
                _chairs.RemoveAt(i);
            }
        }
        
        var angle = 360 / chairsNumber;
        var baseAngle = (180 - angle) / 2;

        int index = 0;
        foreach (var chair in _chairs)
        {
            var bounds = chair.GetComponent<Renderer>().bounds;
            var b = bounds.size.x;
            var side = b * Mathf.Sin(Mathf.Deg2Rad * baseAngle) / Mathf.Sin(Mathf.Deg2Rad * angle);
            var area = 0.5f * b * side * Mathf.Sin(Mathf.Deg2Rad * baseAngle);
            var depth = 2 * area / b;
            
            var rotation = Quaternion.Euler(0, angle * index, 0);
            var direction = rotation * Vector3.forward;
            
            chair.transform.LookAt(transform.position + direction * 100);
            chair.transform.position = transform.position + direction.normalized * (depth + depth / 3.0f); // magic offset
            index++;
        }
    }

    void UpdateNPCsTarget()
    {
        var slots = chairsNumber + 1;
        var angle = 360 / slots;
        var depth = Vector3.Distance(transform.position, _chairs[0].transform.position);

        int index = 0;
        foreach (var chair in _chairs)
        {
            var rotation = Quaternion.Euler(0, angle * index, 0);
            var direction = rotation * Vector3.forward;
            
            chair.transform.LookAt(transform.position + direction * 100);
            chair.transform.position = transform.position + direction.normalized * (depth + depth / 3.0f); // magic offset
            index++;
        }
    }

    private float _angle = 0.0f;
    
    public float testDepth = 5.0f;
    public float moveSpeed = 1.0f;

    private void OnDrawGizmos()
    {
        var angle = 360 / chairsNumber;
        
        // angle += (moveSpeed / (radius * Mathf.PI * 2.0f)) * Time.deltaTime
        
        var depth = testDepth;
        _angle += moveSpeed / (testDepth * Mathf.PI * 2.0f) * Time.deltaTime;
        
        // draw line from center to angle
        for (var i = 0; i < chairsNumber; i++)
        {
            var rotation = Quaternion.Euler(0, angle * i + _angle, 0);
            var direction = rotation * Vector3.forward;


            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + direction * depth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
