using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace savarankiskas2
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "students.csv";
            Program p = new Program();
            StudentContainer studentContainer = p.ReadData(path);
            GroupContainer groups = p.GetGroups(studentContainer);
            GroupContainer calculatedGroups = p.GetGroupsAverages(groups);
            calculatedGroups.SortDescending();
            p.PrintGroups(calculatedGroups);
        }

        /// Spausdina grupių vidurkius ir grupių pavadinimus
        /// <param name="groups"> Grupių konteineris </param>
        private void PrintGroups(GroupContainer groups)
        {
            Console.WriteLine("-------------------------\n" +
                              "| Vidurkis |   Grupė    |\n" +
                              "-------------------------");
            for(int i = 0; i < groups.GetCount(); i ++)
            {
                Group group = groups.GetGroup(i);
                Console.WriteLine("| {0, 8:0.00} | {1, -10} |", group.GetAverageGrade(), group.GetName());
                Console.WriteLine("-------------------------");
            }
        }

        /// Paskaičiuoja grupių vidurkius iš grupių konteinerio
        /// <param name="groupContainer"> Grupių konteineris </param>
        /// <returns> Paskaičiuotais vidurkiais grupių konteineris </returns>
        private GroupContainer GetGroupsAverages(GroupContainer groupContainer)
        {
            for (int i = 0; i < groupContainer.GetCount(); i++)
            {
                Group group = groupContainer.GetGroup(i);
                List<double> studentAverages = new List<double>();
                for (int j = 0; j < group.GetCount(); j++)
                {
                    Student student = group.GetStudent(j);
                    //Prie grupės prideda studento vidurkį
                    studentAverages.Add( GetAverage(student.Grades, student.GradeCount) );
                }
                //Paskaičiuoja grupės vidurkį
                group.SetAverageGrade( GetAverage(studentAverages.ToArray(), group.GetCount()) );
            }
            return groupContainer;
        }

        private GroupContainer GetGroups(StudentContainer studentContainer)
        {
            GroupContainer groups = new GroupContainer(studentContainer.GetCount());

            for(int i = 0; i < studentContainer.GetCount(); i++)
            {
                Student student = studentContainer.GetStudent(i);
                int groupIndex = groups.IndexOf(student.Group);
                if (groupIndex == -1)
                {
                    Group group = new Group(studentContainer.GetCount());
                    group.SetName(student.Group);
                    group.Add(student);
                    groups.Add(group);
                }
                else
                {
                    groups.GetGroup(groupIndex).Add(student);
                }
            }
            return groups;
        }

        public static double GetAverage(double[] numbers, int count)
        {
            double average = 0;
            for (int i = 0; i < count; i++)
                average += numbers[i];
            average = (double)average / count;
            return average;
        }

        private StudentContainer ReadData(string path)
        {
            string[] lines = File.ReadAllLines(@path);
            StudentContainer studentContainer = new StudentContainer(lines.Length);
            foreach (string line in lines)
            {
                //pavardė, vardas, grupė, pažymių kiekis, pažymiai
                string[] values = line.Split(';');
                string surname = values[0];
                string name = values[1];
                string group = values[2];
                int gradeCount = int.Parse(values[3]);
                double[] grades = new double[gradeCount];
                for (int i = 4; i < values.Length; i++)
                    grades[i - 4] = int.Parse(values[i]);

                Student student = new Student(surname, name, group, gradeCount, grades);
                studentContainer.Add(student);
            }
            return studentContainer;
        }
    }

    class GroupContainer
    {
        private Group[] Groups;
        private int Count;

        public GroupContainer(int size)
        {
            Count = 0;
            Groups = new Group[size];
        }

        public void Add(Group group)
        {
            Groups[Count] = group;
            Count++;
        }

        public Group GetGroup(int i)
        {
            return Groups[i];
        }

        public int GetCount()
        {
            return Count;
        }

        /// Suranda elemento indeksą masyve pagal pavadinimą
        /// <param name="groupName"> Grupės pavadinimas </param>
        /// <returns> Elemento indeksas (-1, jei nerastas) </returns>
        public int IndexOf(string groupName)
        {
            int index = -1;
            for (int i = 0; i < Count; i++)
                if (Groups[i].GetName() == groupName)
                    index = i;
            return index;
        }

        /// Surušiuoja grupes žemėjančia tvarka pagal vidurkius ir pavadinimus
        public void SortDescending()
        {
            for(int i = 0; i < Count; i++)
            {
                for(int j = i+1; j < Count; j++)
                {
                    if(Groups[j] >= Groups[i])
                    {
                        Group temp = Groups[j];
                        Groups[j] = Groups[i];
                        Groups[i] = temp;
                    }
                }
            }
        }
    }

    class Group
    {
        private string Name;
        private Student[] Students;
        private int Count;
        private double AverageGrade;

        public Group(int size)
        {
            Count = 0;
            Students = new Student[size];
        }

        public void SetAverageGrade(double grade)
        {
            AverageGrade = grade;
        }

        public double GetAverageGrade()
        {
            return AverageGrade;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public string GetName()
        {
            return Name;
        }

        public void Add(Student student)
        {
            Students[Count] = student;
            Count++;
        }

        public int GetCount()
        {
            return Count;
        }

        public Student GetStudent(int i)
        {
            return Students[i];
        }

        public static bool operator >= (Group group1, Group group2)
        {
            if (group1.GetAverageGrade() > group2.GetAverageGrade())
                return true;
            else if (group1.GetAverageGrade() == group2.GetAverageGrade())
            {
                if (string.Compare(group1.GetName(), group2.GetName()) < 0)
                    return true;
            }
            return false;
        }

        public static bool operator <= (Group group1, Group group2)
        {
            if (group1.GetAverageGrade() < group2.GetAverageGrade())
                return true;
            else if (group1.GetAverageGrade() == group2.GetAverageGrade())
            {
                if (string.Compare(group1.GetName(), group2.GetName()) > 0)
                    return true;
            }
            return false;
        }
    }

    class Student
    {
        public string Surname { get; private set; }
        public string Name { get; private set; }
        public string Group { get; private set; }
        public int GradeCount { get; private set; }
        public double[] Grades { get; private set; }
        public Student(string surname, string name, string group, int gradeCount, double[] grades)
        {
            Surname = surname;
            Name = name;
            Group = group;
            GradeCount = gradeCount;
            Grades = grades;
        }
    }

    class StudentContainer
    {
        private Student[] Students;
        private int Count;
        public StudentContainer(int size)
        {
            Count = 0;
            Students = new Student[size];
        }

        public Student GetStudent(int i)
        {
            return Students[i];
        }

        public int GetCount()
        {
            return Count;
        }

        public void Add(Student student)
        {
            Students[Count] = student;
            Count++;
        }
    }
}