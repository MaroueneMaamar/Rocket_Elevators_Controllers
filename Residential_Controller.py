def main():
    column1.ElevatorsList.append(E1)
    column1.ElevatorsList.append(E2)
    scenario1()
    #scenario2()
    #scenario3()

class CallButton:
    def __init__(self, direction, floor):
        self.Direction = direction
        self.Floor = floor

class Column:
    def __init__(self, id, nbElevators, nbFloors):
        self.Id = id
        self.CallButtonList = []
        self.ElevatorsList = []
        self.FloorsList = []
        self.NbFloors = nbFloors
        for i in range( 1 , nbFloors+1 ):
            if i != 1:
                callButton = CallButton("Down",i)
            if i != nbFloors+1:
                callButton = CallButton("Up",i)
            self.CallButtonList.append(callButton)
            self.FloorsList.append(i)

    def RequestElevator(self, Direction, Floor):
        print ("********** REQUEST ELEVATOR **********")
        Best_Elevator = column1.ElevatorsList[0]
        for i in ( 1 , len(column1.ElevatorsList)-1) :
            E = column1.ElevatorsList[i]
            if (E.Direction == Direction)or(E.Direction == "Idle"):                                 #  Meme Direction OU Elevator en Repos
                if Direction== "Up" :
                    if Floor == E.CurrentFloor :                                                    #  Elevator&User dans la meme position
                        return E
                    elif (Floor > E.CurrentFloor)and(Floor > Best_Elevator.CurrentFloor):           #  Elevator/Best_Elevator - User
                        Best_Elevator = E
                    elif (Floor < E.CurrentFloor)and(Floor < Best_Elevator.CurrentFloor):           #  User - Elevator/Best_Elevator
                        Best_Elevator = E
                    elif (Floor > E.CurrentFloor) and (Floor < Best_Elevator.CurrentFloor) :        #  Ordre: Elevator - User - Best_Elevator
                        Best_Elevator = E
                    return Best_Elevator
                elif Direction == "Down":
                    if (Floor == E.CurrentFloor):                                                   #  Elevator&User dans la meme position
                        return E
                    elif (Floor < E.CurrentFloor) and (Floor < Best_Elevator.CurrentFloor) :        #  User - Elevator/Best_Elevator
                        Best_Elevator = E
                    elif (Floor > E.CurrentFloor) and (Floor > Best_Elevator.CurrentFloor):         #  Elevator/Best_Elevator - User
                        Best_Elevator = E
                    elif (Floor < E.CurrentFloor) and (Floor > Best_Elevator.CurrentFloor):         #  Ordre: Best_Elevator - User - Elevator
                        Best_Elevator = E
                    return Best_Elevator
            else:
                return Best_Elevator

    def RequestFloor(self, Elevator, RequestedFloor):
        print("----------- REQUESTED FLOOR = ",RequestedFloor+1," --------------")
        Elevator.AddStop(RequestedFloor)
        Elevator.MoveElevator(Elevator.CurrentFloor, Elevator.RequestList, Elevator.Direction, RequestedFloor)

class Elevator:
    def __init__(self, id, nbFloors):
        self.Id= id
        self.FloorRequestButton= []
        self.Direction= "Idle"
        self.CurrentFloor= 0
        self.RequestList= []
        # initialisation de RequestList a 0
        for i in range(nbFloors):
            self.RequestList.append(0)

    def CloseDoors(self):
        print("------------ CLOSED DOORS ------------" )

    def OpenDoors(self):
        print("------------ OPEN DOORS ------------" )

    def AddStop (self, floor):
        print ("********** ADD STOP **********")
        self.RequestList [floor] = 1
        print ("E",self.Id,"    CurrentFloor = " , self.CurrentFloor+1 ,"    Direction = ",self.Direction,"    " , self.RequestList)

    def RemoveStop (self, RequestList, currentFloor, Direction):
        print ("********** REMOVE STOP **********")
        self.RequestList[currentFloor] = 0    #   Remplacer le 1 de la position actuelle de l'elevateur dans Elevator_Stops par 0
        print(self.RequestList)
        self.Direction="Idle"

    def MoveElevator (self, currentFloor, RequestList, Direction, requestedFloor):
        print ("**********  MOVING ELEVATOR E",self.Id,"  TO FLOOR  ", requestedFloor+1,"  **********")
        if self.Direction == "Idle" :
            if    currentFloor < requestedFloor :    self.Direction = "Up"
            elif  currentFloor > requestedFloor :    self.Direction = "Down"
        if self.Direction =="Up":
            while self.RequestList[self.CurrentFloor] != 1 :
                print("            E",self.Id,"-- Direction = ",self.Direction,"-- CurrentFloor = ",self.CurrentFloor+1)
                self.CurrentFloor += 1
        elif self.Direction == "Down":
            while self.RequestList[self.CurrentFloor] != 1 :
                print("            E",self.Id,"-- Direction = ",self.Direction,"-- CurrentFloor = ",self.CurrentFloor+1)
                self.CurrentFloor -= 1
        print("            E",self.Id,"-- Direction = ",self.Direction,"-- CurrentFloor = ",self.CurrentFloor+1)
        self.RemoveStop (self.RequestList, self.CurrentFloor, self.Direction)
        self.OpenDoors()
        self.CloseDoors()

def scenario1():
    print ("--------------------------- SCENARIO 1 ---------------------------")
    E1.CurrentFloor=1
    E2.CurrentFloor=5
    callButton1 = CallButton("Up", 2)
    print ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList)
    print ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList)
    print ("----- CallButton : ",callButton1.Direction," pressed at Floor : ", callButton1.Floor+1)
    Best_Elevator = column1.RequestElevator(callButton1.Direction , callButton1.Floor)
    print ("----- THE BEST ELEVATOR IS =   E" , Best_Elevator.Id)
    Best_Elevator.AddStop(callButton1.Floor)
    Best_Elevator.MoveElevator (Best_Elevator.CurrentFloor , Best_Elevator.RequestList , Best_Elevator.Direction, callButton1.Floor)
    print ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList)
    print ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList)
    column1.RequestFloor(Best_Elevator,6)
    print ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList)
    print ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList)

def scenario2():
    print ("--------------------------- SCENARIO 2 ---------------------------")
    E1.CurrentFloor=9
    E2.CurrentFloor=2
    callButton1 = CallButton("Up", 0)
    print ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList)
    print ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList)
    print ("----- CallButton : ",callButton1.Direction," pressed at Floor : ", callButton1.Floor+1)
    Best_Elevator = column1.RequestElevator(callButton1.Direction , callButton1.Floor)
    print ("-----THE BEST ELEVATOR IS =   E" ,Best_Elevator.Id)
    Best_Elevator.AddStop(callButton1.Floor)
    Best_Elevator.MoveElevator(Best_Elevator.CurrentFloor, Best_Elevator.RequestList, Best_Elevator.Direction, callButton1.Floor)
    print ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList)
    print ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList)
    column1.RequestFloor(Best_Elevator,5)
    print ("----------------------------------- 2 minutes later -------------------------------------------------")
    callButton2 = CallButton("Up", 2)
    print ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList)
    print ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList)
    print ("----- CallButton : ",callButton2.Direction," pressed at Floor : ", callButton2.Floor+1)
    Best_Elevator = column1.RequestElevator(callButton2.Direction , callButton2.Floor)
    print ("-----THE BEST ELEVATOR IS =   E" ,Best_Elevator.Id)
    Best_Elevator.AddStop(callButton2.Floor)
    Best_Elevator.MoveElevator (Best_Elevator.CurrentFloor , Best_Elevator.RequestList , Best_Elevator.Direction, callButton2.Floor)
    print ("E 1 CurrentFloor =" , E1.CurrentFloor+1 ,"-- Direction = ",E1.Direction," ----- RequestList E 1 =" , E1.RequestList)
    print ("E 2 CurrentFloor =" , E2.CurrentFloor+1 ,"-- Direction = ",E2.Direction," ----- RequestList E 2 =" , E2.RequestList)
    column1.RequestFloor(Best_Elevator,4)
    print ("----------------------------------- Finally -------------------------------------------------")
    callButton3 = CallButton("Down", 8)
    print ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList)
    print ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList)
    print ("----- CallButton : ",callButton3.Direction," pressed at Floor : ", callButton3.Floor+1)
    Best_Elevator = column1.RequestElevator(callButton3.Direction , callButton3.Floor)
    print ("-----THE BEST ELEVATOR IS =   E" ,Best_Elevator.Id)
    Best_Elevator.AddStop(callButton3.Floor)
    Best_Elevator.MoveElevator (Best_Elevator.CurrentFloor , Best_Elevator.RequestList , Best_Elevator.Direction, callButton3.Floor)
    print ("E 1 CurrentFloor =" , E1.CurrentFloor+1 ,"-- Direction = ",E1.Direction," ----- RequestList E 1 =" , E1.RequestList)
    print ("E 2 CurrentFloor =" , E2.CurrentFloor+1 ,"-- Direction = ",E2.Direction," ----- RequestList E 2 =" , E2.RequestList)
    column1.RequestFloor(Best_Elevator,1)
    print ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList)
    print ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList)

def scenario3():
    print ("--------------------------- SCENARIO 3 ---------------------------")
    E1.CurrentFloor=9
    E2.CurrentFloor=2
    E2.Direction="Up"
    E2.RequestList[5]=1
    print(E2.RequestList)
    callButton1 = CallButton("Down", 2)
    print ("--E1 At Floor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList)
    print ("--E2 At Floor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList)
    print ("----- Button : ",callButton1.Direction," pressed at Floor : ", callButton1.Floor+1)
    Best_Elevator = column1.RequestElevator(callButton1.Direction , callButton1.Floor)
    print ("-----THE BEST ELEVATOR IS =   E",Best_Elevator.Id)
    Best_Elevator.AddStop(callButton1.Floor)
    E2.MoveElevator(2,E2.RequestList,"Up",5)
    Best_Elevator.MoveElevator (Best_Elevator.CurrentFloor , Best_Elevator.RequestList , Best_Elevator.Direction, callButton1.Floor)
    print ("--E1 At Floor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList)
    print ("--E2 At Floor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList)
    column1.RequestFloor(Best_Elevator,1)
    print ("----------------------------------- 5 minutes later -------------------------------------------------")
    callButton2 = CallButton("Down", 9)
    print ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList)
    print ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList)
    print ("----- CallButton : ",callButton2.Direction," pressed at Floor : ", callButton2.Floor+1)
    Best_Elevator = column1.RequestElevator(callButton2.Direction , callButton2.Floor)
    print ("-----THE BEST ELEVATOR IS =   E" ,Best_Elevator.Id)
    Best_Elevator.AddStop(callButton2.Floor)
    Best_Elevator.MoveElevator(Best_Elevator.CurrentFloor, Best_Elevator.RequestList, Best_Elevator.Direction, callButton2.Floor)
    print ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList)
    print ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList)
    column1.RequestFloor(Best_Elevator,2)

column1 = Column(1,2,10)
E1 = Elevator(1,10)
E2 = Elevator(2,10)
main()