# -*- coding: utf-8 -*-
"""
Created on Mon Feb 27 09:53:23 2017

@author: Jonas Luz Jr. <jonasluzjr@edu.unifor.br>
"""

import matplotlib.pyplot as plt
import matplotlib.mlab as mlab

def hist_rel_freq(a, mu=None, sigma=None, label="Valor", bins=None):
    n, bins, patches = plt.hist(a, normed=True, bins=bins)
    plt.grid = True
    if mu != None and sigma != None:
        y = mlab.normpdf(bins, mu, sigma)
        plt.plot(bins, y, 'r--', linewidth=1)
    plt.title("Distribuição")
    plt.xlabel(label)
    plt.ylabel("Frequencia")
    #plt.axis([0, 100, 0, 0.5])
    plt.show()    
    
class FrequencyData:
    """
    Estrutura de dados para frequencias
    """
    def __init__(self, start=None):
        self.start  = start
        self.end    = None
        self.median = None
        self.freq   = 0
        self.rfreq  = 0
        
    def calc_median(self):
        if self.start != None and self.end != None:
            self.start, self.end = float(self.start), float(self.end)
            self.median = (self.start + self.end) / 2

    def calc_rfreq(self, total):
        if self.median == None:
            self.calc_median()
        if self.median != None:
            self.rfreq = self.freq / total
            
    def add(self, increment=1):
        self.freq = self.freq + increment
        
def rel_freq(a, bins):
    freqs = []
    freq  = FrequencyData()
    for b in bins:
        if freq.start != None:
            freq.end = b
            freq.calc_median()
            freqs.append(freq)
            freq = FrequencyData()
        freq.start = b
    i = 0
    data = sorted(a)
    for f in freqs:
        total = len(data)
        while i < total and data[i] < f.end and data[i] >= f.start:
            f.add()
            i = i + 1
        f.calc_rfreq(total)

    # Imprime a tabela de dados.
    print("Freqs", "médio", "Ocorrências", "f", sep="\t\t")
    for f in freqs:
        print ("{:.4} a {:.4}".format(f.start, f.end), "{:.4}".format(f.median), 
               f.freq, "{:.4}".format(f.rfreq), sep="\t\t")
    print("Totais:", len(a))
    
class StemAndLeaf:
    
    def __init__(self, a, margin=1):
        """ 
        Gera gráfico de ramos-e-folhas
        """
        self.data       = {}
        self.total      = len(a)
        self.maxcolsize = 0
        
        # Ordenar a amostra.
        data            = sorted(a)
            
        # Para cada valor da amostra, extrair os ramos e folhas correspondentes.
        current_stem    = ''
        current_leaves  = []

        for v in data:
            value = str(v)              # converter em string.
            stem = value[:margin]       # extrai o ramo.
            leaf = value[margin:]       # extrai a folha.
            # Se o ramo ainda é o corrente...
            if stem == current_stem: 
                # ... adiciona a folha na lista do ramo.
                current_leaves.append(leaf)
            else:
                # atribui a lista de folhas ao ramo, se não for vazio.
                if current_stem != "":
                    self.data[current_stem] = current_leaves
                    leaves_size = len(current_leaves)
                    if (leaves_size > self.maxcolsize) :
                        self.maxcolsize = leaves_size
                # reinicializa a lista com a folha atual e... 
                current_leaves = [leaf]
                # marca o novo ramo como o corrente.
                current_stem = stem
        self.data[current_stem] = current_leaves
    
    def print(self, with_freq = True):
        """
        Plota o gráfico de ramo-e-folhas
        """
        
        # Monta cabeçalho do gráfico.
        freq_distance = self.maxcolsize - len("Folhas")
        header = ["Ramos", "Folhas" + " " * freq_distance]
        if with_freq:
            header = header + ["Frequencia", "Frequencia relativa"]
        else:
            header = header + ["", ""]
        print("%s\t%s\t%s\t%s" % tuple(header))
        
        # Para cada ramo, imprime suas folhas e frequências, se for o caso.
        for k, v in self.data.items():
            print (k, "\t", end="") # valor do ramo.
            for l in v:
                print (l, end=" ")   # valor da folha.
            if with_freq: 
                # calcula e imprime a frequência absoluta e relativa.
                print(" " * (freq_distance - len(v)), "\t", len(v), "\t" * 2, len(v)/self.total)
            else:
                print()             # termina a linha.
        print("\nTotal:\t", self.total) # imprime total.