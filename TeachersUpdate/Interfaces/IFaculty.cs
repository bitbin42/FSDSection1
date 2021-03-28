using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersUpdate{

    /// <summary>
    /// Manage teacher data
    /// </summary>
    public interface IFaculty {

    /// <summary>
    /// Options for sorting the teacher list
    /// </summ
    public enum SortOptions {ID=0, LastName=1, FirstName=2}

    /// <summary>
    /// Save last error encountered
    /// </summary>
    Exception LastException {get;}

    /// <summary>
    /// Clean up
    /// </summary>
    public void Dispose();

    /// <summary>
    /// Flag to tell if the data has changed
    /// </summary>
    bool IsDirty {get;}

    /// <summary>
    /// Get a list of teachers
    /// </summary>
    /// <returns>Enumerable list of ITeacher objects</returns>
    IEnumerable<ITeacher> Teachers {get;}

    /// <summary>
    /// Locate a specific teacher by name
    /// </summary>
    /// <param name="FirstName">Teacher's first name</param>
    /// <param name="LastName">Teacher's last name</param>
    /// <returns>Teacher object</returns>
    ITeacher FindTeacher(string FirstName,string LastName);

    /// <summary>
    /// Locate a specific teacher by ID
    /// </summary>
    /// <param name="ID">Teacher's ID</param>
    /// <returns>Teacher object</returns>
    ITeacher FindTeacher(Int32 ID);

    /// <summary>
    /// Get a list of departments
    /// </summary>
    IEnumerable<string> Departments {get;}

    /// <summary>
    /// Add a new teacher to the staff
    /// </summary>
    /// <param name="FirstName">Teacher's given name</param>
    /// <param name="LastName">Teacher's family name</param>
    /// <param name="Department">Department name</param>
    /// <returns>True if the name does not already exist</returns>
    bool AddTeacher(string FirstName,string LastName,string Department);

    /// <summary>
    /// Update an existing teacher's info
    /// </summary>
    /// <param name="ID">Teacher's ID number</param>
    /// <param name="FirstName">Teacher's given name</param>
    /// <param name="LastName">Teacher's family name</param>
    /// <param name="Department">Department name</param>
    /// <returns>True if the name does not already exist</returns>
    bool UpdateTeacher(Int32 ID,string FirstName,string LastName,string Department);

    /// <summary>
    /// Remove a teacher from the staff
    /// </summary>
    /// <param name="Teacher">Teacher object</param>
    /// <returns>True if successful</returns>
    bool RemoveTeacher(ITeacher Teacher);

    /// <summary>
    /// Save all updates
    /// </summary>
    /// <returns>True if the save was successful</returns>
    bool Save();

    /// <summary>
    /// Sort the list of teachers by ID, lastname, or firstname
    /// </summary>
    /// <param name="SortOrder">Sort key identifier</param>
    /// <returns>True if sort successful</returns>
    public bool SortTeachers(SortOptions SortOrder);

  }


}
