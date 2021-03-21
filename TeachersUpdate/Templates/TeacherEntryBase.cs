using System;
using System.Collections.Generic;
using System.Text;

namespace TeachersUpdate {
  
  /// <summary>
  /// Used to handle data entry
  /// </summary>
  public abstract class TeacherEntryBase : IDisposable {

  /// <summary>
  /// Color for prompt text
  /// </summary>
  protected ConsoleColor PromptColor {get; set;}
  /// <summary>
  /// Color for data entry text
  /// </summary>
  protected ConsoleColor EntryColor {get; set;}

  /// <summary>
  /// Initalize the data entry with the colors we want to use
  /// </summary>
  /// <param name="PromptColor"></param>
  /// <param name="EntryColor"></param>
  public TeacherEntryBase(ConsoleColor PromptColor,ConsoleColor EntryColor) {
  this.PromptColor=PromptColor;
  this.EntryColor=EntryColor;
  }

  /// <summary>
  /// Clean up
  /// </summary>
  public void Dispose() {
  // no dispose needed (yet)
  }

  /// <summary>
  /// Ask the user for the first name of the teacher
  /// </summary>
  /// <returns>Teacher's first name</returns>
  public abstract string GetFirstName(string DefaultValue="");

  /// <summary>
  /// Ask the user for the last name of the teacher
  /// </summary>
  /// <returns>Teacher's last name</returns>
  public abstract string GetLastName(string DefaultValue="");

  /// <summary>
  /// Ask the user for the department the teacher belongs to
  /// </summary>
  /// <returns>Teacher's department</returns>
  public abstract string GetDepartment(string DefaultValue="");

  /// <summary>
  /// Get all data for the teacher
  /// </summary>
  /// <returns>(First name,Last name,Department)</returns>
  public void GetAllData(ref string FirstName,ref string LastName,ref string Department) {
  FirstName=GetFirstName(FirstName);
  if (FirstName.Length>0) {
    LastName=GetLastName(LastName);
    if (LastName.Length>0)
      Department=GetDepartment(Department);
    }
  }

  /// <summary>
  /// Handle console I/O
  /// </summary>
  /// <param name="PromptText">The question to ask</param>
  /// <returns>The user's response</returns>
  protected string GetInput(string PromptText,string DefaultValue) {
  string sOut="";
  ConsoleColor cHold=Console.ForegroundColor;
  Console.ForegroundColor=PromptColor;
  Console.Write(PromptText);
  Console.ForegroundColor=EntryColor;
  sOut=Console.ReadLine().Trim();
  if (sOut.Length==0)
    sOut=DefaultValue;
  Console.ForegroundColor=cHold;
  return sOut;
  }

  }
}
