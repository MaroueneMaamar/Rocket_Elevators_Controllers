function main(){
    column1 = new Column (1,2,10)
    E1 = new Elevator(1,10);
    E2 = new Elevator(2,10);
    column1.ElevatorsList.push(E1);
    column1.ElevatorsList.push(E2);
    scenario1();
    //scenario2();
    //scenario3();
}

class CallButton{
    constructor (direction, floor){
        this.Direction = direction;
        this.Floor = floor;
    }
}

class Column{
    constructor (id, nbElevators, nbFloors){
        this.Id = id;
        this.CallButtonList = [];
        this.ElevatorsList = [];
        this.FloorsList = [];
        this.NbFloors = nbFloors;
        for (let i=1 ; i<nbFloors+1 ; i++){
            if (i != 1){
                let callButton = new CallButton("Down",i);
                this.CallButtonList.push(callButton);
            }
            if (i != nbFloors+1){
                let callButton = new CallButton("Up",i);
                this.CallButtonList.push(callButton);
            }
            this.FloorsList.push(i);
        }
    }

    RequestElevator(Direction, Floor){
        console.log ("********** REQUEST ELEVATOR **********")
        let MovingElevatorList = [];
        let IdleElevatorList = [];
        for (let i = 0 ; i <= column1.ElevatorsList.length -1; i++ )  {
            let E = column1.ElevatorsList[i];
            if (E.Direction == "Idle"){
                if (E.CurrentFloor == Floor){
                    return E;
                }
                    else {
                        IdleElevatorList.push(E);
                    }
            }
                else if (E.Direction != "Idle") {
                    MovingElevatorList.push(E);
                }
        }
        if (IdleElevatorList.length > 0 ){
            let BestElevator = IdleElevatorList[0];
            let GapBestElevator = Math.abs(BestElevator.CurrentFloor - Floor);
            for (let i = 0; i <= IdleElevatorList.length -1; i++){
                let E = IdleElevatorList[i];
                let Gap = Math.abs(E.CurrentFloor - Floor);
                if (Gap < GapBestElevator) {
                    BestElevator = E
                }
            }
            return BestElevator;
        }
        else if ( MovingElevatorList.length > 0 ) {
            let BestElevator = MovingElevatorList[0];
            let GapBestElevator = Math.abs(BestElevator.RequestList.index(1)-BestElevator.CurrentFloor) + Math.abs(BestElevator.RequestList.index(1)-Floor);
            for (let i=0; i <= MovingElevatorList.length -1 ; i++) {
                let Gap =  Math.abs(E.RequestList.index(1)- E.CurrentFloor) + Math.abs(E.RequestList.index(1)-Floor);
                if (Gap < GapBestElevator) {
                    BestElevator = E
                }
            }
            return BestElevator;
        }
    }

    RequestFloor(E, RequestedFloor){
        console.log("----------- REQUESTED FLOOR = ",RequestedFloor+1," --------------");
        E.AddStop(RequestedFloor);
        E.MoveElevator(E.CurrentFloor, E.RequestList, E.Direction, RequestedFloor);
    }
}

class Elevator{
    constructor (id, nbFloors){
        this.Id= id;
        this.FloorRequestButton= [];
        this.Direction= "Idle";
        this.CurrentFloor= 0;
        this.RequestList= [];
        // initialisation de RequestList a 0
        for (let i=0; i<nbFloors; i++){
           this.RequestList.push(0);
        }
    }

    CloseDoors(){
        console.log("------------ CLOSED DOORS ------------" );
    }

    OpenDoors(){
        console.log("------------ OPEN DOORS ------------" );
    }

    AddStop (floor){
        console.log ("********** ADD STOP **********");
        this.RequestList [floor] = 1;
        console.log ("E",this.Id,"    CurrentFloor = " , this.CurrentFloor+1 ,"    Direction = ",this.Direction,"    " , this.RequestList);
    }

    RemoveStop ( RequestList, currentFloor, Direction){
        console.log ("********** REMOVE STOP **********");
        this.RequestList[currentFloor] = 0 ;   //   Remplacer le 1 de la position actuelle de l'elevateur dans Elevator_Stops par 0
        console.log(this.RequestList);
        this.Direction="Idle";
    }

    MoveElevator ( currentFloor, RequestList, Direction, requestedFloor){
        console.log ("**********  MOVING ELEVATOR E",this.Id,"  TO FLOOR  ", requestedFloor+1,"  **********");
        if (this.Direction == "Idle"){
            if    (currentFloor < requestedFloor) {    this.Direction = "Up"; }
            else if  (currentFloor > requestedFloor) {    this.Direction = "Down"; }
        }
        if (this.Direction =="Up"){
            while (this.RequestList[this.CurrentFloor] != 1) {
                console.log("            E",this.Id,"-- Direction = ",this.Direction,"-- CurrentFloor = ",this.CurrentFloor+1);
                this.CurrentFloor += 1;
            }
        }
        else if (this.Direction == "Down"){
            while (this.RequestList[this.CurrentFloor] != 1) {
                console.log("            E",this.Id,"-- Direction = ",this.Direction,"-- CurrentFloor = ",this.CurrentFloor+1);
                this.CurrentFloor -= 1;
            }
        }
        console.log("            E",this.Id,"-- Direction = ",this.Direction,"-- CurrentFloor = ",this.CurrentFloor+1);
        this.RemoveStop (this.RequestList, this.CurrentFloor, this.Direction);
        this.OpenDoors();
        this.CloseDoors();
    }
}

function scenario1(){
    console.log ("--------------------------- SCENARIO 1 ---------------------------")
    E1.CurrentFloor=1;
    E2.CurrentFloor=5;
    callButton1 = new CallButton("Up", 2);
    console.log ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList);
    console.log ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList);
    console.log ("----- CallButton : ",callButton1.Direction," pressed at Floor : ", callButton1.Floor+1);
    Best_Elevator = column1.RequestElevator(callButton1.Direction , callButton1.Floor);
    console.log ("----- THE BEST ELEVATOR IS =   E" , Best_Elevator.Id);
    Best_Elevator.AddStop(callButton1.Floor);
    Best_Elevator.MoveElevator (Best_Elevator.CurrentFloor , Best_Elevator.RequestList , Best_Elevator.Direction, callButton1.Floor);
    console.log ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList);
    console.log ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList);
    column1.RequestFloor(Best_Elevator,6);
    console.log ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList);
    console.log ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList);
}

function scenario2(){
    console.log ("--------------------------- SCENARIO 2 ---------------------------")
    E1.CurrentFloor=9;
    E2.CurrentFloor=2;
    callButton1 = new CallButton("Up", 0);
    console.log ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList);
    console.log ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList);
    console.log ("----- CallButton : ",callButton1.Direction," pressed at Floor : ", callButton1.Floor+1);
    Best_Elevator = column1.RequestElevator(callButton1.Direction , callButton1.Floor);
    console.log ("-----THE BEST ELEVATOR IS =   E" ,Best_Elevator.Id);
    Best_Elevator.AddStop(callButton1.Floor);
    Best_Elevator.MoveElevator(Best_Elevator.CurrentFloor, Best_Elevator.RequestList, Best_Elevator.Direction, callButton1.Floor);
    console.log ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList);
    console.log ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList);
    column1.RequestFloor(Best_Elevator,5);
    console.log ("----------------------------------- 2 minutes later -------------------------------------------------");

    callButton2 = new CallButton("Up", 2);
    console.log ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList);
    console.log ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList);
    console.log ("----- CallButton : ",callButton2.Direction," pressed at Floor : ", callButton2.Floor+1);
    Best_Elevator = column1.RequestElevator(callButton2.Direction , callButton2.Floor);
    console.log ("-----THE BEST ELEVATOR IS =   E" ,Best_Elevator.Id);
    Best_Elevator.AddStop(callButton2.Floor);
    Best_Elevator.MoveElevator (Best_Elevator.CurrentFloor , Best_Elevator.RequestList , Best_Elevator.Direction, callButton2.Floor);
    console.log ("E 1 CurrentFloor =" , E1.CurrentFloor+1 ,"-- Direction = ",E1.Direction," ----- RequestList E 1 =" , E1.RequestList);
    console.log ("E 2 CurrentFloor =" , E2.CurrentFloor+1 ,"-- Direction = ",E2.Direction," ----- RequestList E 2 =" , E2.RequestList);
    column1.RequestFloor(Best_Elevator,4);

    console.log ("----------------------------------- Finally -------------------------------------------------");

    callButton3 = new CallButton("Down", 8);
    console.log ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList);
    console.log ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList);
    console.log ("----- CallButton : ",callButton3.Direction," pressed at Floor : ", callButton3.Floor+1);
    Best_Elevator = column1.RequestElevator(callButton3.Direction , callButton3.Floor);
    console.log ("-----THE BEST ELEVATOR IS =   E" ,Best_Elevator.Id);
    Best_Elevator.AddStop(callButton3.Floor);
    Best_Elevator.MoveElevator (Best_Elevator.CurrentFloor , Best_Elevator.RequestList , Best_Elevator.Direction, callButton3.Floor);
    console.log ("E 1 CurrentFloor =" , E1.CurrentFloor+1 ,"-- Direction = ",E1.Direction," ----- RequestList E 1 =" , E1.RequestList);
    console.log ("E 2 CurrentFloor =" , E2.CurrentFloor+1 ,"-- Direction = ",E2.Direction," ----- RequestList E 2 =" , E2.RequestList);
    column1.RequestFloor(Best_Elevator,1);
    console.log ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList);
    console.log ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList);
}

function scenario3(){
    console.log ("--------------------------- SCENARIO 3 ---------------------------")
    E1.CurrentFloor=9;
    E2.CurrentFloor=2;
    E2.Direction="Up";
    E2.RequestList[5]=1;
    callButton1 = new CallButton("Down", 2);
    console.log ("--E1 At Floor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList);
    console.log ("--E2 At Floor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList);
    console.log ("----- Button : ",callButton1.Direction," pressed at Floor : ", callButton1.Floor+1);
    Best_Elevator = column1.RequestElevator(callButton1.Direction , callButton1.Floor);
    console.log ("-----THE BEST ELEVATOR IS =   E",Best_Elevator.Id);
    Best_Elevator.AddStop(callButton1.Floor);
    E2.MoveElevator(2,E2.RequestList,"Up",5);
    Best_Elevator.MoveElevator (Best_Elevator.CurrentFloor , Best_Elevator.RequestList , Best_Elevator.Direction, callButton1.Floor);
    console.log ("--E1 At Floor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList);
    console.log ("--E2 At Floor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList);
    column1.RequestFloor(Best_Elevator,1);
    console.log ("----------------------------------- 5 minutes later -------------------------------------------------");
    callButton2 = new CallButton("Down", 9);
    console.log ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList);
    console.log ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList);
    console.log ("----- CallButton : ",callButton2.Direction," pressed at Floor : ", callButton2.Floor+1);
    Best_Elevator = column1.RequestElevator(callButton2.Direction , callButton2.Floor);
    console.log ("-----THE BEST ELEVATOR IS =   E" ,Best_Elevator.Id);
    Best_Elevator.AddStop(callButton2.Floor);
    Best_Elevator.MoveElevator(Best_Elevator.CurrentFloor, Best_Elevator.RequestList, Best_Elevator.Direction, callButton2.Floor);
    console.log ("--E1 CurrentFloor =  " , E1.CurrentFloor+1 ,"  -- Direction =  ",E1.Direction," ----- RequestList E1 = " , E1.RequestList);
    console.log ("--E2 CurrentFloor =  " , E2.CurrentFloor+1 ,"  -- Direction =  ",E2.Direction," ----- RequestList E2 = " , E2.RequestList);
    column1.RequestFloor(Best_Elevator,2);
}

main()