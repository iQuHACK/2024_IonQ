using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private GameObject SingleGateUI;
    [SerializeField] private StageObject stage;

    public List<string> duckGates = new List<string> {};

    public int gateCount;
    private int qID;
    void Start()
    {
        gateCount = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Gate g;
        qID = gameObject.GetComponent<PlayerMovement>().qubitID;
        float curr_state = gameObject.GetComponent<PlayerMovement>().state;
        string coll_tag = collision.gameObject.tag;
        Debug.Log(coll_tag);
        if((coll_tag == "CX-Gate") || (coll_tag == "CZ-Gate")) {
            Debug.Log("Add gate");
            gateCount++;
            AddDoubleGateUIElement(collision.gameObject);
            g = new Gate(collision.gameObject.tag, qID - 1, qID);
            stage.gate_list.Add(g);
            Debug.Log(g.ToString());
            float qID0State = StageObject.qubit_array[qID - 1].GetComponent<PlayerMovement>().state;
            stage.ChangeState(qID, (curr_state + qID0State) % 2);
            stage.entangled_qubits1.Add(qID - 1);
            stage.entangled_qubits1.Add(qID);
            Destroy(collision.gameObject);
        } else if((coll_tag == "Water") || (coll_tag == "Lava") || (coll_tag == "Measure")) {
            Debug.Log("Execute measure");
            gateCount++;
            AddSingleGateUIElement(collision.gameObject);
            g = new Gate(collision.gameObject.tag, qID);
            stage.gate_list.Add(g);
            Debug.Log(g.ToString());
            stage.Measure();
            if(gameObject.GetComponent<PlayerMovement>().state == collision.gameObject.GetComponent<Finish>().targetState)
            {
                CompleteLevel();
            }
        } else {
            Debug.Log("Add gate");
            gateCount++;
            AddSingleGateUIElement(collision.gameObject);
            g = new Gate(collision.gameObject.tag, qID);
            stage.gate_list.Add(g);
            Debug.Log(g.ToString());
            if(coll_tag == "X-Gate") {
                stage.ChangeState(gameObject, Math.Abs(((1f-curr_state)+2)%2));
            } else if(coll_tag == "H-Gate") {
                stage.ChangeState(gameObject, Math.Abs((float)((0.5f-curr_state)+2)%2));
            } else if(coll_tag == "Z-Gate") {
                stage.ChangeState(gameObject, (((curr_state-1)*-1) + 1) % 2);
            }
            Destroy(collision.gameObject);
        }
    }

    void AddSingleGateUIElement(GameObject gate_obj)
    {
        Sprite gate_sprite = gate_obj.GetComponent<SpriteRenderer>().sprite;
        Vector2 pos = new Vector2(-280 + gateCount*50,-30 - 40 * qID);
        GameObject uiElement = Instantiate(SingleGateUI, pos, Quaternion.identity);
        uiElement.transform.SetParent(GameObject.Find("Canvas").transform, false);
        Image gateImage = uiElement.GetComponent<Image>();
        gateImage.sprite = gate_sprite;
    }

    void AddDoubleGateUIElement(GameObject gate_obj) {
        // stub
    }

    public void EmptyCircuit()
    {
        Transform canvasTransform = GameObject.Find("Canvas").transform;

        // Loop through all child objects of the canvas and destroy them
        foreach (Transform child in canvasTransform)
        if (child.name != "CircuitBackground")
        {
            if (child.name != "Button")
            {
            Destroy(child.gameObject);

            }
            // Destroy the child object
        }
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
