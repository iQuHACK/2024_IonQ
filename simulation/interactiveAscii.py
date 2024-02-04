
from constants import *
from ascii import *
import networkx as nx
from asciicode import graph_to_ascii
import random


def startScreen():
    '''
    Prints the start screen of the game
    '''

    print(START)
    print("\n\n------------------------------------------------------------------------------------ \n")
    print("------------------------------------------------------------------------------------ \n")


def printScenarios(list_of_scenarios, list_of_names):
    '''
    Prints the scenarios and removes the chosen scenario from the list
    '''

    rand = random.randint(0, len(list_of_scenarios)-1)
    ret_scenario, ret_name = list_of_scenarios[rand], list_of_names[rand]
    list_of_scenarios.pop(rand)
    list_of_names.pop(rand)
    return ret_scenario, ret_name



def askUser(list_of_scenarios, list_of_names, n_times = 1, cooldown = 0):
    '''
    Asks the user for input
    '''

    userInput = ""

    for i in range(n_times):

        if cooldown == 0:
            scenario, name = printScenarios(list_of_scenarios, list_of_names)
            potential_node_dic = getPotentialNodeDic()
            print("Current potential connections: ", potential_node_dic["q0"])
            print("\n\n")
            print(scenario)
            print("\n\n")
                
            userInput = input("Enter your choice (y/n): ")

        if (userInput == "y" and cooldown == 0):
            potential_node_dic = getPotentialNodeDic()
            potential_node_dic["q0"].append(name)
            logging(graph, True)
            cooldown = 3
            
        elif (userInput == "n"):
            print("You chose to stay")
            logging(graph, True)
            cooldown = cooldown - 1 if cooldown > 0 else 0

        else:
            print("You cannot make a choice yet")
            logging(graph, True)
            cooldown = cooldown - 1 if cooldown > 0 else 0

            

list_of_scenarios = SCENARIOS[:]
list_of_names = SCENARIOS_NAMES[:]
cooldown = 0
startScreen()
askUser(list_of_scenarios, list_of_names, 13)
