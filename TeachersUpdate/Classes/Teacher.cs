using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersUpdate {

  /// <inheritdoc />
  public class Teacher : ITeacher {

    /// <inheritdoc />
    public Exception LastException {get; private set;}

    /// <inheritdoc />
    public bool IsDirty {get; set;}

    /// <inheritdoc />
    public string FirstName {get; private set;} 

    /// <inheritdoc />
    public string LastName {get; private set;} 

    /// <inheritdoc />
    public string Department {get; private set;} 

    /// <inheritdoc />
    public Int32 ID {get; private set;}

    /// <summary>
    /// Create new Teacher instance
    /// </summary>
    /// <param name="FirstName">Teacher's given name</param>
    /// <param name="LastName">Teacher's family name</param>
    /// <param name="Department">Teacher's department name</param>
    public Teacher(Int32 ID,string FirstName,string LastName,string Department) {
    this.ID=ID;
    this.FirstName=FirstName;
    this.LastName=LastName;
    this.Department=Department;
    IsDirty=false;
    }

    /// <inheritdoc />
    public string FullName(bool LastFirst=false) {
    return LastFirst?LastName+", "+FirstName:FirstName+" "+LastName;
    }

    /// <summary>
    /// Display the object definition
    /// </summary>
    /// <returns>Teacher's name and department</returns>
    public override string ToString() {
    return FullName(true)+" ["+Department+"]";
    }

  }
}
