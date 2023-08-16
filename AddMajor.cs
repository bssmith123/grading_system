using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tap_university_tech_test.Program;

namespace Tap_university_tech_test
{
    internal class AddMajor
    {
        //function to add a Major, takes a reference to the major list created in Main
        public void AddMajorFunc(ref List<MajorList> majorList, ref List<string[]> majorClassList)
        {
            Console.WriteLine(); //just used for spacing
            Begin_AddMajor: //label to mark the beginning of the input in case something is not correct

            //explaining what we are looking for, and then displaying all the current lower case letters being used to represent majors
            Console.WriteLine("please enter a single lower case character to represent the major.");
            Console.WriteLine("These are the characters already in use: ");
            foreach (var majorChar in majorList)
            {
                Console.Write(majorChar.MajorCharacter + " ");
            }

            //formaing and requesting input
            Console.WriteLine();
            Console.Write("Input: ");

            string newMajorChar = ""; //string that will hold the single lower case letter that represents the major

            newMajorChar = Console.ReadLine().ToLower(); //getting that letter, and making it lowercase just in case the user put an upper case letter

            //first check is if there was no (or null) input, if so, go back to the beginning
            if (String.IsNullOrEmpty(newMajorChar))
            {
                Console.WriteLine();
                Console.WriteLine("Sorry it seems there was no input.");
                goto Begin_AddMajor;
            }

            //second check is to confirm it was only one character long, if not, go back to the beginning
            if (newMajorChar.Length != 1)
            {
                Console.WriteLine();
                Console.WriteLine("Sorry it seems there was more than one character.");
                goto Begin_AddMajor;
            }

            //third check is to make sure it wasn't a digit, if so, go back to the beginning
            if (newMajorChar.All(char.IsDigit))
            {
                Console.WriteLine();
                Console.WriteLine("Sorry, it seems you have entered a number, please enter a single lower case character");
                goto Begin_AddMajor;
            }


            //a forloop to iterate over majorList's length (Count)...
            for (int i = 0; i < majorList.Count; i++)
            {
                //if a major character is found (this means it's already in use), go back to the beginning
                if (majorList[i].MajorCharacter == newMajorChar)
                {
                    Console.WriteLine("Sorry, it seems that character is already in use.");
                    goto Begin_AddMajor;
                }
            }

            //if none of the above are triggered the input should be a single character, in that case we condinue on.


            //formating and asking for a word to associate the major with.
            Console.WriteLine();
            Console.WriteLine("Please type a Major Name");
            Console.WriteLine("These are the majors that already exist: ");

            //displaying all the words to show what is already in use
            foreach (var majorWord in majorList)
            {
                Console.Write(majorWord.MajorName + " ");
            }
            Console.WriteLine();
            Console.Write("Input: ");

            string newMajorName = Console.ReadLine().ToUpper().Trim(); //getting it, and trimming any excess whitespace off the outside

            //if nothing was input, go back to the beginning
            if (String.IsNullOrEmpty(newMajorName))
            {
                Console.WriteLine();
                Console.WriteLine("Sorry, you must input a major name.");
                goto Begin_AddMajor;
            }

            //iterating over the marjorList's lenght...
            for (int i = 0; i < majorList.Count; i++)
            {
                //if the name already exists, go back to the beginning.
                if (majorList[i].MajorName == newMajorName)
                {
                    Console.WriteLine();
                    Console.WriteLine("Sorry that major name is in use.");
                    goto Begin_AddMajor;
                }
            }

            Console.WriteLine(); //formatting

            //if the program has gotten this far then both the character and name are valid and thus we update the text document, and add the new major character/name pair to the list.

            List<string> itemsInFile = File.ReadAllLines("./info/UserSettings.txt").ToList();

            string insertLine = "studentmajor='" + newMajorChar + "' - " + newMajorName; //building the string to insert into the text document

            //iterating over all the lines in the text document...
            for (int i = 0; i < itemsInFile.Count; i++)
            {
                //at the first instance of "studentMajor="...
                if (itemsInFile[i].Contains("studentmajor="))
                {
                    itemsInFile.Insert(itemsInFile.IndexOf(itemsInFile[i]) + majorList.Count, insertLine); //insert insertLine at the index of the line we found 'studentmajor=' + the lenght of the list (to put it at the end)
                    break; //break out because the forloop is unneeded beyond this
                }
            }


            //since a new major is being added, it makes sense to also make a new major/class grouping

            string insertLineMajorClass = "majorclassgroup=" + newMajorName; //creating the line that will be inserted to the majorclassgroup section of the text document

            //iterate over all the lines in the file...
            for (int i = 0; i < itemsInFile.Count; i++)
            {
                //when it finds the first one containing "majorclassgroup="...
                if (itemsInFile[i].Contains("majorclassgroup="))
                {
                    itemsInFile.Insert(itemsInFile.IndexOf(itemsInFile[i]) + majorClassList.Count, insertLineMajorClass); //insert the line at the position of that line, plus the total number of lines in majorClassList (to put it at the end)
                    break; //break out ot avoid overflow
                }
            }

            string[] insertLineMajorClassArr = { newMajorName }; //since majorClassList is a list of arrays, we need an array that holds, for now, only the major name



            File.WriteAllLines("./info/UserSettings.txt", itemsInFile); //actually updating the text document
            majorList.Add(new MajorList(newMajorChar, newMajorName)); //adding the information to the list
            majorClassList.Add(insertLineMajorClassArr);

        }

    }
}
