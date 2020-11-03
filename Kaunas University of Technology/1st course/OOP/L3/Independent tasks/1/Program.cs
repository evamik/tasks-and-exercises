using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savarankiskas1
{
    class Program
    {
        public const int NumberOfBranches = 3;
        public const int MaxNumberOfBreeds = 30;
        public const int MaxNumberOfAnimals = 50;

        static void Main(string[] args)
        {
            Program p = new Program();
            Branch[] branches = new Branch[NumberOfBranches];
            branches[0] = new Branch("Kaunas");
            branches[1] = new Branch("Vilnius");
            branches[2] = new Branch("Siauliai");
            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv");
            ReadFiles(branches);

            Console.WriteLine("Surušiuotas visų šunų sąrašas: ");
            AnimalsContainer allDogs = new AnimalsContainer(MaxNumberOfAnimals * NumberOfBranches);
            p.AddDogsToContainer(branches, allDogs);
            allDogs.SortAnimals();
            PrintAnimalsToConsole(allDogs);
            Console.WriteLine();

            Console.WriteLine("Surušiuotas visų kačių sąrašas: ");
            AnimalsContainer allCats = new AnimalsContainer(MaxNumberOfAnimals * NumberOfBranches);
            p.AddCatsToContainer(branches, allCats);
            allCats.SortAnimals();
            PrintAnimalsToConsole(allCats);
            Console.WriteLine();

            Console.WriteLine("Surušiuotas visų jūrų kiaulyčių sąrašas: ");
            AnimalsContainer allGuineaPigs = new AnimalsContainer(MaxNumberOfAnimals * NumberOfBranches);
            p.AddGuineaPigsToContainer(branches, allGuineaPigs);
            allGuineaPigs.SortAnimals();
            PrintAnimalsToConsole(allGuineaPigs);
        }

        private void AddDogsToContainer(Branch[] branches, AnimalsContainer allDogs)
        {
            for (int i = 0; i < NumberOfBranches; i++)
            {
                for (int j = 0; j < branches[i].Dogs.Count; j++)
                {
                    allDogs.AddAnimal(branches[i].Dogs.GetAnimal(j));
                }
            }
        }

        private void AddCatsToContainer(Branch[] branches, AnimalsContainer allCats)
        {
            for (int i = 0; i < NumberOfBranches; i++)
            {
                for (int j = 0; j < branches[i].Cats.Count; j++)
                {
                    allCats.AddAnimal(branches[i].Cats.GetAnimal(j));
                }
            }
        }

        private void AddGuineaPigsToContainer(Branch[] branches, AnimalsContainer allGuineaPigs)
        {
            for (int i = 0; i < NumberOfBranches; i++)
            {
                for (int j = 0; j < branches[i].GuineaPigs.Count; j++)
                {
                    allGuineaPigs.AddAnimal(branches[i].GuineaPigs.GetAnimal(j));
                }
            }
        }

        private void GetBreeds(AnimalsContainer animals, out string[] breeds, out int breedCount)
        {
            breeds = new string[MaxNumberOfBreeds];
            breedCount = 0;
            for (int i = 0; i < animals.Count; i++)
            {
                string breed = animals.GetAnimal(i).Breed;
                if (!breeds.Contains(breed))
                {
                    breeds[breedCount++] = breed;
                }
            }
        }

        private AnimalsContainer FilterByBreed(AnimalsContainer animals, string breed)
        {
            AnimalsContainer filteredAnimals = new
            AnimalsContainer(Program.MaxNumberOfAnimals);
            for (int i = 0; i < animals.Count; i++)
            {
                if (animals.GetAnimal(i).Breed == breed)
                {
                    filteredAnimals.AddAnimal(animals.GetAnimal(i));
                }
            }
            return filteredAnimals;
        }

        private string GetMostPopularBreed(AnimalsContainer animals)
        {
            String popular = "not found";
            int count = 0;
            int breedCount = 0;
            string[] breeds;
            GetBreeds(animals, out breeds, out breedCount);
            for (int i = 0; i < breedCount; i++)
            {
                AnimalsContainer filteredAnimals = FilterByBreed(animals, breeds[i]);
                if (filteredAnimals.Count > count)
                {
                    popular = breeds[i];
                    count = filteredAnimals.Count;
                }
            }
            return popular;
        }

        static void ReadFiles(Branch[] branches)
        {
            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv");
            foreach (string path in filePaths)
            {
                bool rado = ReadAnimalData(path, branches);
                if (rado == false)
                    Console.WriteLine("Neatpažintas failo {0} miestas", path);
            }
        }

        private int CountAggressive(AnimalsContainer animals)
        {
            int counter = 0;
            for (int i = 0; i < animals.Count; i++)
            {
                Dog dog = animals.GetAnimal(i) as Dog;
                if (dog != null && dog.Aggressive)
                {
                    counter++;
                }
            }
            return counter;
        }

        static void PrintAnimalsToConsole(AnimalsContainer animals)
        {
            for (int i = 0; i < animals.Count; i++)
            {
                Console.WriteLine("Nr {0, 2}: {1}", (i + 1),
               animals.GetAnimal(i).ToString());
            }
        }


        static private Branch GetBranchByTown(Branch[] branches, string town)
        {
            for (int i = 0; i < NumberOfBranches; i++)
            {
                if (branches[i].Town == town)
                {
                    return branches[i];
                }
            }
            return null;
        }

        private static bool ReadAnimalData(string file, Branch[] branches)
        {
            string town = null;
            using (StreamReader reader = new StreamReader(@file))
            {
                string line = null;
                line = reader.ReadLine();
                if (line != null)
                {
                    town = line;
                }
                Branch branch = GetBranchByTown(branches, town);
                if (branch == null) // neatpažino miesto
                    return false;
                while (null != (line = reader.ReadLine()))
                {
                    string[] values = line.Split(',');
                    char type = line[0];
                    string name = values[1];
                    int chipId = int.Parse(values[2]);
                    string breed = values[3];
                    string owner = values[4];
                    string phone = values[5];
                    DateTime vd = DateTime.Parse(values[6]);
                    switch (type)
                    {
                        case 'D':
                            //atkreipkite dėmesį - šunys turi papildomą požymį "aggressive"
                            bool aggressive = bool.Parse(values[7]);
                            Dog dog = new Dog(name, chipId, breed, owner, phone, vd,
                           aggressive);
                            if (!branch.Dogs.Contains(dog))
                            {
                                branch.AddDog(dog);
                            }
                            break;
                        case 'C':
                            Cat cat = new Cat(name, chipId, breed, owner, phone, vd);
                            if (!branch.Cats.Contains(cat))
                            {
                                branch.AddCat(cat);
                            }
                            break;
                        case 'G':
                            GuineaPig guineaPig = new GuineaPig(name, breed, owner, phone, vd);
                            if (!branch.GuineaPigs.Contains(guineaPig))
                            {
                                branch.AddGuineaPig(guineaPig);
                            }
                            break;
                    }
                }
                return true;
            }
        }
    }

    class Branch
    {
        public string Town { get; set; }
        public AnimalsContainer Dogs { get; set; }
        public AnimalsContainer Cats { get; set; }
        public AnimalsContainer GuineaPigs { get; set; }
        public Branch(string town)
        {
            Town = town;
            Dogs = new AnimalsContainer(Program.MaxNumberOfAnimals);
            Cats = new AnimalsContainer(Program.MaxNumberOfAnimals);
            GuineaPigs = new AnimalsContainer(Program.MaxNumberOfAnimals);
        }

        public void AddDog(Dog dog)
        {
            Dogs.AddAnimal(dog);
        }

        public void AddCat(Cat cat)
        {
            Cats.AddAnimal(cat);
        }

        public void AddGuineaPig(GuineaPig guineaPig)
        {
            GuineaPigs.AddAnimal(guineaPig);
        }
    }

    class GuineaPig : Animal
    {
        private static int VaccinationDuration = 1;

        public GuineaPig(string name, string breed, string owner, string phone, DateTime vaccinationDate) 
            : base (name, breed, owner, phone, vaccinationDate)
        {

        }

        public override bool isVaccinationExpired()
        {
            return VaccinationDate.AddYears(VaccinationDuration).CompareTo(DateTime.Now) > 0;
        }

        public override int GetHashCode()
        {
            return Owner.GetHashCode() ^ Name.GetHashCode();
        }

        public override String ToString()
        {
            return String.Format("ChipId: {0,-5} Breed: {1,-20} Name: {2,-10} Owner: {3,-10} " +
                "({4}) Last vaccination date: {5:yyyy - MM - dd}",
                "", Breed, Name, Owner, Phone, VaccinationDate);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as GuineaPig); //kviečiame tipui specifinį metodą toje pačioje klasėje
        }

        public bool Equals(GuineaPig guineaPig)
        {
            return base.Equals(guineaPig); //kviečiame tėvinės klasės Animal Equals metodą
                                     //galima papildomai tikrinti pagal tik Dog klasės būdingas sąvybes, pvz
                                     //return base.Equals(dog) && this.Aggressive == dog.Aggressive;
        }

        public static bool operator ==(GuineaPig lhs, GuineaPig rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    return true;
                }
                return false;
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(GuineaPig lhs, GuineaPig rhs)
        {
            return !(lhs == rhs);
        }
    }


    abstract class AnimalMarked : Animal
    {
        public int ChipId { get; set; }

        public AnimalMarked (string name, int chipId, string breed, string owner, string phone,
       DateTime vaccinationDate) : base (name, breed, owner, phone, vaccinationDate)
        {
            Name = name;
            ChipId = chipId;
            Breed = breed;
            Owner = owner;
            Phone = phone;
            VaccinationDate = vaccinationDate;
        }

        public override int GetHashCode()
        {
            return ChipId.GetHashCode() ^ Name.GetHashCode();
        }
    }

    class Dog : AnimalMarked
    {
        private static int VaccinationDuration = 1;

        public Dog(string name, int chipId, string breed, string owner, string phone,
       DateTime vaccinationDate, bool aggressive) : base(name, chipId, breed, owner, phone,
       vaccinationDate)
        {
            Aggressive = aggressive;
        }

        public bool Aggressive { get; set; }
        //abstraktaus Animal klasės metodo realizacija

        public override bool isVaccinationExpired()
        {
            return VaccinationDate.AddYears(VaccinationDuration).CompareTo(DateTime.Now) > 0;
        }

        public override String ToString()
        {
            return String.Format("ChipId: {0,-5} Breed: {1,-20} Name: {2,-10} Owner: {3,-10} " +
                "({4}) Last vaccination date: {5:yyyy - MM - dd} Agressive: {6}",
                ChipId, Breed, Name, Owner, Phone, VaccinationDate, Aggressive);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Dog); //kviečiame tipui specifinį metodą toje pačioje klasėje
        }

        public bool Equals(Dog dog)
        {
            return base.Equals(dog); //kviečiame tėvinės klasės Animal Equals metodą
                                     //galima papildomai tikrinti pagal tik Dog klasės būdingas sąvybes, pvz
                                     //return base.Equals(dog) && this.Aggressive == dog.Aggressive;
        }

        public override int GetHashCode()
        {
            return ChipId.GetHashCode() ^ Name.GetHashCode();
        }

        public static bool operator ==(Dog lhs, Dog rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    return true;
                }
                return false;
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Dog lhs, Dog rhs)
        {
            return !(lhs == rhs);
        }
    }

    class Cat : AnimalMarked
    {
        private static int VaccinationDurationMonths = 6;
        public Cat(string name, int chipId, string breed, string owner, string phone,
       DateTime vaccinationDate)
        : base(name, chipId, breed, owner, phone, vaccinationDate)
        {

        }

        //abstraktaus Animal klasės metodo realizacija
        public override bool isVaccinationExpired()
        {
            return
           VaccinationDate.AddMonths(VaccinationDurationMonths).CompareTo(DateTime.Now) > 0;
        }

        public override String ToString()
        {
            return String.Format("ChipId: {0,-5} Breed: {1,-20} Name: {2,-10} Owner: {3,-10}" +
           "({4}) Last vaccination date: {5:yyyy - MM - dd}", ChipId, Breed, Name, Owner, Phone, VaccinationDate);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Cat);
        }

        public bool Equals(Cat cat)
        {
            return base.Equals(cat);
        }

        public override int GetHashCode()
        {
            return ChipId.GetHashCode() ^ Name.GetHashCode();
        }

        public static bool operator ==(Cat lhs, Cat rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    return true;
                }
                return false;
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Cat lhs, Cat rhs)
        {
            return !(lhs == rhs);
        }
    }

    class AnimalsContainer
    {
        private Animal[] Animals;
        public int Count { get; private set; }

        public AnimalsContainer(int size)
        {
            Animals = new Animal[size];
        }

        public void AddAnimal(Animal animal)
        {
            Animals[Count] = animal;
            Count++;
        }

        public void SetAnimal(int index, Animal animal)
        {
            Animals[index] = animal;
        }

        public Animal GetAnimal(int index)
        {
            return Animals[index];
        }

        public void RemoveAnimal(Animal animal)
        {
            int i = 0;
            while (i < Count)
            {
                if (Animals[i].Equals(animal))
                {
                    Count--;
                    for (int j = i; j < Count; j++)
                    {
                        Animals[j] = Animals[j + 1];
                    }
                    break;
                }
                i++;
            }
        }

        public bool Contains(Animal animal)
        {
            return Animals.Contains(animal);
        }

        public void SortAnimals()
        {
            for (int i = 0; i < Count - 1; i++)
            {
                Animal minValueAnimal = Animals[i];
                int minValueIndex = i;
                for (int j = i + 1; j < Count; j++)
                {
                    if (Animals[j] <= minValueAnimal)
                    {
                        minValueAnimal = Animals[j];
                        minValueIndex = j;
                    }
                }
                Animals[minValueIndex] = Animals[i];
                Animals[i] = minValueAnimal;
            }
        }
    }

    abstract class Animal {

        public string Name { get; set; }
        public string Breed { get; set; }
        public string Owner { get; set; }
        public string Phone { get; set; }
        public DateTime VaccinationDate { get; set; }

        public Animal(string name, string breed, string owner, string phone, DateTime vaccinationDate)
        {
            Name = name;
            Breed = breed;
            Owner = owner;
            Phone = phone;
            VaccinationDate = vaccinationDate;
        }

        public override int GetHashCode()
        {
            return Owner.GetHashCode() ^ Name.GetHashCode();
        }

        abstract public bool isVaccinationExpired();

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Animal);
        }

        public bool Equals(Animal animal)
        {
            if (Object.ReferenceEquals(animal, null))
            {
                return false;
            }
            if (this.GetType() != animal.GetType())
            {
                return false;
            }
            return (Name == animal.Name) && (Owner == animal.Owner);
        }

        public static bool operator ==(Animal lhs, Animal rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    return true;
                }
                return false;
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Animal lhs, Animal rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator <=(Animal lhs, Animal rhs)
        {
            return String.Compare(lhs.Name, rhs.Name) < 0 || 
                String.Compare(lhs.Name, rhs.Name) == 0 && String.Compare(lhs.Owner, rhs.Owner) < 0;
        }

        public static bool operator >=(Animal lhs, Animal rhs)
        {
            return String.Compare(lhs.Name, rhs.Name) > 0 && String.Compare(lhs.Owner, rhs.Owner) > 0;
        }
    }
}
