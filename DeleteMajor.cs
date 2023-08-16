using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tap_university_tech_test.Program;

namespace Tap_university_tech_test
{
    internal class DeleteMajor
    {

        //This function is used to delete a major from the list.
        public void DeleteMajorFunc(ref List<MajorList> majorList, ref List<string[]> majorClassGroup)
        {
        Begin_deleteClass: //label used to mark the beginning in case an incorrect value is entered

            //explaining to the user what they should do, and displaying the majors that exist
            Console.WriteLine("Please type the name of the Major you would like to delete.");
            Console.WriteLine("These are the majors that are available.");

            foreach (var majorName in majorList)
            {
                Console.Write(majorName.MajorName + " ");
            }
            Console.WriteLine();
            Console.Write("Input: ");

            string majorToErase = Console.ReadLine().ToUpper().Trim(); //getting the input, then making it all uppercase and trimming any white space

            //if there was no input, tell the user there was none, and start over
            if (String.IsNullOrEmpty(majorToErase))
            {
                Console.WriteLine(); //formatting
                Console.WriteLine("I'm sorry, there was no input.");
                goto Begin_deleteClass;
            }

            List<string> itemsInFile = File.ReadAllLines("./info/UserSettings.txt").ToList(); //list that holds all the information from the text document

            bool containsWord = false; //used to check if the major exists
            string majorID = "studentmajor="; //identifier for finding a line in the text document that holds the major

            //iterate over all the lines in the text document
            for (int i = 0; i < itemsInFile.Count; i++)
            {
                //if the line it's checking starts with the identifier (majorID) AND the major the user entered (majorToErase) matches the information stored on the same line after the equal sign (so checking if it finds the word)...
                if (itemsInFile[i].StartsWith(majorID) && itemsInFile[i].Split("-")[1].Trim() == majorToErase)
                {
                    containsWord = true; //it has the word, so sets containsWord to true for later
                    itemsInFile.RemoveAt(i); //removes that line from the array for lines of the text document
                    break; //breaks out of the for loop to stop it from searching more
                }
            }

            //if it did find a match...
            if (containsWord)
            {
                //iterates over the list of majors...
                for (int i = 0; i < majorList.Count; i++)
                {
                    //until it fights a match by checking everyone
                    if (majorList[i].MajorName == majorToErase)
                    {
                        majorList.RemoveAt(i); //then removes it from the list
                        break; //and breaks out of the forloop
                    }
                }

                //within the coditional that we DID find a match of what the user typed and an existing major...
                //iterate over the file again...
                for (int i = 0; i < itemsInFile.Count; i++)
                {
                    //searching for a line that starts with "majorclassgroup=" and just after that equal sign contains the major the user typed
                    if (itemsInFile[i].StartsWith("majorclassgroup=") && itemsInFile[i].Split("=")[1].StartsWith(majorToErase))
                    {
                        itemsInFile.RemoveAt(i); //erase it, this is done because if they remove the major, the class grouping doesn't need to exist either
                        break; //break out to avoid overflow
                    }
                }

                //this forloop will iterate over the array of strings list, majorClassGroup, looking for a match to the major typed, and will delete it.
                for (int i = 0; i < majorClassGroup.Count; i++)
                {
                    if (majorClassGroup[i][0] == majorToErase)
                    {
                        majorClassGroup.RemoveAt(i);
                        break;
                    }
                }


            }
            else //if it did not find a match, print a "didn't match, maybe mispelling" type of message
            {
                Console.WriteLine("\nI'm sorry, that doesn't appear to be a major, please check if it matches the major name exactly");
            }

            //write the new information (or lack there of) to the text document to insure it is updated correctly
            File.WriteAllLines("./info/UserSettings.txt", itemsInFile);

            containsWord = false; //resets containsWord to false, though this is likely unessary as it goes out of scope right after
        }
    }
}
