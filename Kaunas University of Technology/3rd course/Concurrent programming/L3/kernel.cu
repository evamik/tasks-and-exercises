
#include "cuda_runtime.h"
#include "device_launch_parameters.h"

#include <stdio.h>
#include <sstream>
#include <fstream>
#include <vector>
#include <picojson.h>
//#include <iomanip>

struct Student
{
    double grade = 0.0;
    char name[20];
    int year = 0;
};

std::vector<Student> jsonToStudentVector(picojson::value jf) {
    picojson::array jsonarray = jf.get<picojson::array>();
    int size = jsonarray.size();
    std::vector<Student> students(size);
    for (int i = 0; i < size; i++)
    {
        Student stud = Student();
        stud.grade = jsonarray[i].get("grade").get<double>();
        std::string nameStr = jsonarray[i].get("name").get<std::string>();
        strcpy(stud.name, nameStr.c_str());
        stud.year = (int)jsonarray[i].get("year").get<double>();
        students[i] = stud;
    }
    return students;
}

__device__ char* studentToString(Student student) {
    int from = 0; 
    int to = 0;
    while (student.name[to] != '\0') to++;

    char* string = new char[to+10];
    string[from++] = '{'; to++;
    for (int i = from; i < to; i++) {
        string[i] = student.name[i - 1];
    }
    string[to++] = ' ';
    string[to++] = student.year + 48;
    string[to++] = ' ';

    int grade = student.grade * 100;
    for (int i = 3; i > 0; i--) {
        if(i == 1) string[to++] = '.';
        int div = (int)pow(10.0, i);
        int highest = floor((float)grade / div);
        string[to++] = highest + 48;
        grade -= highest*div;
    }
    string[to++] = grade + 48;

    string[to++] = '}';
    string[to] = '\0';

    return string;
}

__device__ void hashStudent(char* student, char* hash)
{
    int count = 0;
    while (student[count] != '\0') count++;
    int number = 0;
    for (int i = 0; i < count; i++) {
        number += i * student[i];
    }

    for (int i = 0; i < 40; i++) {
        int encoder = number % 36;
        if (encoder < 10) {
            hash[i] = encoder + 48;
        }
        else {
            hash[i] = encoder - 10 + 65;
        }
        number -= number % (count + encoder) - i;
    }
    hash[40] = '\0';
}

__device__ int countHashDigits(char* hash) {
    int count = 0;
    for (int i = 0; i < 40; i++)
        if (hash[i] > 47 && hash[i] < 58)
            count++;
    return count;
}

__global__ void hashCounts(Student* data, const int* count, char* results, int* index, char* hashes, int* hashIndex);

int main()
{
    std::vector<std::string> dataFiles = {
        "IFF-8-11_MikalauskasE_L1_dat_1.json",
        "IFF-8-11_MikalauskasE_L1_dat_2.json",
        "IFF-8-11_MikalauskasE_L1_dat_3.json",
    };

    std::vector<std::string> resFiles = {
        "IFF-8-11_MikalauskasE_L1_rez_1.txt",
        "IFF-8-11_MikalauskasE_L1_rez_2.txt",
        "IFF-8-11_MikalauskasE_L1_rez_3.txt",
    };

    cudaDeviceProp prop{};
    cudaGetDeviceProperties(&prop, 0);

    for (int i = 0; i < (int)dataFiles.size(); i++) {
        std::ifstream ifs(dataFiles[i]);
        picojson::value jf;
        picojson::parse(jf, ifs);
        std::vector<Student> studentVector = jsonToStudentVector(jf);
        int count = studentVector.size();
        Student* students = new Student[count]; 
        copy(studentVector.begin(), studentVector.end(), students);
        char* results = new char[count * 23+1];
        int index = 0;
        char* hashes = new char[count * 61 + 1];
        int hashIndex = 0;

        Student* device_students;
        int* device_count;
        char* device_results;
        int* device_index = 0;
        int size = count * sizeof(Student);
        char* device_hashes;
        int* device_hashIndex = 0;
        cudaMalloc(&device_students, size);
        cudaMalloc(&device_count, sizeof(int));
        cudaMalloc(&device_results, sizeof(char)*count * 23 + 1);
        cudaMalloc(&device_index, sizeof(int));
        cudaMalloc(&device_hashes, sizeof(char) * count * 61 + 1);
        cudaMalloc(&device_hashIndex, sizeof(int));

        cudaMemcpy(device_students, students, size, cudaMemcpyHostToDevice);
        cudaMemcpy(device_count, &count, sizeof(int), cudaMemcpyHostToDevice);
        cudaMemcpy(device_results, results, sizeof(char) * count*23 +1, cudaMemcpyHostToDevice);
        cudaMemcpy(device_index, &index, sizeof(int), cudaMemcpyHostToDevice);
        // hashes for printing
        cudaMemcpy(device_hashes, hashes, sizeof(char) * count * 61 + 1, cudaMemcpyHostToDevice);
        cudaMemcpy(device_hashIndex, &index, sizeof(int), cudaMemcpyHostToDevice);

        hashCounts <<<1, count/3 >>>(device_students, device_count, device_results, device_index, device_hashes, device_hashIndex);
        cudaDeviceSynchronize();

        cudaMemcpy(results, device_results, sizeof(char) * count * 23+1, cudaMemcpyDeviceToHost);
        cudaMemcpy(&index, device_index, sizeof(int), cudaMemcpyDeviceToHost);
        cudaMemcpy(hashes, device_hashes, sizeof(char) * count * 61 + 1, cudaMemcpyDeviceToHost);
        cudaMemcpy(&hashIndex, device_hashIndex, sizeof(int), cudaMemcpyDeviceToHost);
        cudaFree(device_students);
        cudaFree(device_count);
        cudaFree(device_results);
        cudaFree(device_index);
        cudaFree(device_hashes);
        cudaFree(device_hashIndex);
        results[index] = '\0';
        hashes[hashIndex] = '\0';


        std::ofstream ofs(resFiles[i]);

        std::string dataLine = "----------------------------------";
        char* buffer = new char[35];
        sprintf_s(buffer, 35, "| id | %-10s | %-4s | %-5s |", "Name", "Year", "Grade");
        std::string dataHeader = std::string(buffer);

        ofs << dataFiles[i] << ":" << std::endl;
        ofs << dataLine << std::endl;
        ofs << dataHeader << std::endl;
        ofs << dataLine << std::endl;
        for (int i = 0; i < studentVector.size(); i++) {
            buffer = new char[50];
            sprintf_s(buffer, 50, "| %2d | %-10s | %4d | %5.2f |",
                i + 1, studentVector.at(i).name,
                studentVector.at(i).year, studentVector.at(i).grade);
            ofs << std::string(buffer) << std::endl;
        }
        ofs << dataLine << std::endl << std::endl;

        ofs << std::endl << std::string(results) << std::endl;
        printf("%s hashes:\n", dataFiles[i].c_str());
        int j = 0;
        while (hashes[j] != '\0') {
            char c = hashes[j];
            printf("%c", hashes[j]);
            j++;
            if (j % 61 == 0)  printf("\n"); 
        }
        printf("\n");

        delete [] students;
        delete[] results;
    }
    return 0;
}

__global__ void hashCounts(Student* data, const int* count, char* results, int* index, char* hashes, int* hashIndex) {
    const auto slice_size = *count / blockDim.x;
    unsigned long start_index = slice_size * threadIdx.x;
    unsigned long end_index;
    if (threadIdx.x == blockDim.x - 1) {
        end_index = *count;
    }
    else {
        end_index = slice_size * (threadIdx.x + 1);
    }

    for (auto i = start_index; i < end_index; i++) {
        char hash[40];
        hashStudent(studentToString(data[i]), hash);
        int k = 0;
        // storing hashes for printing purposes ------------------
        int hto = atomicAdd(hashIndex, 61);
        while (data[i].name[k] != '\0') {
            hashes[hto + k] = data[i].name[k];
            k++;
        }
        for (;k < 20; k++) {
            hashes[hto + k] = ' ';
        }
        hashes[hto + k] = '-';
        k = 0;
        for (;k < 40; k++) {
            hashes[hto + k + 21] = hash[k];
        }
        // --------------------------------------------------------
        int digitsCount = countHashDigits(hash);
        if (digitsCount < 10) {
            int to = atomicAdd(index, 23);
            int j = 0;
            while (data[i].name[j] != '\0') {
                results[to + j] = data[i].name[j];
                j++;
            }
            results[to + j++] = '-';
            results[to + j++] = digitsCount + 48;
            for (;j < 23; j++) {
                results[to + j] = ' ';
            }
        }
    }
}