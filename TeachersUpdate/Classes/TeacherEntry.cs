using System;
using System.Collections.Generic;
using System.Text;

namespace TeachersUpdate {
  public class TeacherEntry : TeacherEntryBase {

    /// <inheritdoc />
    public TeacherEntry(ConsoleColor PromptColor, ConsoleColor EntryColor) : base(PromptColor, EntryColor) {}

    /// <inheritdoc />
    public override string GetDepartment(string DefaultValue="") {
    return GetInput("What department does this teacher belong to? "+((DefaultValue.Length==0)?"":$"[{DefaultValue}] "),DefaultValue);
    }

    /// <inheritdoc />
    public override string GetFirstName(string DefaultValue="") {
    return GetInput("What is the first name of the teacher? "+((DefaultValue.Length==0)?"":$"[{DefaultValue}] "),DefaultValue);
    }

    /// <inheritdoc />
    public override string GetLastName(string DefaultValue="") {
    return GetInput("What is the last name? "+((DefaultValue.Length==0)?"":$"[{DefaultValue}] "),DefaultValue);
    }

  }
}
