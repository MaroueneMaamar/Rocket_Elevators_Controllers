package main

import (
	"fmt"
)

type Battery struct {
	ID, NbColumns, NbFloors, NbBasements, NbElevators int
	ListColumns                                       []Column
}

type Column struct {
	ID                                 int
	ListElevators                      []Elevator
	ListFloors                         []int
	NbFloors, NbBasements, NbElevators int
}

type Elevator struct {
	ID, ElevatorCurrentFloor, ElevatorNextStop, Gap int
	Direction                                       string
}

func newBattery(id, nbColumns, nbFloors, nbBasements, nbElevators int) *Battery {
	b := Battery{}
	b.ID = id
	b.NbBasements = nbBasements
	b.NbFloors = nbFloors
	b.NbColumns = nbColumns
	b.NbElevators = nbElevators
	// Create ListColumns
	for i := 1; i <= b.NbColumns; i++ {
		c := Column{ID: i, NbElevators: b.NbElevators, NbFloors: b.NbFloors, NbBasements: b.NbBasements}
		b.ListColumns = append(b.ListColumns, c)
	}

	// Create ListFloors and ListElevators for each column
	avgFloors := int((b.NbFloors - b.NbBasements) / (b.NbColumns - 1))
	nbFilledColumns := 0
	minFloor := 2
	maxFloor := avgFloors
	for x, col := range b.ListColumns {

		if col.ID == 1 {
			b.ListColumns[x].ListFloors = append(b.ListColumns[x].ListFloors, 1)
			for i := -1; i >= -col.NbBasements; i-- {
				b.ListColumns[x].ListFloors = append(b.ListColumns[x].ListFloors, i)
			}
		}
		if col.ID != 1 {
			nbFilledColumns++
			b.ListColumns[x].ListFloors = append(b.ListColumns[x].ListFloors, 1)
			for i := minFloor; i <= maxFloor; i++ {
				b.ListColumns[x].ListFloors = append(b.ListColumns[x].ListFloors, i)
			}
			minFloor = maxFloor + 1
			maxFloor = maxFloor + avgFloors
		}
		// create listElevator for each colum
		for j := 1; j <= b.NbElevators; j++ {
			e := Elevator{ID: j}
			b.ListColumns[x].ListElevators = append(b.ListColumns[x].ListElevators, e)
		}
		fmt.Println("Column  ", col.ID, " -- ListFloors =  ", b.ListColumns[x].ListFloors)
	}
	return &b
}

// function return True if int exist in list of int
func contains(s []int, e int) bool {
	for _, a := range s {
		if a == e {
			return true
		}
	}
	return false
}

// function return the best elevator when user is on groundfloor RC 'NOT FINISHED return only the best column'
func (b *Battery) AssignElevator(userDestination int) *Column {
	var BestColumn Column
	fmt.Println("** User at RC  and request the floor : ", userDestination, " **")
	for x, col := range b.ListColumns {
		if contains(col.ListFloors, userDestination) {
			BestColumn = b.ListColumns[x]
		}
	}
	// var MovingElevators, IdleElevators []Elevator
	// for i, E := range BestColumn.ListElevators {
	// 	if E.Direction == "Idle" || E.ElevatorCurrentFloor == 1 {
	// 		IdleElevators = append(IdleElevators, E)
	// 		gap := Abs(E.ElevatorCurrentFloor - 1)
	// 	} else {
	// 		MovingElevators = append(MovingElevators, E)
	// 		gap := Abs(E.ElevatorCurrentFloor-E.ElevatorNextStop) + Abs(E.ElevatorNextStop-1)
	// 	}
	// 	BestColumn.ListElevators[i].Gap = gap
	// }
	// fmt.Println("Elevator ", BestColumn.Id, E.Id, " Direction= ", E.Direction, "| CurrentFloor = ", E.ElevatorCurrentFloor, "| NextStop  = ", E.ElevatorNextStop)
	// fmt.Println("|  Gap = ", gap)
	// var BestElevator Elevator
	// if len(IdleElevators) > 0 {
	// 	By(GAP).Sort(BestColumn.ListElevators)
	// 	BestElevator = First(BestColumn.IdleElevators)
	// } else {
	// 	By(GAP).Sort(BestColumn.ListElevators)
	// 	BestElevator = First(BestColumn.MovingElevators)
	// }

	// fmt.Println("**********  The Best Elevator is : " + this.BestColumn.Id + this.BestElevator.Id + " **********")

	// MoveElevator(this.BestElevator,1 );
	// MoveElevator(this.BestElevator,userDestination);
	return &BestColumn

}
func main() {
	b := newBattery(1, 4, 66, 6, 5)
	//fmt.Println(b)
	// Function AssignElevator not finished but return THE BEST COLUMN
	BestColumn := b.AssignElevator(-3)
	fmt.Println("*********** The BEST Cloumn is : ", BestColumn.ID)
	BestColumn = b.AssignElevator(10)
	fmt.Println("*********** The BEST Cloumn is : ", BestColumn.ID)
	BestColumn = b.AssignElevator(30)
	fmt.Println("*********** The BEST Cloumn is : ", BestColumn.ID)
	BestColumn = b.AssignElevator(50)
	fmt.Println("*********** The BEST Cloumn is : ", BestColumn.ID)

}
