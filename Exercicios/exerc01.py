# -*- coding: utf-8 -*-
"""
Lista de exercícios 1.
"""

import numpy as np
import matplotlib.pyplot as plt
    
from common import question_header as header
import mia

def q01():
    header(1)
    q1 = [123, 116, 122,110, 175, 126, 125, 111, 111, 118, 117]
    print("Amostra ordenada:", sorted(q1))
    print("Média:", np.mean(q1), "\t", "Mediana:", np.median(q1))
    print("A diferença se deve à grande variância, especialmente os valores mais altos.")

def q02():
    header(2)
    print("Sim. ...")

def q03():
    header(3)
    q3 = [23, 60, 79, 32, 57, 74, 52, 70, 82, 36, 80, 77, 81, 95, 41, 65, 92, 85, 
          55, 76, 52, 10, 64, 75, 78, 25, 80, 98, 81, 67, 41, 71, 83, 54, 64, 72, 
          88, 62, 74, 43, 60, 78, 89, 76, 84, 48, 84, 90, 15, 79, 34, 67, 17, 82, 
          69, 74, 63, 80, 85, 61]
    print("Amostra ordenada", sorted(q3))
    # Diagrama de ramos e folhas.
    s_l = mia.StemAndLeaf(q3)
    s_l.print()
    # Frequencias.
    mia.rel_freq(q3, [0, 25, 50, 75, 100])
    # Gráfico de Histograma.
    mia.hist_rel_freq(q3, label="Nota", mu=np.median(q3), sigma=np.std(q3), 
                      bins=(0, 25, 50, 75, 100))
    print ("Média:", np.mean(q3), "\t", "Mediana:", np.median(q3), "\t", 
           "Desvio-padrão:", np.std(q3))
    
def q04():
    header(4)
    q4 = [6.72, 6.77, 6.82, 6.70, 6.78, 6.70, 6.62, 6.75, 6.66, 6.66, 6.64, 
          6.76, 6.73, 6.80, 6.72, 6.76, 6.76, 6.68, 6.66, 6.62, 6.72, 6.76, 
          6.70, 6.78, 6.76, 6.67, 6.70, 6.72, 6.74, 6.81, 6.79, 6.78, 6.66, 
          6.76, 6.76, 6.72]
    print("Amostra ordenada:", sorted(q4))
    print("Média:", np.mean(q4), "\t", "Mediana:", np.median(q4), "\t", 
          "Desvio-padrão:", np.std(q4))
    mia.rel_freq(q4, [6.6, 6.65, 6.7, 6.75, 6.8, 6.85])
    mia.hist_rel_freq(q4, label="Diâmetro", mu=np.median(q4), sigma=np.std(q4), 
                      bins=np.arange(6.6, 6.86, 0.05))

def q05():
    header(5)
    q5 = [3.79, 2.99, 2.77, 2.91, 3.10, 1.84, 2.52, 3.22, 2.45, 2.14, 2.67, 
          2.52, 2.71, 2.75, 3.57, 3.85, 3.36, 2.05, 2.89, 2.83, 3.13, 2.44, 
          2.10, 3.71, 3.14, 3.54, 2.37, 2.68, 3.51, 3.37]
    print ("Média:", np.mean(q5), "\t", "Mediana:", np.median(q5), "\t", 
           "Desvio-padrão:", np.std(q5))
    mia.rel_freq(q5, [1.5, 2., 2.5, 3., 3.5, 4.])
    mia.hist_rel_freq(q5, label="Valor em US$, por aluno", 
                      mu=np.median(q5), sigma=np.std(q5), 
                      bins=np.arange(1.5, 4.1, 0.5))
    mia.StemAndLeaf(q5, 2).print(False)
    
def q06():
    header(6)
    q6 = { 700  : [145, 105, 260, 330],
           1000 : [250, 195, 375, 480],
           1300 : [150, 180, 420, 750] }
    q6mean = {}
    for k, v in q6.items():
        q6mean[k] = np.mean(v)
    plt.plot(list(q6mean.keys()), list(q6mean.values()))
    q6by_spec = {}
    for k, v in q6.items():
        for i in range(1,len(v) + 1):
            if not (i in q6by_spec):
                q6by_spec[i] = []
            q6by_spec[i].append(v[i - 1])
    for i in q6by_spec.keys():
        plt.plot(list(q6.keys()), q6by_spec[i])
