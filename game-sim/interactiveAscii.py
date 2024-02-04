
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

statusCode = 404

def askUser(list_of_scenarios, list_of_names, n_times = 1, cooldown = 0):

    global statusCode, graphObj
    '''
    Asks the user for input
    '''

    userInput = ""

    for i in range(n_times):

        statusCode = 404

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
            graphObj.logging(True)
            cooldown = 3
            statusCode = 200
            
        elif (userInput == "n"):
            print("You chose to stay")
            graphObj.logging(True)
            cooldown = cooldown - 1 if cooldown > 0 else 0
            statusCode = 200

        else:
            print("You cannot make a choice yet")
            graphObj.logging(True)
            cooldown = cooldown - 1 if cooldown > 0 else 0
            statusCode = 200

def getStatusCode():
    '''
    Returns the status code
    '''
    return statusCode
    


list_of_scenarios = SCENARIOS[:]
list_of_names = SCENARIOS_NAMES[:]
cooldown = 0

getPotentialNodeDic()
startScreen()
askUser(list_of_scenarios, list_of_names, 13)
