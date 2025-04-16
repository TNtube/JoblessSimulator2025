using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ChairsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chairPrefab;

    [SerializeField]
    private GameObject npcPrefab;
    
    [SerializeField]
    private int chairsNumber = 5;
    
    public float walkingSpeed = 1f;
    
    List<Chair> _chairs = new ();
    public List<Chair> Chairs => _chairs;
    List<NPCBehaviour> _npcs = new ();
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateChairs(true);
        UpdateNPCsTarget();
    }

    public void RemoveOneChair(Chair chair)
    {
        chairsNumber -= 1;
        _chairs.Remove(chair);
        Destroy(chair.gameObject);
        UpdateChairs();
        
        // todo: start next round
    }
    
    
    void UpdateChairs(bool init = false)
    {
        if (_chairs.Count < chairsNumber)
        {
            for (var i = _chairs.Count; i < chairsNumber; i++)
            {
                _chairs.Add(Instantiate(chairPrefab, transform).GetComponent<Chair>());
            }
        }
        
        var angle = 360 / chairsNumber;
        var baseAngle = (180 - angle) / 2;

        int index = 0;
        foreach (var chair in _chairs)
        {
            var b = chair.baseLenght;
            var side = b * Mathf.Sin(Mathf.Deg2Rad * baseAngle) / Mathf.Sin(Mathf.Deg2Rad * angle);
            var area = 0.5f * b * side * Mathf.Sin(Mathf.Deg2Rad * baseAngle);
            var depth = 2 * area / b;
            
            var rotation = Quaternion.Euler(0, angle * index, 0);
            var direction = rotation * Vector3.forward;

            if (init)
            {
                chair.transform.LookAt(transform.position + direction * 100);
                chair.transform.position = transform.position + direction.normalized * (depth + depth / 3.0f); // magic offset
            }
            else
            {
                chair.transform.DODynamicLookAt(transform.position + direction * 100, 0.3f);
                chair.transform.DOMove(transform.position + direction.normalized * (depth + depth / 3.0f), 1f);

            }
            index++;
        }
    }

    void UpdateNPCsTarget()
    {
        var position = transform.position;
        position.y = 0;
        while(_npcs.Count < chairsNumber)
        {
            var behaviour = Instantiate(npcPrefab, position, transform.rotation).GetComponent<NPCBehaviour>();
            behaviour.chairsManager = this;
            _npcs.Add(behaviour);
        }
        
        while (_npcs.Count > chairsNumber)
            _npcs.RemoveAt(_npcs.Count - 1);
        
        UpdateNPCsPosition(true);
    }

    private float _angle = 0;

    void UpdateNPCsPosition(bool setPosition = false)
    {
        if (_npcs.Any(npc => npc.Sitting || npc.TryingToSit))
            return;
        
        var slots = chairsNumber + 1;
        var angle = 360 / slots;
        var depth = Vector3.Distance(transform.position, _chairs[0].transform.position);

        _angle += (walkingSpeed / (depth+0.1f * Mathf.PI * 2.0f)) * Time.deltaTime;
        // Debug.Log("angle is " + _angle);
        // Debug.Log("depth is " + depth);

        int index = 0;
        foreach (var npc in _npcs)
        {
            var rotation = Quaternion.Euler(0, angle * index + _angle, 0);
            var direction = rotation * Vector3.forward;

            var nextPos = transform.position + direction.normalized * (depth + 1); // magic offset
            nextPos.y = 0;
            if (setPosition)
                npc.transform.position = nextPos;
            else
                npc.SetTarget(nextPos);

            index++;
        }
    }

    private void OnDrawGizmos()
    {
        var angle = 360 / chairsNumber;
        
        // angle += (moveSpeed / (radius * Mathf.PI * 2.0f)) * Time.deltaTime
        
        // var depth = testDepth;
        // _angle += moveSpeed / (testDepth * Mathf.PI * 2.0f) * Time.deltaTime;
        
        // draw line from center to angle
        for (var i = 0; i < chairsNumber; i++)
        {
            var rotation = Quaternion.Euler(0, angle * i, 0);
            var direction = rotation * Vector3.forward;


            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + direction * 5);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNPCsPosition();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var firstNpc = _npcs[0];
            // get the closest chair index to the first npc
            var closestChairIndex = -1;
            var closestChairDistance = float.MaxValue;
            for (var i = 0; i < _chairs.Count; i++)
            {
                var distance = Vector3.Distance(firstNpc.transform.position, _chairs[i].transform.position);
                if (distance < closestChairDistance)
                {
                    closestChairDistance = distance;
                    closestChairIndex = i;
                }
            }
            
            
            foreach (var npc in _npcs)
            {
                if (npc.Lost) continue;
                npc.StartSitting(_chairs[closestChairIndex]);
                closestChairIndex++;
                if (closestChairIndex >= _chairs.Count)
                    closestChairIndex = 0;
            }
        }
    }
}
