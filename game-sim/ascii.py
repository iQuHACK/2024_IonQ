import networkx as nx
from asciicode import graph_to_ascii

class Graph:

    def __init__(self) -> None:
    
        '''
        Creates a graph and returns it
        '''
        self.graph = nx.Graph()
        self.potential_node_dic = {'q0' : ['q1', 'q2'], 'q1' : ['q3', 'q4'], 'q3' : ['q5'], 'q5' : ['q6']}
        self.graph.add_node("q0")
        self.graph.add_node("q1")
        self.graph.add_node("q2")


    def logging(self, log = False):
        '''
        Logs the graph in ascii art
        '''

        ascii_art = graph_to_ascii(self.graph)

        if log:
            print(ascii_art)


    def connect(self, q0, qx):
        '''
        Connects q0 to qx and updates the potential_node_dic
        '''
        self.graph.add_edge(q0, qx)
        
        if qx in self.potential_node_dic.keys():   
            for node in self.potential_node_dic[qx]:
                self.potential_node_dic[q0].append(node)
                self.graph.add_node(node)

        self.potential_node_dic[q0].remove(qx)

  
graphObj = None 
flag = False

def getPotentialNodeDic():
    '''
    Returns the potential node dictionary
    '''
    global graphObj, flag
    if not flag:
        graphObj = Graph()
        flag = True

    return graphObj.potential_node_dic

def setConnection(qx):
    '''
    Sets a connection from q0 to qx
    '''
    graphObj.connect('q0', qx)
    graphObj.logging( True)



