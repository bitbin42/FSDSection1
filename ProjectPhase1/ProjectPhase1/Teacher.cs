using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPhase1
{
    public class Teacher
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Teacher()
        {

        }

        public Teacher(int id, string firstName, string lastName)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString()
        {
            return $"[{ID}] {LastName},{FirstName}"; 
        }
    }
}
