using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersUpdate {

  /// <summary>
  /// Handles persisting teacher data
  /// </summary>
  public interface IPersist {

    /// <summary>
    /// Save last error encountered
    /// </summary>
    Exception LastException {get;}

    /// <summary>
    /// Clean up
    /// </summary>
    void Dispose();

    /// <summary>
    /// Write teacher data to persistent storage
    /// </summary>
    /// <param name="Teachers">List of Teacher objects</param>
    /// <returns>True if successful</returns>
    bool SaveTeachers(IEnumerable<ITeacher> Teachers);

    /// <summary>
    /// Read teacher data from persistent storage
    /// </summary>>
    /// <returns>List of Teacher objects</returns>
    IEnumerable<ITeacher> LoadTeachers();

  }
}
