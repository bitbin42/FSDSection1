using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPhase1.Builders
{
    public class ConcreteTeacherBuilder : AbstractTeacherBuilder
    {
        protected override void BuildID(Teacher teacher)
        {
            int ID=-1;
            while (ID<1) {
              Console.Write("Enter an ID for the teacher: ");
              var idAsText = Console.ReadLine();
              int.TryParse(idAsText,out ID);
              }
            teacher.ID = ID;
        }

        protected override void BuildFirstName(Teacher teacher)
        {
            string name=string.Empty;
            while (string.IsNullOrEmpty(name)) {
              Console.Write("Enter the teacher's first name: ");
              name = Console.ReadLine().Trim();
              }
            teacher.FirstName=name;
        }

        protected override void BuildLastName(Teacher teacher)
        {
            string name=string.Empty;
            while (string.IsNullOrEmpty(name)) {
              Console.Write("Enter the teacher's last name: ");
              name = Console.ReadLine().Trim();
              }
            teacher.LastName=name;
        }

    }
}
