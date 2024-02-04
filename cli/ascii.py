

import networkx as nx
from asciicode import graph_to_ascii

def createGraph():
    '''
    Creates a graph and returns it
    '''
    return nx.Graph()


def initGraph(graph):
    '''
    Initializes the graph with the nodes and returns a dictionary of potential nodes
    '''

    potential_node_dic = {'q0' : ['q1', 'q2'], 'q1' : ['q3', 'q4']}

    graph.add_node("q0")
    graph.add_node("q1")
    graph.add_node("q2")

    return potential_node_dic


def logging(graph, log = False):
    '''
    Logs the graph in ascii art
    '''

    ascii_art = graph_to_ascii(graph)

    if log:
        print(ascii_art)


def getPotentialNodeDic():
    '''
    Returns the potential node dictionary
    '''

    return potential_node_dic


def connect(q0, qx, graph, potential_node_dic):
    '''
    Connects q0 to qx and updates the potential_node_dic
    '''
    graph.add_edge(q0, qx)
    
    if qx in potential_node_dic.keys():   
        for node in potential_node_dic[qx]:
            potential_node_dic[q0].append(node)
            graph.add_node(node)

    potential_node_dic[q0].remove(qx)

def setConnection(qx):
    '''
    Sets a connection from q0 to qx
    '''

    connect('q0', qx, graph, potential_node_dic)
    logging(graph, True)


# MAIN
graph = createGraph()
potential_node_dic = initGraph(graph)


## TESTING (MODULAR)
# print(getPotentialNodeDic())
# setConnection('q1')
# print(getPotentialNodeDic())

