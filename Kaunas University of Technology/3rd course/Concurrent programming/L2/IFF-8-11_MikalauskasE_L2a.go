package main

import (
	"crypto/sha1"
	"encoding/hex"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
)

// Student object
type Student struct {
	Name  string  `json:"name"`
	Year  int     `json:"year"`
	Grade float64 `json:"grade"`
	Count int
}

// Hash func
func (s Student) hash() [20]byte {
	structString := fmt.Sprintf("{%s %d %.2f}", s.Name, s.Year, s.Grade)
	b := []byte(structString)
	hash := sha1.Sum(b)
	return hash
}

var (
	dataBufferSize = 10
	threadCount    = 5
)

func getDataFiles() []string {
	return []string{
		"IFF-8-11_MikalauskasE_L2_dat_1.json",
		"IFF-8-11_MikalauskasE_L2_dat_2.json",
		"IFF-8-11_MikalauskasE_L2_dat_3.json",
	}
}

func getResFiles() []string {
	return []string{
		"IFF-8-11_MikalauskasE_L2_rez_1.txt",
		"IFF-8-11_MikalauskasE_L2_rez_2.txt",
		"IFF-8-11_MikalauskasE_L2_rez_3.txt",
	}
}

func main() {
	for i := 0; i < len(getDataFiles()); i++ {
		fmt.Printf("Starting work with %s\n", getDataFiles()[i])
		students := readStudents(getDataFiles()[i])
		res := make([]Student, len(students))

		mainToData := make(chan Student)
		dataToWork := make(chan Student, threadCount)
		workToData := make(chan bool, threadCount)
		workToRes := make(chan Student, threadCount)
		workers := make(chan bool)
		resToMain := make(chan Student)

		go dataThread(mainToData, dataToWork, workToData)
		for j := 0; j < threadCount; j++ {
			go execute(dataToWork, workToData, workToRes, workers)
		}
		go resThread(workToRes, resToMain, len(students))

		for j := 0; j < len(students); j++ {
			mainToData <- students[j]
		}
		close(mainToData)

		for j := 0; j < threadCount; j++ {
			<-workers
		}
		close(workToRes)

		j := 0
		for stud := range resToMain {
			res[j] = stud
			j++
		}

		fmt.Printf("Work done, saving to %s\n\n", getResFiles()[i])
		writeResults(students, res, getDataFiles()[i], getResFiles()[i])
	}
}

func dataThread(mainToData <-chan Student, dataToWork chan<- Student, workToData <-chan bool) {
	count := 0
	to := 0
	from := 0
	data := make([]Student, dataBufferSize)
	done := false
	for {
		if done && count == 0 {
			close(dataToWork)
			break
		}
		if count > 0 {
			select {
			case <-workToData:
				var student = data[from]
				var emptyStudent Student
				data[from] = emptyStudent
				from = (from + 1) % dataBufferSize
				count--
				dataToWork <- student
			}
		}
		if !done && count < dataBufferSize {
			stud := <-mainToData
			if stud == (Student{}) {
				done = true
			} else {
				data[to] = stud
				to = (to + 1) % dataBufferSize
				count++
			}
		}
	}
}

func execute(dataToWork <-chan Student, workToData chan<- bool, workToRes chan<- Student, workers chan<- bool) {
	for {
		workToData <- true
		stud := <-dataToWork
		if stud == (Student{}) {
			workers <- true
			break
		}
		hashBytes := stud.hash()
		hash := hex.EncodeToString(hashBytes[:])
		count := countDigits(hash)
		if count > 20 {
			stud.Count = count
			workToRes <- stud
		}
	}
}

func resThread(workToRes <-chan Student, resToMain chan<- Student, len int) {
	count := 0
	from := 0
	data := make([]Student, len)
	for {
		stud := <-workToRes
		if stud == (Student{}) {
			break
		}
		i := len - 1
		for stud.Count > data[i].Count {
			i--
			if i < 0 {
				break
			}
			data[i+1] = data[i]
		}
		data[i+1] = stud
		count++
	}
	for i := 0; count > 0; i++ {
		var student = data[from]
		var emptyStudent Student
		data[from] = emptyStudent
		from = (from + 1) % len
		count--
		resToMain <- student
	}
	close(resToMain)
}

// CountDigits - counts digits in a given string
func countDigits(str string) int {
	count := 0
	for _, char := range str {
		if _, err := strconv.Atoi(string(char)); err == nil {
			count++
		}
	}
	return count
}

func readStudents(filename string) []Student {
	jsonFile, err := os.Open(filename)
	if err != nil {
		fmt.Println(err)
	}

	byteValue, _ := ioutil.ReadAll(jsonFile)
	jsonFile.Close()

	var students []Student
	json.Unmarshal(byteValue, &students)

	return students
}

func writeResults(data []Student, res []Student, dataFile string, resFile string) {
	file, err := os.Create(resFile)
	if err != nil {
		log.Fatal(err)
		return
	}

	dataLine := fmt.Sprintln("-----------------------------------")
	dataHeader := fmt.Sprintf("| ID  | %-10s | %-4s | %-5s |\n", "Name", "Year", "Grade")

	file.WriteString(dataFile + ":\n")
	file.WriteString(dataLine)
	file.WriteString(dataHeader)
	file.WriteString(dataLine)
	for i, student := range data {
		str := fmt.Sprintf("| %3d | %-10s | %4d | %5.2f |\n", i, student.Name, student.Year, student.Grade)
		file.WriteString(str)
	}
	file.WriteString(dataLine)
	file.WriteString("\n")

	resLine := fmt.Sprintln("-------------------------------------------")
	resHeader := fmt.Sprintf("| ID  | %-10s | %-4s | %-5s | %-5s |\n", "Name", "Year", "Grade", "Count")

	file.WriteString("results:\n")
	file.WriteString(resLine)
	file.WriteString(resHeader)
	file.WriteString(resLine)
	for i, student := range res {
		if student.Name == "" {
			continue
		}
		str := fmt.Sprintf("| %3d | %-10s | %4d | %5.2f | %5d |\n", i, student.Name, student.Year, student.Grade, student.Count)
		file.WriteString(str)
	}
	file.WriteString(resLine)
}
