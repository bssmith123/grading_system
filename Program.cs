using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Linq;

//a note to anyone reading this, I know best practice is to split the classes up between files and import/export them
//however I am deciding to keep them all in one place to keep it easy to quickly scan and read during QA grading

namespace Tap_university_tech_test
{
    internal class Program
    {
        //class that will hold the 'key value' pairs of class name and class grade per examanii
        public class ClassGrades
        {
            //private fields that will direclty hold the class name and grade
            private string className;
            private int classGrade;

            //constructor for creating the class
            public ClassGrades(string className, int classGrade)
            {
                this.className = className;
                this.classGrade = classGrade;
            }

            //two public methods for setting or getting the name and grade
            public string ClassName
            {
                get { return className; }
                set { className = value; }
            }
            public int ClassGrade
            {
                get { return classGrade; }
                set { classGrade = value; }
            }
        }

        //this class is functionally the same as the above, it holds the key value pairs for the character that represents a major, and the name of the major
        public class MajorList
        {
            private string majorCharacter;
            private string majorName;

            public MajorList(string majorCharacter, string majorName)
            {
                this.majorCharacter = majorCharacter;
                this.majorName = majorName;
            }

            public string MajorCharacter
            {
                get { return majorCharacter; }
                set { majorCharacter = value; }
            }

            public string MajorName
            {
                get { return majorName; }
                set { majorName = value; }
            }
        }


        static void Main(string[] args)
        {
            //creating instances of other files so that their methods can be used
            
            PopulateItems settingsInitialization = new PopulateItems();
            AddDeleteClass addDeleteClass = new AddDeleteClass();
            DeleteMajor deleteMajor = new DeleteMajor();
            GetLastValue getLastValues = new GetLastValue();
            AddMajor addMajor = new AddMajor();
            Display userSettingsDisplay = new Display();
            AddClassToMajor addClassToMajor = new AddClassToMajor();
            DeleteClassFromMajor deleteClassFromMajor = new DeleteClassFromMajor();
            UpdatePoints updatePoints = new UpdatePoints();


            //a list that essentially acts as a key value pair for class and grades
            List<ClassGrades> ClassGradesList = new List<ClassGrades>();
            List<MajorList> majorList = new List<MajorList>();
            List<string[]> majorClassGroup = new List<string[]>();
            
            


            settingsInitialization.GenerateClassList(ref ClassGradesList); //populates the class list based on what exists in the text document
            settingsInitialization.GenerateMajorList(ref majorList); //populate the major list based on the text document
            settingsInitialization.GenerateMajorClassGroupList(ref majorClassGroup); //populate the major/class groupings based on the document

            int userTotalPassPoints = settingsInitialization.GenerateTotalPoints(); //setting total points to the number in the document
            int userTotalMajorPassPoints = settingsInitialization.GenerateTotalMajorPoints(); //setting major points to the number in the document

            //this is a safety initalization, if this conditional runs, it means one or more of the groups were empty because the user edited the file
            //first it will call a function that simply copies information from another text document into the one that is used in all the logic (to make sure it exists)
            //then it will manually set variables here so that the program can run without needing to be restarted
            if (ClassGradesList.Count == 0 || majorList.Count == 0 || majorClassGroup.Count == 0)
            {
                settingsInitialization.EmergencyLoad(); //load a backup of the text file

                //erases any partially loaded information that was stored in the document
                ClassGradesList.Clear();
                majorList.Clear();
                majorClassGroup.Clear();

                //initalize all variables so that they can be used without restarting the program
                ClassGradesList.Add(new ClassGrades("ENGLISH", 0));
                ClassGradesList.Add(new ClassGrades("MATH", 0));
                ClassGradesList.Add(new ClassGrades("SCIENCE", 0));
                ClassGradesList.Add(new ClassGrades("JAPANESE", 0));
                ClassGradesList.Add(new ClassGrades("GEOGRAPHY/HISTORY", 0));

                majorList.Add(new MajorList("s", "SCIENCE"));
                majorList.Add(new MajorList("l", "HUMANITIES"));

                majorClassGroup.Add( new string[] { "SCIENCE", "MATH", "SCIENCE" });
                majorClassGroup.Add(new string[] { "HUMANITIES", "JAPANESE", "GEOGRAPHY/HISTORY" });

                userTotalPassPoints = 350;
                userTotalMajorPassPoints = 160;

                Console.WriteLine("If you are seeing this something went wrong with the intialization of the information.");
                Console.WriteLine("The file \"UserSettings.txt\" might have been altered and is no longer compatible.");
                Console.WriteLine("Loading back up...");

            }

            
            userSettingsDisplay.UserSettings(); //displays the settings by calling the usersettings function
            




            //these two strings will hold the an input that will then be used handle the exception of the user typing a word instead of a number
            string userTotalPassPointsSTR = "";
            string userTotalMajorPassPointsSTR = "";
            
            //string that will hold the answer to rather the user wants to change the settings
            string settingsAnswer;

            

            
            //asks the user if they are okay with these settings, or if the would like to change them.
            Console.WriteLine("\nAre these settings okay?\n'YES' to continue, 'NO' to change something");
            settingsAnswer = Console.ReadLine().ToUpper().Trim(); //gets input and makes it uppercase

            //while answer is YES and not NO, re-ask the user stating what they entered was not a valid response (input)
            while (settingsAnswer != "YES" && settingsAnswer != "NO")
            {
                Console.WriteLine("That was not a valid input.\nAre the settings okay?\n'YES' to continue, 'NO' to change something");
                settingsAnswer = Console.ReadLine().ToUpper().Trim();

            }

            string changeSettingsString; //string that will store the input for what setting they want to change
            int changeSettingsNum; //a num that will be used in a switch case to determine what settings to change (dirived from the changeSettingsString string

            //if the user answered no, it will prompt them to select a number to change a setting, items are self explanitory by looking at the list
            while (settingsAnswer == "NO")
            {
            Begin_settings: //label for the beginning of the settings that will be used with a 'goto' call later
                Console.WriteLine("\nWhat setting would you like to change?");
                Console.WriteLine("1 - Add a class");
                Console.WriteLine("2 - Delete a class");
                Console.WriteLine("3 - Add a Major");
                Console.WriteLine("4 - Delete a Major");
                Console.WriteLine("5 - Change classes associated with a major");
                Console.WriteLine("6 - Change the total amount of points needed to pass");
                Console.WriteLine("7 - Change the amount of points needed to pass the Major");
                Console.WriteLine("8 - I do not wish to change anything (proceed to grading)");
                

                //get the user input, and use int.TryParse to tell if it is a number or not, if so it gets saved as a var for a switch statement, if not go back to the beginning of the settings
                changeSettingsString = Console.ReadLine();
                if(!int.TryParse(changeSettingsString, out changeSettingsNum))
                {
                    Console.WriteLine("Sorry that was not a number, please try again");
                    goto Begin_settings;
                }

                
                //switch case for determining what setting to change
                //I would like to add more cases that will seperate out adding/deleting things, to make the experinece easier/simpler
                switch (changeSettingsNum)
                {
                    case 1: //adds a class
                        Console.WriteLine("Please write the class you would like to add: ");
                        string addClassName = Console.ReadLine().ToUpper(); //gets the class name is sets it to all upper case

                        if(String.IsNullOrEmpty(addClassName))
                        {
                            Console.WriteLine("Sorry that is not an input.");
                            goto Begin_settings;
                        } else
                        {
                            addDeleteClass.AddClass(addClassName, ref ClassGradesList); //calls the add class function and passes the inputed name and a reference to ClassGradesList
                            userSettingsDisplay.UserSettings(); //displays the users settings again so they can see if they are happy with them
                        }
                        break; 
                    case 2: //deletes a class

                        //a quick check to see if there is only one class left to delete
                        //I am doing this to stop the user from removing the final class, which would then cause the document (and thus program) to malfunction
                        if (ClassGradesList.Count == 1)
                        {
                            Console.WriteLine();
                            Console.WriteLine("I'm sorry, it seems there is only one class left, I can't let you delete the final one.");
                            userSettingsDisplay.UserSettings();
                            goto Begin_settings;
                        }

                        Console.WriteLine();
                        Console.WriteLine("Type the class you wish to delete: "); //asks for input on which class to delete                        
                        Console.WriteLine("Here are all of the classes available.");
                        
                        foreach(var classes in ClassGradesList)
                        {
                            Console.Write(classes.ClassName + " ");
                        }

                        Console.WriteLine();
                        Console.Write("Input: ");

                        string className = Console.ReadLine().ToUpper(); //gets the input
                        addDeleteClass.DeleteClass(className, ref ClassGradesList, ref majorClassGroup); //passes that input to the delete class function
                        userSettingsDisplay.UserSettings(); //displays the updated settings that will reflect the deleted class
                        break;
                    case 3: //adds a major
                        addMajor.AddMajorFunc(ref majorList, ref majorClassGroup); //actual function call, passing in a ref to the majorList and the groupings
                        userSettingsDisplay.UserSettings(); //display the current settings
                        break;
                    case 4: //deletes a major

                        //a quick check to see if there is only one major left to delete
                        //I am doing this to stop the user from removing the final major, which would then cause the document (and thus program) to malfunction
                        if (majorList.Count == 1)
                        {
                            Console.WriteLine();
                            Console.WriteLine("I'm sorry, it seems there is only one major left, I can't let you delete the final one.");
                            userSettingsDisplay.UserSettings();
                            goto Begin_settings;
                        }

                        deleteMajor.DeleteMajorFunc(ref majorList, ref majorClassGroup); //actual function call, passing in a ref to majorList and the groupings
                        userSettingsDisplay.UserSettings(); //display the current settings
                        break;
                    case 5: //allos the user to add or delete a class from a major
                        //first, explination + getting the input
                        Console.WriteLine("Would you like to ADD class to a major, or DELETE one?");
                        Console.WriteLine("Please type \"ADD\" or \"DELETE\"");
                        string addDelete = Console.ReadLine().ToUpper().Trim();

                        //checking if what they put in was nothing, OR if it was not AND or DELETE...
                        if(String.IsNullOrEmpty(addDelete) || addDelete != "ADD" && addDelete != "DELETE")
                        {
                            Console.WriteLine(); //formating
                            Console.WriteLine("I'm sorry, that was not a valid input.");
                            goto Begin_settings;
                        }

                        //if it was the word "ADD"...
                        if(addDelete == "ADD")
                        {
                            addClassToMajor.AddClassToMajorFunc(ref majorClassGroup, ClassGradesList); //function call to add a class to the major
                            userSettingsDisplay.UserSettings(); //diplaying settings
                        }

                        //if it was "DELETE"...
                        if(addDelete == "DELETE")
                        {
                            deleteClassFromMajor.DeleteClassFromMajorFunc(ref majorClassGroup, ClassGradesList); //function call to Delete a class from a major
                            userSettingsDisplay.UserSettings(); //display settings
                        }

                        break;
                    case 6: //change the point total
                        Console.WriteLine(); //formatting
                        Console.WriteLine("Please enter a number for the total Points: ");//asks what the new point total should be
                        userTotalPassPointsSTR = Console.ReadLine(); //gets that input

                        //an if statment that will check if the input is a number, if so it will perform the function to update the settings, if not it will display a message and re-print the settings
                        if(int.TryParse(userTotalPassPointsSTR, out userTotalPassPoints))
                        {
                            userTotalPassPoints = updatePoints.UpdateUserSettingsTotalPoints(getLastValues.GetLastValueTotal(), userTotalPassPoints); //passes the number to the update number function, it also calls a fucntion that will retreive the old total from the text document
                            userSettingsDisplay.UserSettings(); //displays the updated settings that will reflected the new point total
                        } else 
                        {
                            Console.WriteLine("\nI'm sorry, you did not input a number."); //informs the user that the value they entered wasn't a number.
                            userSettingsDisplay.UserSettings(); //re-displays settings so the user can easily see what they are to make the next deicison
                        }

                        break;
                    case 7: //functionally similar to the above case, but with the major points instead of the total points, please see it for more details
                        Console.WriteLine(); //formatting
                        Console.WriteLine("Please enter a number for the points per major: ");
                        userTotalMajorPassPointsSTR = Console.ReadLine();

                        if(int.TryParse(userTotalMajorPassPointsSTR, out userTotalMajorPassPoints))
                        {
                            userTotalMajorPassPoints = updatePoints.UpdateUserSettingsMajorPoints(getLastValues.GetLastValueMajor(), userTotalMajorPassPoints);
                            userSettingsDisplay.UserSettings();
                        } else
                        {
                            Console.WriteLine("\nI'm sorry, you did not input a number.");
                            userSettingsDisplay.UserSettings();
                        }

                        break;
                    case 8:
                        Console.WriteLine(); //formatting
                        goto Begin_Student_Entry;
                    default: //catch all of not entering a number
                        Console.WriteLine("\nThat is not a valid response");
                        break;
                }

                //asks if the new settings are okay, then gets the input and if it is yes, breaks out of the loop
                Console.WriteLine("\nAre the settings okay?\n'YES' to continue, 'NO' to change something");
                settingsAnswer = Console.ReadLine().ToUpper().Trim();

                //checking if the answer is not a yes or a not, if it is neither a "NO" or "YES" (so any digits, or random words) the if loop will be entered which will send the user back to the beginning of the settings
                //if the input is "NO" this conditional will be ignored, and the loop will be restared
                //if the input is "YES" this conditional will be ignored, and the loop will /not/ be restarted
                if(settingsAnswer != "YES" && settingsAnswer != "NO")
                {
                    Console.WriteLine("I'm sorry that wasn't a Yes or no.");
                    Console.WriteLine("Going back to settings entry\n");
                    goto Begin_settings;
                }
                
                
                

            }

            //label used for the beginning of the grade entry (used to escape from the settings switch case)
            Begin_Student_Entry:

            string studentTotalString; //string for getting the number of students
            int studentTotalNumber; 
            //an int conversion of that string (since the number of students shouldn't ever be negative, nor can you have a fraction of a student I am using int)
            //unsigned int can work too (again no negative students) the issue would be the user accidently typing a negative number, so as a sort of failsafe, i'm just using int
            //an int is much larger than the tech test input cap of 1000, a short could be used, but this would change the tryParse method
            bool isNumber = false; //a bool to check if the inputed number is a number or something else (like a sting)

            string studentGrades; //this is the string that will hold the major/grades of the student in the format: 's' 1 2 3 4 5 ...            

            //a loop that will ask for the amount of students until a positive whole number is given
            do
            {
                Console.WriteLine("Please enter the amount of students you will be entering grades for: "); //asks for input
                studentTotalString = Console.ReadLine(); //gets the input as a string

                //small conditional to check if the provided string can be parsed into an integer
                //if so it will set the bool "isNumber" to true, exiting the loop, if not it will say not a valid input
                if(int.TryParse(studentTotalString, out studentTotalNumber)){
                    isNumber = true;
                } else
                {
                    Console.WriteLine("That is not a valid input.");
                }

                //small conditional to stop the loop from being exited should a number be entered that is equal to or lower than 0 (no negative students)
                if(studentTotalNumber <= 0)
                {
                    Console.WriteLine("please enter a positive whole number");
                    isNumber = false; //a 0 or less would satsify the previous coditional, so isNumber needs to be reset to false to it from exiting
                }
            } while (isNumber == false);

            //instructs the user to input the information of the students as a string that is the major letter followed by the grades
            Console.WriteLine("\nPlease enter the information for each student in the following format:\n'Major grade1 grade2 grade3 ...'\nExample: 's 10 20 30...'");

            Console.WriteLine(); //formatting

            foreach (var majorPairs in majorList)
            {
                Console.WriteLine("{0}: '{1}'", majorPairs.MajorName, majorPairs.MajorCharacter);
            }

            Console.WriteLine(); //formatting
            Console.WriteLine("These are the current classes that need grades:");

            //simple loop to write out all of the classes so the user knows how many/what order to put the grades in
            foreach (var className in ClassGradesList)
            {
                Console.Write(className.ClassName + " ");
            }

            Console.WriteLine(); //just used for spacing to make the program look nicer


            List<string> studentGradesList = new List<string>(); //a list that will store all the examinee's grades

            //label used incase a wrong amount of grades was entered
            Begin_grading:

            //a for loop that will iterate through to the maximum amount of students inputed previously
            for (int i = 0; i < studentTotalNumber; i++)
            {
                Console.WriteLine("\nStudent {0}:", (i + 1)); //just a visual display of which student to help the user if they forget
                studentGrades = Console.ReadLine().Trim(); //getting the input (major + the grades) as a string
                studentGradesList.Add(studentGrades); //adding that string to a list that will hold all of the major/grade strings
            }


            //creation of various variables that will help check the output
            string[] gradetemp; //an array of strings that will be used to hold each individual item in the created studentsGradesList above
            string[] gradetemp2; //an array that will be used to hold a modified gradeTemp to make calculations easier (more below)
            int gradeTotal = 0; //total sum of all the grades
            int majorTotal = 0; //total sum of the major specific grades
            bool totalPass = false; //bool to be used with majorPass to check if the examinee satisfies both pass conditions
            bool majorPass = false;//bool to be used with totalPass to check if the examinee satisfies both pass conditions
            int passNumber = 0; //number that will track how many users have passed the exam
            string majorStringChar; //string that will hold the major that is inputed by the user
            bool checkMajorString = false;
            int test = 0;

            //for loop that uses the length of the created list "studentGradesList" and will iterate over that (essentially we made a list that stores the amount of responses from our original input, and will iterate over it)
            for (int i = 0; i < studentGradesList.Count; i++)
            {
                
                //initalizing a temporary array by splitting the string at position studentGradesList[i]
                gradetemp = studentGradesList[i].Split(' ');
                majorStringChar = gradetemp[0]; //assigning the first value of that split arraw to a string, as it should represent the major
                gradetemp2 = gradetemp.Skip(1).ToArray(); //a second array then is created and assigned a value of the first array, but the initail value (the letter for the major) is skipped
                                                          //gradetemp2 is made so that there doens't need to be any extra caculation when comparing indices for checking grades later 

                //conditional to check if the amount of scores match the amount of classes
                if (ClassGradesList.Count != gradetemp2.Length)
                {
                    //if not, then it will print below message, and reset any variables to a default state, then goto the label marked "Begin_grading" (just before we put in the students grades)
                    Console.WriteLine("\nSorry it seems an invalid amount of grades have been entered. Amount expected: " + ClassGradesList.Count);
                    Console.WriteLine("Make sure to lead with the major for the student.");
                    Console.WriteLine("Please input again");
                    Array.Clear(gradetemp);
                    Array.Clear(gradetemp2);
                    studentGradesList.Clear();
                    majorStringChar = "";
                    goto Begin_grading;
                }

                //a for loop that iterates through the array gradetemp2...
                for (int x = 0; x < gradetemp2.Length; x++)
                {
                    //using Linq it will check if all the characters in gradetemp2(sub string x) are a digit, if not it will reset all the variables (to avoid overflow, print a message saying something was wrong, then go back to the beginning of collecting scores
                    if (!gradetemp2[x].All(char.IsDigit))
                    {
                        Console.WriteLine("\nI'm sorry, it seems one or more of the values for a major grade was not a number, please start again.");
                        Array.Clear(gradetemp);
                        Array.Clear(gradetemp2);
                        studentGradesList.Clear();
                        majorStringChar = "";
                        goto Begin_grading;
                    } 
                }


                //for loop that will iterate through the temporary array, this will be used to check the values provided
                //note, it starts at 1 here because the first value ([0]) will (should) always be a letter, not a grade
                for (int j = 0; j < ClassGradesList.Count; j++)
                {
                    //converts the string value at position gradetemp[j] to an integer, then sets that value equal to the the class grade (which is re-indexed to 0 by "j - 1")
                    //note, currently the classes are hard coded in order so this works out, if they are dynamically changed soemthing will need to be edited
                    //additional note: There is no way to assign an examinee a grade with this code, as the question does not ask the examinee to be returned in any way, as such the list simply gets reset every complete iteration
                    ClassGradesList[j].ClassGrade = Convert.ToInt32(gradetemp2[j]);

                    //Now the caculations need to be done, first the total grade
                    //while itterating through we can add up the grade totals
                    gradeTotal = gradeTotal + ClassGradesList[j].ClassGrade;


                    string majorStringSTR = ""; //this variable will hold the value of the major associated with the lowercase letter

                    //iterating over the majorList which is the key value pairs for majors (exmaple : 's' - SCIENCE
                    for(int x = 0; x < majorList.Count; x++)
                    {
                        //if the chracter we typed, matches on of the characters in the list (meaning it exists)
                        if (majorList[x].MajorCharacter == majorStringChar)
                        {
                            majorStringSTR = majorList[x].MajorName; //set majorStringSTR equal to the name that is associated with the letter
                            break; //break out of the loop to avoid overflow
                        }
                    }

                    //iterating over the list of arrays, majorClassGroup
                    for (int x = 0; x < majorClassGroup.Count; x++)
                    {
                        //nested loop at this one will be responsible for grading the major points
                        for (int y = 1; y < majorClassGroup[x].Length; y++)
                        {
                            //if both the name assigned to majorStringSTR matches one of the majors in the list AND the name of the class that is being grated matches one of the classes in the major list...
                            if (majorClassGroup[x][0] == majorStringSTR && majorClassGroup[x][y] == ClassGradesList[j].ClassName)
                            {
                                majorTotal = majorTotal + ClassGradesList[j].ClassGrade; //add the points to the major total
                            }
                        }
                    }
                }

                


                //checking if the caculated total grade is above or equal to 350
                if (gradeTotal >= userTotalPassPoints)
                {
                    totalPass = true; //if it is, set totalPass = true;
                }

                //checking if the calculated major specific grade is equal to or above 160
                if (majorTotal >= userTotalMajorPassPoints)
                {
                    majorPass = true; //if it is, set majorPass = true;
                }

                //if both totalPass and majorPass are true (thus satisfying the conditions necessary to pass the exam)
                if (totalPass == true && majorPass == true)
                {
                    //increment passnumber by 1
                    passNumber++;

                }
                

                //resetting the values to their default state that way when this for loop continues its iteration there is no value overflow
                totalPass = false;
                majorPass = false;
                gradeTotal = 0;
                majorTotal = 0;
                majorStringChar = "";
                Array.Clear(gradetemp);
                Array.Clear(gradetemp2);
            }

            

            //the number of students that passed in a given group
            Console.WriteLine("Examinees that passed: " + passNumber);

            //used to stop the application from closing upon ending
            Console.ReadLine();
            

        }
    }
}

/* Made by Brandon Smith 7/19/2023, updated 7/29/2023
       

             
Ignore this, it is for the text document incase I accidently erased everything while testing


This list can be changed manually without running the program, however the format needs to be followed.
Look to the already included items to make sure everything is correct
**************************
studentmajor='s' - SCIENCE
studentmajor='l' - HUMANITIES

majorclassgroup=SCIENCE, MATH, SCIENCE
majorclassgroup=HUMANITIES, JAPANESE, GEOGRAPHY/HISTORY

class=ENGLISH
class=MATH
class=SCIENCE
class=JAPANESE
class=GEOGRAPHY/HISTORY

totalPoints=350
majorPoints=160

          
            */