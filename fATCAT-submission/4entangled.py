import qiskit
from qiskit import QuantumCircuit
from qiskit import transpile, QuantumRegister, ClassicalRegister, QuantumCircuit
from qiskit_aer import AerSimulator as Aer
from qiskit.providers.basic_provider import BasicSimulator
import numpy as np
import matplotlib as mpl
import matplotlib.pyplot as plt
from scipy.io import wavfile
import mpmath
import random

# in the film, we have 4 entangled qubits. because they are entangled, a 
# measurement can only result in two possibilities. we can encode all possible
# measurements as binary integers which then correspond to musical notes; with 
# only two possibilities, their music only ends up with two musical notes.
qc = QuantumCircuit(4, 4)
qc.h(0)
qc.cx(0, 1)
qc.cx(1, 2)
qc.cx(2,3)
qc.measure([0,1,2,3],[0,1,2,3])

backend = BasicSimulator()
tqc = transpile(qc, backend)
counts = backend.run(tqc, shots=100, memory=True).result().get_memory()

def convert_to_note(binary):
    return 220 * pow(2.0, (1/12)*(int(binary, 2)))
    
arr = []
for count in counts:
    arr.append(convert_to_note(count))

def get_sine_wave(frequency, duration, sample_rate=44100, amplitude=4096):
    t = np.linspace(0, duration, int(sample_rate*duration))
    wave = amplitude*np.sin(2.0*np.pi*frequency*t)
    return wave

wave=[]
for t in arr:
    ampl = np.iinfo(np.int16).max
    new_wave = get_sine_wave(t, duration = random.choice([0.25, 0.5, 0.75, 1]), amplitude = ampl)
    wave = np.concatenate((wave, new_wave))
open("4entangled.wav", "w")
wavfile.write('4entangled.wav', rate=44100, data=wave.astype(np.int16))