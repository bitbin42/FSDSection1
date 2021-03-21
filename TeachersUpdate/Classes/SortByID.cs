using System;
using System.Collections.Generic;
using System.Text;

namespace TeachersUpdate {
  public class SortByID : ITeacherSort {

  /// <inheritdoc />
  public bool IsInOrder(ITeacher Teacher1,ITeacher Teacher2) {
  return Teacher1.ID<Teacher2.ID;
  }

  }
}
