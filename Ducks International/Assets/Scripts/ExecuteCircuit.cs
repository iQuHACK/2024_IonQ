using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.SearchService;

public class ExecuteCircuit : MonoBehaviour
{

    [SerializeField] private StageObject stage;
    // Start is called before the first frame update
    public int state = 0;
    void Start()
    {
        UpdateColor(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // state = Execute();
            // UpdateColor();
            Gate g = new Gate("Measure", 0);
            stage.gate_list.Add(g);
            stage.Measure();
            ItemCollector ICRef = GetComponent<ItemCollector>();
            ICRef.gateCount = 0;
            ICRef.EmptyCircuit();   
        }
    }

    private int Execute()
    {
        ItemCollector ICRef = GetComponent<ItemCollector>();
        if (ICRef.gateCount > 0)
        {
            List<string> gates = ICRef.duckGates;
            ICRef.duckGates = new List<string> {};
            ICRef.gateCount = 0;
            ICRef.EmptyCircuit();

            System.Random random = new System.Random();
            int randomValue = random.Next(2);
            return randomValue;
        }
        
        return state;

    }

    public void UpdateColor(float x)
    {   
        Color c = new Color(1-Math.Abs(x-1), 0, Math.Abs(x-1));
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = c;
    }
}
