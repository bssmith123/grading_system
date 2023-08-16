using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tap_university_tech_test.Program;


//This function is rather large so I placed it in a file by itself, as opposed to being grouped with the add a class function

namespace Tap_university_tech_test
{
    internal class DeleteClassFromMajor
    {
        //function that will delete a class from the major
        public void DeleteClassFromMajorFunc(ref List<string[]> majorClassGroup, List<ClassGrades> classGradesList)
        {

        Begin_DeleteClassFromMajor: //label for the beginning of the function that will be used if there is an invalid input

            //formating and explaining to the user what to do
            Console.WriteLine();
            Console.WriteLine("Please type the major you would like to delete a class from.");
            Console.WriteLine("These are the current majors available.");

            //displaying all available majors
            foreach (var majors in majorClassGroup)
            {
                Console.Write(majors[0] + " ");
            }

            //formating
            Console.WriteLine();
            Console.Write("Input: ");

            string major = Console.ReadLine().ToUpper().Trim(); //getting the input, setting it to upper case, and trimming any white space
            bool containsMajor = false; //will be used to check if the major is found

            //if the input was empty, tell the user and go back to the beginning
            if (String.IsNullOrEmpty(major))
            {
                Console.WriteLine();
                Console.WriteLine("I'm sorry, it seems there was no input.");
                Console.WriteLine("Let's start over.");
                goto Begin_DeleteClassFromMajor;
            }

            //iterating through the list of arrays of strings that is the majorClassGroup
            for (int i = 0; i < majorClassGroup.Count; i++)
            {
                //if the major is found...
                if (majorClassGroup[i][0] == major)
                {
                    containsMajor = true; //sets containsMajor to true
                    break; //break out to avoid overflow
                }
            }

            //if the major was not found, explain to the user it was not, and go back to the beginning
            if (!containsMajor)
            {
                Console.WriteLine(); //formating
                Console.WriteLine("Sorry That doesn't seem to be a major.");
                Console.WriteLine("Let's start over.");
                containsMajor = false; //unnecessary reset of value, but just being cautious
                goto Begin_DeleteClassFromMajor;
            }

            //if it got this far it means the major exists

            Console.WriteLine(); //formating

            //asking for class to input, and displaying the classes that exist
            Console.WriteLine("Please enter a class to delete from the major.");

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
            string classToDelete = Console.ReadLine().ToUpper().Trim(); //getting the input, making it uppercase, then triming any whitespace off it

            bool containsClass = false; //will be used to check if the class exists

            //if no input, inform the user then start over
            if (String.IsNullOrEmpty(classToDelete))
            {
                Console.WriteLine(); //formating
                Console.WriteLine("I'm sorry, it seems there was no input.");
                Console.WriteLine("Let's start over.");
                goto Begin_DeleteClassFromMajor;
            }

            //this nested forloop will iterate over each element in the container (majorClassGroup) and every element starting at the second index to check for the classes
            //the goal is to only return containsClass as true if the class exists IN the major the user typed
            for (int i = 0; i < majorClassGroup.Count; i++)
            {
                for (int j = 1; j < majorClassGroup[i].Length; j++)
                {
                    if (majorClassGroup[i][j] == classToDelete && majorClassGroup[i][0] == major)
                    {
                        containsClass = true;
                        break;
                    }
                }
            }

            //if there is no class, explain that the class doesn't exist and start over
            if (!containsClass)
            {
                Console.WriteLine(); //formating
                Console.WriteLine("Sorry, that does not seem to be a class, or is not a class in this major. Please start over.");

                containsMajor = false;
                goto Begin_DeleteClassFromMajor;
            }

            string[] tempStringArray = { }; //declares and intializes a temp array of strings that will be used to store the array that contains the major

            List<string> tempStringList = new List<string>(); //temp list that will be used to convert the above array into a list to easily edit elements


            //iterate over the length of the majorClassGroup list...
            for (int i = 0; i < majorClassGroup.Count; i++)
            {
                //when the major is found...
                if (majorClassGroup[i][0] == major)
                {
                    //set tempStringArray to it
                    tempStringArray = majorClassGroup[i];
                    break; //break out ot avoid overflow
                }
            }

            Console.WriteLine(); //formating

            tempStringList = tempStringArray.ToList(); //convert the array to a list.


            //iterate over the new list STARTING AT ONE to avoid the major (if it is named the same as a class it will delete the major, not the class)
            for (int i = 1; i < tempStringList.Count; i++)
            {
                //when it finds the class...
                if (tempStringList[i] == classToDelete)
                {
                    //delete it
                    tempStringList.RemoveAt(i);
                    break; //break
                }
            }


            tempStringArray = tempStringList.ToArray(); //converst the list back into an array of strings


            string classesOfString = ""; //empty string that will hold the major and classes, this will just be used to rebuild the sentence in the text document

            //iterating over the array and setting it up in the string as element1, element2, element3, ...
            for (int i = 0; i < tempStringArray.Length; i++)
            {
                classesOfString = classesOfString + tempStringArray[i] + ", ";
            }

            classesOfString = classesOfString.TrimEnd(',', ' '); //removing the final comma and whitespace from the string

            string fullString = "majorclassgroup=" + classesOfString; //creating the full string (example: majorclassgroup=SCIENCE, MATH, SCIENCE)

            List<string> itemsInFile = File.ReadAllLines("./info/UserSettings.txt").ToList(); //creating a list of string to read whole text document

            //this loop is what replaces the information in the text document
            //iterating over the list created from the text document
            for (int i = 0; i < itemsInFile.Count; i++)
            {
                //if a particular line contains the phrase "majorclassgroup="...
                if (itemsInFile[i].Contains("majorclassgroup="))
                {
                    //and if the items after that equal sign starts with the phrase the user typed (the major)
                    if (itemsInFile[i].Split("=")[1].StartsWith(major))
                    {
                        itemsInFile[i] = fullString; //replace the whole string with the one built above
                        break; //break to avoid overflow
                    }
                }
            }

            //this loop is what replaces the array in majorClassGroup so that it may be accessed and used by the program.
            //iterating over majorClassGroup
            for (int i = 0; i < majorClassGroup.Count; i++)
            {
                //since we are only looking for a major we can just use [i][0] as the first element of every [i] will be the major
                //if the major matches what was typed by the user
                if (majorClassGroup[i][0] == major)
                {
                    majorClassGroup[i] = tempStringArray; //replace the whole line with out altered tempStringArray
                    break; //break
                }
            }

            File.WriteAllLines("./info/UserSettings.txt", itemsInFile); //write the items back to the text document

        }
    }
}
