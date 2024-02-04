using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public struct Gate {
    public Gate(string gate, int qubit1, int qubit2 = -1) {
        GateType = gate;
        Qubit1 = qubit1;
        Qubit2 = qubit2;
    }

    public string GateType {get;}
    public int Qubit1 {get;}
    public int Qubit2 {get;}

    public override string ToString() => $"({GateType}, {Qubit1}, {Qubit2})";
}

// public struct Duck {

// }

public class StageObject : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Gate> gate_list = new List<Gate>();
    [SerializeField] private int qubitNum;
    public static GameObject[] qubit_array;

    public List<int> entangled_qubits1 = new List<int>();
    void Start()
    {
        qubit_array = new GameObject[qubitNum];
        gate_list.Clear();
        qubit_array = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < qubit_array.Length; i++) { 
            UnityEngine.Debug.Log(qubit_array[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeState(int id, float rot) { 
        PlayerMovement pm = qubit_array[id].GetComponent<PlayerMovement>();
        ExecuteCircuit ec = qubit_array[id].GetComponent<ExecuteCircuit>();
        UnityEngine.Debug.Log(rot);
        UnityEngine.Debug.Log("TEST");
        pm.state = rot;
        ec.UpdateColor(rot);
        // if(entangled_qubits1.Contains(id)) {
        //     for(int i = 0; i < entangled_qubits1.Count; i++) {
        //         if(entangled_qubits1[i] != id) {
        //             pm = qubit_array[entangled_qubits1[i]].GetComponent<PlayerMovement>();
        //             ec = qubit_array[entangled_qubits1[i]].GetComponent<ExecuteCircuit>();
        //             pm.state += rot;
        //             ec.UpdateColor(pm.state);
        //         }
        //     }
        // }
    }

    public void ChangeState(GameObject duck, float rot) {
        PlayerMovement pm = duck.GetComponent<PlayerMovement>();
        ExecuteCircuit ec = duck.GetComponent<ExecuteCircuit>();
        pm.state = rot;
        ec.UpdateColor(rot);
        // if(entangled_qubits1.Contains(id)) {
        //     for(int i = 0; i < entangled_qubits1.Count; i++) {
        //         if(entangled_qubits1[i] != id) {
        //             pm = qubit_array[entangled_qubits1[i]].GetComponent<PlayerMovement>();
        //             ec = qubit_array[entangled_qubits1[i]].GetComponent<ExecuteCircuit>();
        //             pm.state += rot;
        //             ec.UpdateColor(pm.state);
        //         }
        //     }
        // }
    }

    public List<int> Measure() {
        string base_string = @"from qiskit import *;
def runCircuit():
    simulator = Aer.get_backend('qasm_simulator');";
        base_string += $"circuit = QuantumCircuit({qubitNum}, {qubitNum})\n";
        for(int i = 0; i < gate_list.Count; i++) {
            if(gate_list[i].GateType == "X-Gate") {
                base_string += $"    circuit.x({gate_list[i].Qubit1});\n";
            } else if(gate_list[i].GateType == "Z-Gate") {
                base_string += $"    circuit.z({gate_list[i].Qubit1});\n";
            } else if(gate_list[i].GateType == "CX-Gate") {
                base_string += $"    circuit.cx({gate_list[i].Qubit1}, {gate_list[i].Qubit2});\n";
            } else if(gate_list[i].GateType == "CZ-Gate") {
                base_string += $"    circuit.cz({gate_list[i].Qubit1}, {gate_list[i].Qubit2});\n";
            } else if(gate_list[i].GateType == "H-Gate") {
                base_string += $"    circuit.h({gate_list[i].Qubit1});\n";
            } else if(gate_list[i].GateType == "Measure") {
                base_string += $"    circuit.measure(range({qubitNum}), range({qubitNum}));\n";
            }
        }
        base_string += "    return list(execute(circuit, backend = simulator, shots = 1).result().get_counts().keys())[0];\n";
        // base_string += "print runCircuit()";
        base_string += @"f = open('c:/Users/benku/unity projects/ducks-international/unity/My project/circ_output.txt', 'w')
f.write(str(runCircuit()))
f.close()";
        File.WriteAllText("./test.py", base_string);
        
        using(System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
        {
            pProcess.StartInfo.FileName = @"C:/Python312/python.exe";
            pProcess.StartInfo.Arguments = "\"c:/Users/benku/unity projects/ducks-international/unity/My project/test.py\""; //argument
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows
            pProcess.Start();
            string output = pProcess.StandardOutput.ReadToEnd(); //The output result
            pProcess.WaitForExit();
        }

        string qubitState = File.ReadAllText("c:/Users/benku/unity projects/ducks-international/unity/My project/circ_output.txt");
        UnityEngine.Debug.Log(qubitState);

        int int_state = Int32.Parse(qubitState);

        // if(int_state == 1) {
        //     return true;
        // }
        // return false;

        List<int> measured_states = new List<int>();

        if(int_state == 0) {
            for(int i = 0; i < qubitNum; i++) {
                measured_states.Add(0);
            }
        } else {
            for(int i = 0; int_state > 0; i++) {
                measured_states.Add(int_state % 10);
                int_state /= 10;
            }
        }

        UnityEngine.Debug.Log(measured_states);
        for(int i = 0; i < measured_states.Count; i++) {

            // ExecuteCircuit ec = qubit_array[i].GetComponent<ExecuteCircuit>();
            ChangeState(i, measured_states[i]);
            // ec.UpdateColor((float)measured_states[i]);
            UnityEngine.Debug.Log(measured_states[i]);
        }

        return measured_states;
    }
}
