# import qiskit
from qiskit import QuantumCircuit, QuantumRegister, ClassicalRegister
from qiskit import Aer, execute
from qiskit.circuit.library import QFT
from qiskit.quantum_info import Statevector
from qiskit.visualization import plot_bloch_multivector

from qiskit_ionq import IonQProvider 

#Call provider and set token value
# provider = IonQProvider(token='EDEq7Meo9Re0MIVV2loVBe2hZJCUG4VY')

# numpy
import numpy as np

# plotting
import matplotlib.pyplot as plt

player = QuantumRegister(1, name='player')
n_starting_people = 6

network = QuantumRegister(n_starting_people, name='network')

qc = QuantumCircuit(player, network)

# creates random network starting states
def randomized_network_state(n):
    temp = QuantumCircuit(n)
    for i in range(n):
        theta = np.random.rand() * np.pi
        phi = np.random.rand() * np.pi
        temp.rx(theta, i)
        temp.ry(phi, i)
    return temp

# gives the people in the network the random starting positions
q2 = randomized_network_state(n_starting_people)
q2.draw()
qc.compose(q2, np.arange(1, n_starting_people+1), inplace=True)
    
qc.draw(output='mpl')
plot_bloch_multivector(qc)

epochs = 10
for i in range (epochs):
    s = input('interact with:')

# while True: