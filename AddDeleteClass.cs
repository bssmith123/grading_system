using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tap_university_tech_test.Program;

namespace Tap_university_tech_test
{
    internal class AddDeleteClass
    {


        //this fucnction will delete a class from the class list
        public void DeleteClass(string className, ref List<ClassGrades> scoreList, ref List<string[]> majorClassGroup)
        {
            //create a list of strings and sets it equal to all the lines from the text document
            List<string> itemsInFile = File.ReadAllLines("./info/UserSettings.txt").ToList();

            bool containsWord = false; //bool that will be used to check if the list contains the word (class) provided by the user
            string classString = "class="; //a string variable that will simply contain the first half of an identifier for a longer string

            //iterate through every line in the text document...
            for (int i = 0; i < itemsInFile.Count; i++)
            {
                //if one of the lines starts with classString AND the information after that matches /exactly/ with what was typed by the user...
                if (itemsInFile[i].StartsWith(classString) && itemsInFile[i].Split("=")[1] == className)
                {
                    containsWord = true; //containWord is set to true
                    itemsInFile.RemoveAt(i); //remove the line from the text document
                    break; //break out of the loop
                }
            }

            //if containsWord is true (meaning we previously matched the word provided by the user
            if (containsWord)
            {
                //iterate through the refernced list of classes
                for (int i = 0; i < scoreList.Count; i++)
                {
                    //and if the name of the class matches what the user input...
                    if (scoreList[i].ClassName == className)
                    {
                        scoreList.RemoveAt(i); //remove that entry
                        break; //break out of the loop
                    }
                }

                
                //since the class is deleted, it would also make sense to remove it from the major's that have it.

                List<string> tempStringList = new List<string>(); //a list that will be used to alter data for majorClassGroup

                //iterate through the majorClassGroup list
                for(int i = 0; i < majorClassGroup.Count; i++)
                {
                    //then iterate through each element of the array within majorClassGroup (this is the major + class groupings)
                    for (int j = 1; j < majorClassGroup[i].Length; j++)
                    {
                        //if it finds a match for what the user typed...
                        if (majorClassGroup[i][j] == className)
                        {
                            tempStringList = majorClassGroup[i].ToList(); //convert that specific array (majorClassGroup[i]) to a list, then...
                            tempStringList.RemoveAt(j); //remove the element that is the className (in this case it would be stored at index 'j' because that is where it was found, then...
                            majorClassGroup[i] = tempStringList.ToArray(); //reset the array (majorClassGroup[i]) to the tempString (the array that has had the class removed), setting it back to an array (with .ToArray())
                            //affectively the above code allowed me to remove an element of an array completely, as in not setting it to null or ""

                            string tempSTR = ""; //an empty string that will hold the text to be replaced in the txt document

                            //iterate through the majorClassGroup[i]'s length (this is the now altered array)
                            for (int k = 0; k < majorClassGroup[i].Length ; k++)
                            {
                                tempSTR = tempSTR + majorClassGroup[i][k] + ", "; //setting tempSTR to itself + each element of an array + a comma
                            }

                            //iterate through the txt document...
                            for (int x = 0; x < itemsInFile.Count; x++)
                            {
                                //when it its the first instance of a line that says "majorclassgroup="...
                                if (itemsInFile[x].StartsWith("majorclassgroup="))
                                {
                                    //use the position of itemsInFile + the index of the majorClassGroup (this will properly calculate how far down the list we need to go in the text document and overwrite that line
                                    //with the phrase "majorclassgroup=" + tempSTR, where tempSTR aslo has any white space trimmed off, and the final comma removed.
                                    itemsInFile[i + x] = "majorclassgroup=" + tempSTR.Trim(' ').TrimEnd(',');
                                    break; //break out of the forloop, because it is no longer necessary
                                }
                            }

                            break; //break out of this for loop because only one class can be entered at a time to delete, so the job is done, and move on to the next line to see if there is another.
                        }
                    }
                }

                

            } else
            {
                //else we just write a simple "the words don't match, please check" line
                Console.WriteLine("\nI'm sorry, that doesn't appear to be a class, please check if it matches the class name exactly.");
            }

            //write the lines in the text document with the updated (or unchanged if it wasn't found) list
            File.WriteAllLines("./info/UserSettings.txt", itemsInFile);

            containsWord = false; //reset the bool to false, likely unnecessary because it will be re-initalized when the function is entered again.

        }





        //function for adding classes to the text document/list container, that will then be used to determine scores.
        public void AddClass(string className, ref List<ClassGrades> studentScore)
        {
            //list created by reading every line in the text document
            List<string> itemsInFile = File.ReadAllLines("./info/UserSettings.txt").ToList();

            //a string that will act as the line to be added by cominging the literal "class=" string + the string the user input (the class)
            string insertLine = "class=" + className;

            //for loop that will iterate over the list (the text document)
            for (int i = 0; i < itemsInFile.Count; i++)
            {
                //if an item in the list contains the phrase "class="...
                if (itemsInFile[i].Contains("class="))
                {
                    //this line will insert the text contained in "insertLine" that the position where we found the first instance of "class=" + the length of our classes list
                    //(this is done so the class always gets put at the bottom of the list)
                    itemsInFile.Insert(itemsInFile.IndexOf(itemsInFile[i]) + studentScore.Count, insertLine);
                    break; //break out of the for loop so we don't overflow information
                }
            }

            File.WriteAllLines("./info/UserSettings.txt", itemsInFile); //overwrite the text document with the new information
            studentScore.Add(new ClassGrades(className, 0)); //add the inputed class name to the student score list, as well as assigning a value of 0 to classGrade
        }


    }
}
