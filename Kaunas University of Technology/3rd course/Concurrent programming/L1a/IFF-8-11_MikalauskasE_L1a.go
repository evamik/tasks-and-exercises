package main

import (
	"crypto/sha1"
	"encoding/hex"
	"encoding/json"
	"errors"
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"sync"
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

// Monitor struct
type Monitor struct {
	data        []Student
	mutex       *sync.Mutex
	cond        *sync.Cond
	currentSize int
	isDone      bool
	to          int
	from        int
}

func newMonitor(bufferSize int) Monitor {
	var buffer = make([]Student, bufferSize)
	var mutex = sync.Mutex{}
	var cond = sync.NewCond(&mutex)
	return Monitor{
		data:        buffer,
		mutex:       &mutex,
		cond:        cond,
		currentSize: 0,
		isDone:      false,
	}
}

func (m *Monitor) insert(student Student, isSorted bool) {
	defer m.mutex.Unlock()
	m.mutex.Lock()
	for m.currentSize == len(m.data) {
		m.cond.Wait()
	}
	if !isSorted {
		m.data[m.to] = student
		m.to = (m.to + 1) % len(m.data)
	} else {
		i := len(m.data) - 1
		for student.Count > m.data[i].Count {
			i--
			if i < 0 {
				break
			}
			m.data[i+1] = m.data[i]
		}
		m.data[i+1] = student
	}
	m.currentSize++
	m.cond.Broadcast()
}

func (m *Monitor) remove() (Student, error) {
	defer m.mutex.Unlock()
	m.mutex.Lock()
	for m.currentSize == 0 {
		if m.isDone {
			var emptyStudent Student
			return emptyStudent, errors.New("No student")
		}

		m.cond.Wait()
	}
	var student = m.data[m.from]
	var emptyStudent Student
	m.data[m.from] = emptyStudent
	m.from = (m.from + 1) % dataBufferSize
	m.currentSize--
	m.cond.Broadcast()
	return student, nil
}

var (
	dataBufferSize = 10
	wg             sync.WaitGroup
	threadCount    = 5
)

func getDataFiles() []string {
	return []string{
		"IFF-8-11_MikalauskasE_L1_dat_1.json",
		"IFF-8-11_MikalauskasE_L1_dat_2.json",
		"IFF-8-11_MikalauskasE_L1_dat_3.json",
	}
}

func getResFiles() []string {
	return []string{
		"IFF-8-11_MikalauskasE_L1_rez_1.txt",
		"IFF-8-11_MikalauskasE_L1_rez_2.txt",
		"IFF-8-11_MikalauskasE_L1_rez_3.txt",
	}
}

func main() {
	for i := 0; i < len(getDataFiles()); i++ {
		fmt.Printf("Starting work with %s\n", getDataFiles()[i])
		students := readStudents(getDataFiles()[i])

		dataMonitor := newMonitor(dataBufferSize)
		resMonitor := newMonitor(len(students))

		for j := 0; j < threadCount; j++ {
			wg.Add(1)
			go execute(&dataMonitor, &resMonitor)
		}
		wg.Add(1)
		go addToMonitor(&dataMonitor, students)
		wg.Wait()
		fmt.Printf("Work done, saving to %s\n\n", getResFiles()[i])
		writeResults(students, resMonitor.data, getDataFiles()[i], getResFiles()[i])
	}
}

func execute(dataMonitor *Monitor, resMonitor *Monitor) {
	defer wg.Done()
	for {
		var student, err = dataMonitor.remove()
		if err != nil {
			break
		}

		hashBytes := student.hash()
		hash := hex.EncodeToString(hashBytes[:])
		count := countDigits(hash)
		if count > 20 {
			student.Count = count
			resMonitor.insert(student, true)
		}
	}
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

func addToMonitor(monitor *Monitor, data []Student) {
	defer wg.Done()
	for _, el := range data {
		monitor.insert(el, false)
	}
	monitor.mutex.Lock()
	monitor.isDone = true
	monitor.cond.Broadcast()
	monitor.mutex.Unlock()
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

	dataLine := fmt.Sprintln("-----------------------------")
	dataHeader := fmt.Sprintf("| %-10s | %-4s | %-5s |\n", "Name", "Year", "Grade")

	file.WriteString(dataFile + ":\n")
	file.WriteString(dataLine)
	file.WriteString(dataHeader)
	file.WriteString(dataLine)
	for _, student := range data {
		str := fmt.Sprintf("| %-10s | %4d | %5.2f |\n", student.Name, student.Year, student.Grade)
		file.WriteString(str)
	}
	file.WriteString(dataLine)
	file.WriteString("\n")

	resLine := fmt.Sprintln("-------------------------------------")
	resHeader := fmt.Sprintf("| %-10s | %-4s | %-5s | %-5s |\n", "Name", "Year", "Grade", "Count")

	file.WriteString("results:\n")
	file.WriteString(resLine)
	file.WriteString(resHeader)
	file.WriteString(resLine)
	for _, student := range res {
		if student.Name == "" {
			continue
		}
		str := fmt.Sprintf("| %-10s | %4d | %5.2f | %5d |\n", student.Name, student.Year, student.Grade, student.Count)
		file.WriteString(str)
	}
	file.WriteString(resLine)
}
