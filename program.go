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
	ID, ElevatorCurrentFloor, ElevatorNextStop int
	Direction                                  string
}

func newBattery(id, nbColumns, nbFloors, nbBasements, nbElevators int) *Battery {
	b := Battery{}
	b.ID = id
	b.NbBasements = nbBasements
	b.NbFloors = nbFloors
	b.NbColumns = nbColumns
	b.NbElevators = nbElevators
	for i := 1; i <= b.NbColumns; i++ {
		c := Column{ID: i, NbElevators: b.NbElevators, NbFloors: b.NbFloors, NbBasements: b.NbBasements}
		b.ListColumns = append(b.ListColumns, c)
	}
	return &b
}

func (b *Battery) newColumn(id, nbElevators, nbFloors, nbBasements int) *Column {
	c := Column{}
	c.ID = id
	c.NbFloors = nbFloors
	c.NbBasements = nbBasements
	c.NbElevators = nbElevators
	c.ListFloors = []int{}
	c.ListElevators = []Elevator{}
	avgFloors := int((b.NbFloors - b.NbBasements) / (b.NbColumns - 1))
	fmt.Println("Avg Floors = ", avgFloors)
	nbFilledColumns := 0
	minFloor := 2
	maxFloor := avgFloors
	for _, c := range b.ListColumns {
		if c.ID == 1 {
			c.ListFloors = append(c.ListFloors, 1)
			for i := -1; i >= -c.NbBasements; i-- {
				c.ListFloors = append(c.ListFloors, i)
			}
		}
		if c.ID != 1 {
			nbFilledColumns++
			c.ListFloors = append(c.ListFloors, 1)
			for i := minFloor; i <= maxFloor; i++ {
				c.ListFloors = append(c.ListFloors, i)
			}
			minFloor = maxFloor + 1
			maxFloor = maxFloor + avgFloors
		}
	}
	fmt.Println("Column  ", c.ID, " -- ListFloors =  ", c.ListFloors)
	return &c
}

func main() {
	b := newBattery(1, 4, 66, 6, 5)
	fmt.Println(b)

}
