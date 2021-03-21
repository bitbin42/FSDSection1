using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersUpdate {

  /// <summary>
  /// Deifne data fr a single teacher
  /// </summary>
  public interface ITeacher {

    /// <summary>
    /// Save last error encountered
    /// </summary>
    Exception LastException {get;}

    /// <summary>
    /// Flag if the teacher data has changed
    /// </summary>
    bool IsDirty {get; set;}

    /// <summary>
    /// Unique ID for teacher
    /// </summary>
    Int32 ID {get; }

    /// <summary>
    /// instructor's first/given name
    /// </summary>
    string FirstName {get;} 

    /// <summary>
    /// instructor's last/family name
    /// </summary>
    string LastName {get;} 

    /// <summary>
    /// type of classes they teach
    /// </summary>
    string Department {get;} 

    /// <summary>
    /// Get the teacher's full name
    /// </summary>
    /// <param name="LastFirst">return last,first format instead of first last</param>
    /// <returns>Full name</returns>
    string FullName(bool LastFirst=false);

  }
}
