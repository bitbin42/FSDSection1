using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io=System.IO;

namespace TeachersUpdate {
  class Program {

  private static readonly ConsoleColor INFOCOLOR=ConsoleColor.Blue;
  private static readonly ConsoleColor PROMPTCOLOR=ConsoleColor.Green;
  private static readonly ConsoleColor DATACOLOR=ConsoleColor.Yellow;
  private static readonly ConsoleColor ERRORCOLOR=ConsoleColor.Red;
  private static readonly ConsoleColor DEFAULTCOLOR=ConsoleColor.White;

  /// <summary>
  /// List of teachers we know about
  /// </summary>
  private static IFaculty moFaculty=null;

  /// <summary>
  /// Program start
  /// </summary>
  /// <param name="args"></param>
  static void Main(string[] args) {
  WriteConsole("Welcome to teacher scheduling",INFOCOLOR);
  IPersist oStorage=new FileStorage(); // use text file for I/O
  moFaculty=new Faculty(oStorage);
  if (moFaculty.LastException!=null)
    WriteConsole("Unable to initialize faculty list: "+moFaculty.LastException.Message,ERRORCOLOR);
  else {
    ShowInfo();
     WriteConsole("Use HELP for a list of commands",PROMPTCOLOR);
    while (NextCommand()) {}
    if (moFaculty.IsDirty && AskSave()) {
      WriteConsole("Saving data",INFOCOLOR);
      if (moFaculty.Save())
        WriteConsole("Updates saved",INFOCOLOR);
      else
        WriteConsole("Save failed: "+moFaculty.LastException,ERRORCOLOR);
      }
    }
  }

  /// <summary>
  /// Prompt user if they want to save changes
  /// </summary>
  /// <returns>True if save needed</returns>
  private static bool AskSave() {
  bool bSave=true;
  bool bAsk=true;
  while (bAsk) {
    WriteConsole("Do you want to save the changes? ",PROMPTCOLOR);
    string sResp=Console.ReadLine();
    switch (sResp.ToLower()) {
      case "n":
      case "no":
        bAsk=false;
        bSave=false;
        break;
      case "y":
      case "yes":
        bAsk=false;
        break;
      default:
        WriteConsole("Please answer with just YES or NO",ERRORCOLOR);
        break;
      }
    }
  return bSave;
  }

  /// <summary>
  /// Ask user for command and process it
  /// </summary>
  /// <returns>False when ready to quit</returns>
  private static bool NextCommand() {
  bool bContinue=true; // assume we continue
  WriteConsole("Action: ",PROMPTCOLOR,false);
  string sRequest=Console.ReadLine().Trim();
  if (sRequest.Length>0) {
    Int32 x=sRequest.IndexOf(' ');
    string sCommand=(x>=0?sRequest.Substring(0,x):sRequest).ToLower();
    sRequest=(x<0)?"":sRequest.Substring(x+1).Replace('\t',' ').Replace(',',' ');
    switch (sCommand) {
      case "help":
      case "?":
        ShowHelp();
        break;
      case "info":
        ShowInfo();
        break;
      case "exit":
      case "quit":
      case "end":
        bContinue=false;
        break;
      case "list":
      case "li":
      case "l":
        ListRecords(sRequest);
        break;
      case "add":
        AddTeacher(sRequest);
        break;
      case "edit":
        EditTeacher(sRequest);
        break;
      case "delete":
        DeleteTeacher(sRequest);
        break;
      case "save":
      case "sa":
        if (moFaculty.Save())
          WriteConsole("Saved",INFOCOLOR);
        else
          WriteConsole($"Save error {moFaculty.LastException.Message}",ERRORCOLOR);
        break;
      case "sort":
        SortTeachers(sRequest);
        break;
      default:
        WriteConsole("Unrecognized request; use HELP for a list of actions",ERRORCOLOR);
        break;
      }
    }
  return bContinue;
  }

  /// <summary>
  /// Delete a teacher
  /// </summary>
  /// <param name="Request">FIRST LAST</param>
  private static void DeleteTeacher(string Request) {
  string[] sParts=Request.Split(new char[]{' '},StringSplitOptions.RemoveEmptyEntries);
  if (sParts==null || sParts.Length!=1 || sParts[0].Length==0 || !Int32.TryParse(sParts[0],out Int32 lID))
    WriteConsole("Format is: Delete TEACHER_ID",ERRORCOLOR);
  else {
    ITeacher oDel=moFaculty.FindTeacher(lID);
    if (oDel==null)
      WriteConsole("That teacher is not on the list",ERRORCOLOR);
    else {
      WriteConsole($"Are you sure you want to delete {oDel.FullName()} [{oDel.Department}]? ",PROMPTCOLOR,false);
      switch (Console.ReadLine().ToLower()) {
        case "yes":
        case "y":
          moFaculty.RemoveTeacher(oDel);
          WriteConsole("Deleted",INFOCOLOR);
          break;
        default:
          WriteConsole("Not deleted",INFOCOLOR);
          break;
        }
      }
    }
  }

  /// <summary>
  /// Add a new teacher
  /// </summary>
  /// <param name="Request">FIRST LAST DEPARTMENT</param>
  private static void AddTeacher(string Request) {
  if (Request.Length>0) {
    string[] sParts=Request.Split(new char[]{' '},StringSplitOptions.RemoveEmptyEntries);
    if (sParts==null || sParts.Length!=3 || sParts[0].Length==0 ||sParts[1].Length==0 || sParts[2].Length==0)
      WriteConsole("Format is: Add {FIRSTNAME LASTNAME DEPARTMENT}",ERRORCOLOR);
    else {
      AddTeacher(sParts[0],sParts[1],sParts[2]);
      }
    }
  else {
    string sFirst="";
    string sLast="";
    string sDept="";
    TeacherEntryBase oDataEntry = new TeacherEntry(PROMPTCOLOR, DEFAULTCOLOR);
    oDataEntry.GetAllData(ref sFirst,ref sLast,ref sDept);
    AddTeacher(sFirst,sLast,sDept);
    }
  }

  /// <summary>
  /// Request new teacher object
  /// </summary>
  /// <param name="FirstName">Teacher's first name</param>
  /// <param name="LastName">Teacher's last name</param>
  /// <param name="Dept">Department name</param>
  private static void AddTeacher(string FirstName,string LastName,string Dept) {
  if (!moFaculty.AddTeacher(FirstName,LastName,Dept))
    WriteConsole("Unable to add: "+moFaculty.LastException.Message,ERRORCOLOR);
  else {
    WriteConsole("Added",INFOCOLOR);
    }
  }

  /// <summary>
  /// Edit teacher
  /// </summary>
  /// <param name="Request">ID {FIRST LAST DEPARTMENT}</param>
  private static void EditTeacher(string Request) {
  bool bFormatOK=false;
  if (Request.Length>0) {
    string[] sParts=Request.Split(new char[]{' '},StringSplitOptions.RemoveEmptyEntries);
    if (sParts.Length==1 || sParts.Length==4) {
      if (Int32.TryParse(sParts[0],out Int32 nID)) {
        bFormatOK=true;
        ITeacher oTeacher=moFaculty.FindTeacher(nID);
        if (oTeacher==null)
          WriteConsole($"Teacher ID {nID} not found",ERRORCOLOR);
        else {
          string sFirst="";
          string sLast="";
          string sDept="";
          if (sParts.Length==4) {
            sFirst=sParts[1];
            sLast=sParts[2];
            sDept=sParts[3];
            }
          else {
            sFirst=oTeacher.FirstName;
            sLast=oTeacher.LastName;
            sDept=oTeacher.Department;
            }
          TeacherEntryBase oDataEntry = new TeacherEntry(PROMPTCOLOR, DEFAULTCOLOR);
          oDataEntry.GetAllData(ref sFirst,ref sLast,ref sDept);
          if (moFaculty.UpdateTeacher(nID,sFirst,sLast,sDept))
            WriteConsole("Teacher updated",INFOCOLOR);
          else
            WriteConsole($"Unable to update: {moFaculty.LastException.Message}",ERRORCOLOR);
          }
        }
      }
    }
  if (!bFormatOK)
    WriteConsole("Format is: Edit Teacher_ID {First Last Dept}",ERRORCOLOR);
  }

  /// <summary>
  /// Display list of records
  /// </summary>
  /// <param name="Request">TEACHERS, or DEPARTMENTS</param>
  private static void ListRecords(string Request) {
  switch (Request.ToLower()) {
    case "teachers":
    case "teacher":
    case "teach":
    case "t":
    case "":
      foreach (ITeacher oTeacher in moFaculty.Teachers)
        WriteConsole($"{oTeacher.ID}: {oTeacher.FullName(true)} [{oTeacher.Department}]",DATACOLOR);
      WriteConsole($"Teachers found: {moFaculty.Teachers.Count()}",INFOCOLOR);
      break;
    case "departments":
    case "depts":
    case "dept":
    case "d":
      foreach (string sDept in moFaculty.Departments)
        WriteConsole("Department: "+sDept,DATACOLOR);
      WriteConsole($"Departments found: {moFaculty.Departments.Count()}",INFOCOLOR);
      break;
    default:
      WriteConsole("LIST TEACHERS | DEPARTMENTS",ERRORCOLOR);
      break;
    }
  }

  /// <summary>
  /// Alow sorting the list of teachers
  /// </summary>
  private static void SortTeachers(string Request) {
  Request=Request.ToLower();
  if (Request.Length>3 && (Request.StartsWith("by ") || Request.StartsWith("on ")))
    Request=Request.Substring(3);
  switch (Request.ToLower()) {
    case "first name":
    case "firstname":
    case "first":
      if (moFaculty.SortTeachers(IFaculty.SortOptions.FirstName))
        WriteConsole("Sorted by first name",INFOCOLOR);
      else
        WriteConsole($"Sort failed: {moFaculty.LastException.Message}",ERRORCOLOR);
      break;
    case "last name":
    case "lastname":
    case "last":
      if (moFaculty.SortTeachers(IFaculty.SortOptions.LastName))
        WriteConsole("Sorted by last name",INFOCOLOR);
      else
        WriteConsole($"Sort failed: {moFaculty.LastException.Message}",ERRORCOLOR);
      break;
    case "id number":
    case "idnumber":
    case "number":
    case "id":
      if (moFaculty.SortTeachers(IFaculty.SortOptions.ID))
        WriteConsole("Sorted by ID number",INFOCOLOR);
      else
        WriteConsole($"Sort failed: {moFaculty.LastException.Message}",ERRORCOLOR);
      break;
    default:
      WriteConsole("SORT ID|First Name|Last Name",ERRORCOLOR);
      break;
    }
  }

  /// <summary>
  /// Display help info
  /// </summary>
  private static void ShowHelp() {
  WriteConsole("HELP or ? -- displays this list",INFOCOLOR);
  WriteConsole("INFO -- displays the count of records on file",INFOCOLOR);
  WriteConsole("ADD {FIRSTNAME LASTNAME DEPARTMENT} -- adds a new teacher",INFOCOLOR);
  WriteConsole("DELETE ID -- deletes a teacher",INFOCOLOR);
  WriteConsole("EDIT ID {FIRSTNAME LASTNAME DEPARTMENT} -- edits teacher information",INFOCOLOR);
  WriteConsole("LIST {TEACHERS} -- lists all teachers",INFOCOLOR);
  WriteConsole("LIST DEPARTMENTS -- lists all departments",INFOCOLOR);
  WriteConsole("SAVE -- saves changes",INFOCOLOR);
  WriteConsole("EXIT or QUIT or END -- exits the program",INFOCOLOR);
  }

  /// <summary>
  /// Display counts
  /// </summary>
  private static void ShowInfo() {
  WriteConsole($"Teachers on file: {moFaculty.Teachers.Count()}",INFOCOLOR);
  WriteConsole($"Departments: {moFaculty.Departments.Count()}",INFOCOLOR);
  }

  /// <summary>
  /// Write colored text to the console
  /// </summary>
  /// <param name="TheText">Text to write</param>
  /// <param name="TextColor">Color for the text</param>
  /// <param name="AddCRLF">True to add CR\LF after text</param>
  private static void WriteConsole(string TheText,ConsoleColor TextColor,bool AddCRLF=true) {
  Console.ForegroundColor=TextColor;
  if (AddCRLF)
    Console.WriteLine(TheText);
  else
    Console.Write(TheText);
  Console.ForegroundColor=DEFAULTCOLOR;
  }


}
}
