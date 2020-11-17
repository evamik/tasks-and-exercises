package main

import "fmt"

/*
Du procesai-siuntejai siuncia vienam procesui-gavejui skaicius,
pirmasis procesas is eiles nuo 0, antrasis - nuo 11.
Procesas-gavejas priima skaicius, ir siuncia juos vienam is
dvieju procesu-spausdintoju: pirmajam lyginius, antrajam
nelyginius. Procesai-spausdintojai kaupia gautus skaicius
savo masyvuose ir, abiem procesams-siuntejams baigus darba,
isspausdina savo masyvu turinius i ekrana.
Darbas baigiamas, kai procesas-gavejas priima ir persiuncia 20 skaiciu
*/

var (
	numberCount = 20
)

func main() {
	workDone := make(chan bool)
	finishMain := make(chan bool)
	senderToReceiver := make(chan int)
	receiverToOddPrinter := make(chan int)
	receiverToEvenPrinter := make(chan int)

	go sender(0, senderToReceiver, workDone)
	go sender(11, senderToReceiver, workDone)

	go receiver(senderToReceiver, receiverToOddPrinter, receiverToEvenPrinter, workDone)

	go printer(receiverToEvenPrinter, workDone, finishMain)
	go printer(receiverToOddPrinter, workDone, finishMain)

	for i := 0; i < 2; i++ {
		<-finishMain
	}
}

func sender(startingNumber int, senderToReceiver chan<- int, workDone <-chan bool) {
	number := startingNumber
	for !isWorkDone(workDone) {
		select {
		case senderToReceiver <- number:
			number++
		default:
		}
	}
}

func receiver(senderToReceiver <-chan int, receiverToOddPrinter chan<- int,
	receiverToEvenPrinter chan<- int, workDone chan<- bool) {
	count := 0
	for count < numberCount {
		number := <-senderToReceiver

		if number%2 == 0 {
			receiverToEvenPrinter <- number
		} else {
			receiverToOddPrinter <- number
		}
		count++
	}
	close(workDone)
}

func printer(receiverToPrinter <-chan int, workDone <-chan bool, finishMain chan<- bool) {
	array := make([]int, numberCount)
	count := 0
	for !isWorkDone(workDone) {
		select {
		case number := <-receiverToPrinter:
			array[count] = number
			count++
		default:
		}
	}
	for i := 0; i < count; i++ {
		fmt.Println(array[i])
	}
	finishMain <- true
}

func isWorkDone(workDone <-chan bool) bool {
	select {
	case <-workDone:
		return true
	default:
		return false
	}
}
