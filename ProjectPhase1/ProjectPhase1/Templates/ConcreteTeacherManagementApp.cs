using ProjectPhase1.Builders;
using ProjectPhase1.Repositories;
using ProjectPhase1.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPhase1.Templates
{
    public class ConcreteTeacherManagementApp : AbstractTeacherAppTemplate
    {
        protected override void loadTeachers()
        {
            var teachersRepository = new PipeDelimitedFileTeachersRepository("teachers.txt");
            var teachersAsList = teachersRepository.Load();
            _teachers = teachersAsList.ToDictionary(t => t.ID);
            Console.WriteLine($"Loaded {_teachers.Count} teachers");
        }

        protected override void saveTeachers()
        {
            var teachersRepository = new PipeDelimitedFileTeachersRepository("teachers.txt");
            teachersRepository.Save(_teachers.Values);
        }

        protected override int getOption()
        {
            int option=-1;
            while (option<1) {
              string optionAsString=Console.ReadLine().Trim();
              if (int.TryParse(optionAsString,out option)==false || option<1)
                Console.WriteLine("Please enter a positive integer");
              }
            return option;
        }

        protected override void addTeacher()
        {
            var teacherBuilder = new ConcreteTeacherBuilder();
            var teacher = teacherBuilder.Build();
            if (_teachers.ContainsKey(teacher.ID))
              Console.WriteLine("That teacher ID is already in use");
            else {
              _teachers.Add(teacher.ID,teacher);
              Console.WriteLine($"Added {teacher}");
              }
        }

        protected override void deleteTeacher()
        {
            Console.WriteLine("Enter ID of teacher to delete");
            var id = getOption();

            if (!_teachers.ContainsKey(id))
            {
                Console.WriteLine($"Teacher with id {id} not found");
            }
            else
            {
                _teachers.Remove(id);
                Console.WriteLine("Removed teacher");
            }
        }

        protected override void findTeacher()
        {
            Console.WriteLine("Enter ID of teacher to find");
            var id = getOption();
           
            if (!_teachers.ContainsKey(id))
            {
                Console.WriteLine($"Teacher with id {id} not found");
            }
            else
            {
                Teacher teacher=_teachers[id];
                Console.WriteLine(teacher);
            }

        }

        protected override void listTeachers(IEnumerable<Teacher> teachers)
        {
            foreach (Teacher teacher in teachers)
            {
                Console.WriteLine(teacher.ToString());
            }
        }

        protected override void sortTeachers()
        {
            Console.WriteLine("You chose to sort teachers");
            Console.WriteLine("How would you like to sort them?");
            Console.WriteLine("1) ID");
            Console.WriteLine("2) Last Name");
            Console.WriteLine("3) First Name");

            var option = getOption();
            ISortTeachersStrategy sortStrategy = null;
            switch (option)
            {
                case 1: sortStrategy = new SortTeachersByIDStrategy(); break;
                case 2: sortStrategy = new SortTeachersByLastNameStrategy(); break;
                case 3: sortStrategy = new SortTeachersByFirstNameStrategy(); break;
            }
            if (sortStrategy==null) 
              Console.WriteLine("No sort selected");
            else {
              var sorted = sortStrategy.Sort(_teachers.Values);
              listTeachers(sorted);
              }
        }

        protected override void updateTeacher()
        {
            Console.WriteLine("Enter ID of teacher to update");
            var id = getOption();
            if (!_teachers.ContainsKey(id))
            {
                Console.WriteLine($"Did not find teacher with id: {id}");
                return;
            }

            var teacher = _teachers[id];
            Console.WriteLine("You selected...");
            Console.WriteLine(teacher);
            teacher.FirstName=getName("first",teacher.FirstName);
            teacher.LastName=getName("last",teacher.LastName);
            Console.WriteLine($"updated to: {teacher}");

        }

        private string getName(string nameType,string currentName) {
        Console.Write($"Enter the new {nameType} name: [{currentName}] ");
        string name=Console.ReadLine().Trim();
        return (name.Length==0)?currentName:name;
        }

    }
}
