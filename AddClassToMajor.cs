using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tap_university_tech_test.Program;


//This function is rather large, so I am placing it in a file by itself instead of grouping it with the delete function

namespace Tap_university_tech_test
{
    internal class AddClassToMajor
    {
        //fucntion used to add class to major
        public void AddClassToMajorFunc(ref List<string[]> majorClassGroup, List<ClassGrades> classGradesList)
        {
        //label for start of the function that will be used if an invalid input is made
        Begin_AddClassToMajor:

            //exlpaining to user how to enter the information
            Console.WriteLine();
            Console.WriteLine("Please type the major you would like to add a class to.");
            Console.WriteLine("These are the current majors available.");

            //displaying majors that exist
            foreach (var majors in majorClassGroup)
            {
                Console.Write(majors[0] + " ");
            }

            Console.WriteLine(); //formating
            Console.Write("Input: ");


            string major = Console.ReadLine().ToUpper().Trim(); //getting input from user, making it uppercase, and trimming whitespace
            bool containsMajor = false; //bool that will be used to check if the major exists

            //if no input, inform the user and start over
            if (String.IsNullOrEmpty(major))
            {
                Console.WriteLine(); //formating
                Console.WriteLine("I'm sorry, it seems there was no input.");
                Console.WriteLine("Let's start over.");
                goto Begin_AddClassToMajor;
            }

            //iterates through the list of majors to see if the entered one exists
            for (int i = 0; i < majorClassGroup.Count; i++)
            {
                if (majorClassGroup[i][0] == major)
                {
                    containsMajor = true;
                    break;
                }
            }

            //if not inform the user what the entered wasn't a major and have them start over
            if (!containsMajor)
            {
                Console.WriteLine(); //formating
                Console.WriteLine("Sorry That doesn't seem to be a major.");
                Console.WriteLine("Let's start over.");
                containsMajor = false; //unnecessary reset of value, but just being cautious
                goto Begin_AddClassToMajor;
            }

            //if it got this far it means the major exists

            Console.WriteLine(); //formating

            //asking for class to input, and displaying the classes that exist
            Console.WriteLine("Please enter a class to add to the major.");
            Console.WriteLine("These are all of the classes currently available:");
            foreach (var classes in classGradesList)
            {
                Console.Write(classes.ClassName + " ");
            }

            Console.WriteLine(); //formating
            Console.WriteLine(); //formating

            Console.WriteLine("And here are the classes associated with the {0} major:", major); //line to remind the user what is happening

            //ths loop will ultiamtely iterate over each major, but only display the classes associated with the major the user entered.
            for (int i = 0; i < majorClassGroup.Count; i++)
            {

                for (int j = 1; j < majorClassGroup[i].Length; j++)
                {
                    if (majorClassGroup[i][0] == major)
                    {
                        Console.Write(majorClassGroup[i][j] + " ");
                    }

                }
            }


            Console.WriteLine(); //formating
            Console.Write("Input: ");
            string classToAdd = Console.ReadLine().ToUpper().Trim(); //getting the input, making it uppercase, then triming any whitespace off it

            bool containsClass = false; //will be used to check if the class exists

            //if no input, inform the user then start over
            if (String.IsNullOrEmpty(classToAdd))
            {
                Console.WriteLine(); //formating
                Console.WriteLine("I'm sorry, it seems there was no input.");
                Console.WriteLine("Let's start over.");
                goto Begin_AddClassToMajor;
            }

            //iterating through to see if class exists already, here I am using the value instead of a reference because I am not changing anything and don't want to accidently overwrite information.
            for (int i = 0; i < classGradesList.Count; i++)
            {
                if (classGradesList[i].ClassName == classToAdd)
                {
                    containsClass = true;
                    break;
                }
            }

            //if the class doesn't exist inform the user, set containsMajor to false (unnecessary I believe), then go back to the beginning
            if (!containsClass)
            {
                Console.WriteLine(); //formating
                Console.WriteLine("Sorry, that does not seem to be a class, please start over.");
                containsMajor = false;
                goto Begin_AddClassToMajor;
            }

            //Check to see if the class is already associated with the major
            for (int i = 0; i < majorClassGroup.Count; i++)
            {
                for (int j = 1; j < majorClassGroup[i].Length; j++) //this is nested becuse the outter is essentially iterating through every major, and the inner through every class
                {
                    if (majorClassGroup[i][0] == major && majorClassGroup[i][j] == classToAdd) //if both the major matches the typed major AND the the class that was typed matches a class already added to the major...
                    {
                        Console.WriteLine(); //formating
                        Console.WriteLine("I'm sorry, that class is already a part of the major."); //informing use it's already there.
                        Console.WriteLine("Exiting this operation, because it already exists."); //exiting the whole function as opposed to going back to the beginning
                        return;
                    }
                }

            }


            //creating a list of string to read whole text document
            List<string> itemsInFile = File.ReadAllLines("./info/UserSettings.txt").ToList();

            string existingLine = ""; //line that will be used to re-create the line in the document

            //iterating through the length of the majors/class grouping
            for (int i = 0; i < majorClassGroup.Count; i++)
            {
                //if the major we collected from the user exists...
                if (majorClassGroup[i][0] == major)
                {
                    //iterate through that array...
                    for (int j = 0; j < majorClassGroup[i].Length; j++)
                    {
                        existingLine = existingLine + majorClassGroup[i][j].Trim() + ", "; //and recreate the line, with an added ", " at the end
                    }
                }
            }

            //this is the line that will be inserted into the document
            //it is being recreated with the prefix "majorclassgroup=" and the information we collected/created above
            string insertLine = "majorclassgroup=" + existingLine + classToAdd.TrimEnd();



            //iterates through all the lines in the text document...
            for (int i = 0; i < itemsInFile.Count; i++)
            {
                //if it finds one that starts with "majorclassgroup="...
                if (itemsInFile[i].Contains("majorclassgroup="))
                {
                    //then if the line it found has the corrosponding major just after the equal sign...
                    if (itemsInFile[i].Split("=")[1].StartsWith(major))
                    {
                        //replace that line, since we are replacing the whole line and not inserting anything AND have built the replacement line, we can just do this.
                        itemsInFile[i] = insertLine;
                        break; //break out of the loop so no overflow.
                    }
                }
            }


            Console.WriteLine(); //formating

            //informing the user that the add was successful by displying that it is being added.
            Console.WriteLine("Adding the class {0} to the major {1}", classToAdd, major);

            //re-writing all the lines to update the text document
            File.WriteAllLines("./info/UserSettings.txt", itemsInFile);

            //this is actualy appening the new class to the end of our list to be used later during calculations
            for (int i = 0; i < majorClassGroup.Count; i++)
            {
                if (majorClassGroup[i][0] == major)
                {
                    majorClassGroup[i] = majorClassGroup[i].Append(classToAdd).ToArray();
                    break;
                }
            }

        }
    }
}
