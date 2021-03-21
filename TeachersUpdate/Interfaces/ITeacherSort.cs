using System;
using System.Collections.Generic;
using System.Text;

namespace TeachersUpdate {
  public interface ITeacherSort {

  /// <summary>
  /// Check if the first teacher is sorted correctly copared to the second
  /// </summary>
  /// <param name="Teacher1">First teacher</param>
  /// <param name="Teacher2">Second teacher</param>
  /// <returns>True if they are in order</returns>
  public bool IsInOrder(ITeacher Teacher1,ITeacher Teacher2);

  }
}
