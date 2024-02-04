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

qc = QuantumCircuit(2, 2)
qc.h(0)
qc.cx(0,1)

qc.measure([0,1],[0,1])

backend = BasicSimulator()
tqc = transpile(qc, backend)

num_list = []
for i in range(0, 100):
    counts = backend.run(tqc, shots=196).result()
    result = counts.get_counts() #how many 00 and how many 11
    difference = result.get("00") - result.get("11")
    if difference <= 24:
        num_list.append(difference)

#normal distribution --> stdev is 7 (a perfect fifth)

def convert_to_note(difference):
    return 440 * pow(2.0, (1/12)*difference)

arr = []
for count in num_list:
    arr.append(convert_to_note(count))

print(arr)

def get_sine_wave(frequency, duration, sample_rate=44100, amplitude=4096):
    t = np.linspace(0, duration, int(sample_rate*duration))
    wave = amplitude*np.sin(2.0*np.pi*frequency*t)
    return wave

wave=[]
for t in arr:
    ampl = np.iinfo(np.int16).max
    new_wave = get_sine_wave(t, duration = random.choice([0.25, 0.5, 0.75, 1]), amplitude = ampl)
    wave = np.concatenate((wave, new_wave))
open("random_noise.wav", "w")
wavfile.write('random_noise.wav', rate=44100, data=wave.astype(np.int16))

