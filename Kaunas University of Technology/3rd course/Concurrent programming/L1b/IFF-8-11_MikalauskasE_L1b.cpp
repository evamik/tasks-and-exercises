#include <iostream>
#include <iomanip>
#include <sstream>
#include <fstream>
#include <string>
#include <nlohmann/json.hpp>
#include <omp.h>
#include <ostream>
#include <sha1.hpp>

using namespace std;
using json = nlohmann::json;

struct Student
{
    double grade = 0.0;
    string name = "";
    int year = 0;
    int count = 0;
};

struct Monitor
{
    Student* data;
    omp_lock_t* lock;
    bool isDone;
    int maxSize = 0;
    int currentSize = 0;
    int to = 0;
    int from = 0;

    void initMonitor(int size) {
        lock = new omp_lock_t;
        omp_init_lock(lock);
        maxSize = size;
        data = new Student[size];
    }

    void clear() {
        delete[] data;
        omp_destroy_lock(lock);
    }

    bool insert(Student student, bool isSorted) {
        omp_set_lock(lock);
        if (currentSize == maxSize) {
            omp_unset_lock(lock);
            return false;
        }
        else {
            if (!isSorted) {
                data[to] = student;
                to = (to + 1) % maxSize;
            }
            else {
                int i = maxSize - 1;
                while (student.count > data[i].count) {
                    i--;
                    if (i < 0)
                        break;
                    data[i + 1] = data[i];
                }
                data[i + 1] = student;
            }
            currentSize++;
        }
        omp_unset_lock(lock);
        return true;
    }

    bool remove(Student* student) {
        omp_set_lock(lock);
        if (currentSize == 0) {
            if (isDone) {
                student = new Student();
                omp_unset_lock(lock);
                return true;
            }
            omp_unset_lock(lock);
            return false;
        }
        else {
            *student = data[from];
            data[from] = Student();
            from = (from + 1) % maxSize;
            currentSize--;
        }
        omp_unset_lock(lock);
        return true;
    }
};

vector<Student> jsonToStudentVector(json jf) {
    vector<Student> students(jf.size());
    for (int i = 0; i < (int)jf.size(); i++)
    {
        Student stud = Student();
        stud.grade = jf[i].at("grade");
        stud.name = jf[i].at("name");
        stud.year = jf[i].at("year");
        students[i] = stud;
    }
    return students;
}

int hashStudent(Student student) {
    stringstream stream;
    stream << "{" << student.name << " " << student.year << " " << fixed << setprecision(2) << student.grade << "}";
    string s = stream.str();
    int size = s.size();
    const char* chars = s.c_str();
    char hex[41];
    sha1(chars).finalize().print_hex(hex);
    int count = 0;
    for (int i = 0; i < 40; i++) {
        if (isdigit(hex[i])) count++;
    }
    return count;
}

void execute(Monitor* dataMonitor, Monitor* resMonitor) {
    while (true) {
        Student stud;
        while (!dataMonitor->remove(&stud)) {}
        if (stud.name == "")
            break;

        int count = 0;
        count = hashStudent(stud);
        int a = 0;
        if (count > 20) {
            stud.count = count;
            while (!resMonitor->insert(stud, true)) {}
        }
    }
}

void writeResults(vector<Student>* students, Monitor* resMonitor, string dataFile, string resFile) {
    ofstream ofs(resFile);
    
    string dataLine = "----------------------------------";
    char* buffer = new char[35];
    sprintf_s(buffer, 35, "| id | %-10s | %-4s | %-5s |", "Name", "Year", "Grade");
    string dataHeader = string(buffer);

    ofs << dataFile << ":" << endl;
    ofs << dataLine << endl;
    ofs << dataHeader << endl;
    ofs << dataLine << endl;
    for (int i = 0; i < students->size(); i++) {
        buffer = new char[50];
        sprintf_s(buffer, 50, "| %2d | %-10s | %4d | %5.2f |",
            i+1, students->at(i).name.c_str(), 
            students->at(i).year, students->at(i).grade);
        ofs << string(buffer) << endl;
    }
    ofs << dataLine << endl << endl;

    string resLine = "------------------------------------------";
    buffer = new char[43];
    sprintf_s(buffer, 43, "| id | % -10s | % -4s | % -5s | % -5s |", "Name", "Year", "Grade", "Count");
    string resHeader = string(buffer);

    ofs << "results:" << endl;
    ofs << resLine << endl;
    ofs << resHeader << endl;
    ofs << resLine << endl;
    for (int i = 0; i < resMonitor->maxSize; i++) {
        if (resMonitor->data[i].name == "") break;
        buffer = new char[50];
        sprintf_s(buffer, 50, "| %2d | %-10s | %4d | %5.2f | %5d |",
            i+1, resMonitor->data[i].name.c_str(), resMonitor->data[i].year,
            resMonitor->data[i].grade, resMonitor->data[i].count);
        ofs << string(buffer) << endl;
    }
    ofs << resLine << endl;


    delete[] buffer;
}

int main()
{
    vector<string> dataFiles = {
        "IFF-8-11_MikalauskasE_L1_dat_1.json",
        "IFF-8-11_MikalauskasE_L1_dat_2.json",
        "IFF-8-11_MikalauskasE_L1_dat_3.json",
    };

    vector<string> resFiles = {
        "IFF-8-11_MikalauskasE_L1_rez_1.txt",
        "IFF-8-11_MikalauskasE_L1_rez_2.txt",
        "IFF-8-11_MikalauskasE_L1_rez_3.txt",
    };

    for (int i = 0; i < (int)dataFiles.size(); i++) {
        ifstream ifs(dataFiles[i]);
        json jf = json::parse(ifs);
        vector<Student> students = jsonToStudentVector(jf);

        Monitor dataMonitor = Monitor();
        dataMonitor.initMonitor(10);

        Monitor resMonitor = Monitor();
        resMonitor.initMonitor(jf.size());

#pragma omp parallel num_threads(5) default(none) shared(dataMonitor, resMonitor, students, cout)
        {
            int thread_id = omp_get_thread_num();
            if (thread_id == 0) { // adder thread
                for (int j = 0; j < students.size(); j++) {
                    while (!dataMonitor.insert(students[j], false)) {}
                }
                dataMonitor.isDone = true;
            }
            else if(thread_id < 5){
                execute(&dataMonitor, &resMonitor);
            }
        }

        writeResults(&students, &resMonitor, dataFiles[i], resFiles[i]);

        dataMonitor.clear();
        resMonitor.clear();
    }
    

    return 0;
}