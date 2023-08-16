using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tap_university_tech_test.Program;

namespace Tap_university_tech_test
{
    internal class PopulateItems
    {
        //Function that is called upon start up that will check the text document and populate the class list with what it contains
        public void GenerateClassList(ref List<ClassGrades> ClassGrades)
        {
            //createst a list that reads the whole text document
            List<string> itemsInFile = File.ReadAllLines("./info/UserSettings.txt").ToList();

            string classID = "class="; //a string that will be used to identify a class from the text document
            string tempString = ""; //temporary string that will be used for data manipulation

            //for loop that iterates through the list that contains the information in the text document
            for (int i = 0; i < itemsInFile.Count; i++)
            {
                //if a line in the text document (an entry in the list) contains the phrase "class="...
                if (itemsInFile[i].StartsWith(classID))
                {
                    tempString = itemsInFile[i].Split("=")[1]; //assign tempString to the contents of that line AFTER the equal sign (so class=ENGLISH would mean tempString == ENGLISH)
                    ClassGrades.Add(new ClassGrades(tempString, 0)); //add a new class item of ClassGrades that has both the string of the class, and the value 0, which will be assigned later
                    tempString = ""; //reset tempString so there is no overflow
                }
            }
        }
        


        //function will iterate through the text document and generate values for the major list, takes a reference to the majorList created in Main
        public void GenerateMajorList(ref List<MajorList> majorList)
        {
            List<string> itemsInFile = File.ReadAllLines("./info/UserSettings.txt").ToList(); //reading the whole text document and saving it as strings

            string majorID = "studentmajor="; //will be used to identify a major
            string tempMajorChar = ""; //temp string to hold the first letter (identifier for the major)
            string tempMajorWord = ""; //temp string to hold the major name

            //for loop that will iterate through every line in the text document
            for (int i = 0; i < itemsInFile.Count; i++)
            {
                if (itemsInFile[i].StartsWith(majorID)) //if it finds a line that starts with majorID...
                {
                    //it will then split the line, first AFTER the exqual sign, then BEFORE the hyphen, then trim the spaces and the apostrophe
                    tempMajorChar = itemsInFile[i].Split("=")[1].Split("-")[0].Trim(' ', '\'');
                    tempMajorWord = itemsInFile[i].Split("-")[1].Trim(); //also splits the line but only stores what after the hyphen (also trims it to remove spaces)
                    majorList.Add(new MajorList(tempMajorChar, tempMajorWord)); //adds the two above vairables to MajorList
                    tempMajorChar = ""; //resets to avoid overflow
                    tempMajorWord = ""; //resets to avoid overflow
                }
            }

        }



        //this function wil generate the list of majors and their associated classes from the text document
        public void GenerateMajorClassGroupList(ref List<string[]> majorClassGroup)
        {
            List<string> itemsInFile = File.ReadAllLines("./info/UserSettings.txt").ToList(); //create a list of strings that is all the information from the text document

            string majorClassID = "majorclassgroup="; //identifier for a line that will detect the major and classes
            string tempGroup = ""; //a temp string that will hold the whole line that has the information about the major/class grouping
            string[] tempGroup2; //a temporary array that will store the major and classes as individual strings when used

            //iterate through the text document line by line...
            for (int i = 0; i < itemsInFile.Count; i++)
            {
                //if it finds a line that starts with the majorClassID (which is "majorclassgroup="...
                if (itemsInFile[i].StartsWith(majorClassID))
                {
                    tempGroup = itemsInFile[i].Split("=")[1]; //get the right half of the equal sign and store it in temp group (for exmaple: "majorclassgroup=SCIENCE, MATH, SCIENCE" would store "SCIECNE, MATH, SCIECNE")
                    tempGroup2 = tempGroup.Split(','); //take tempGroup and split it at the commas (for example "SCIENCE, MATH, SCIENCE" would become ["SCIENCE", " MATH", " SCIENCE"])

                    //iterate through the length of the array just created.
                    for (int j = 0; j < tempGroup2.Length; j++)
                    {
                        tempGroup2[j] = tempGroup2[j].Trim(); //trimming the whitespace off the edges of each element
                    }
                    majorClassGroup.Add(tempGroup2); //lastly that array gets added to the reference of the array of strings list, majorClassGroup, so that it can be used later.
                }
            }
        }

        //this function will return the value associated with the string "totalPoints=###"
        public int GenerateTotalPoints()
        {
            int value; //local var to store the number

            List<string> itemsInFile = File.ReadAllLines("./info/UserSettings.txt").ToList(); //list of lines in the document

            //iterating through the list
            for (int i = 0; i < itemsInFile.Count; i++)
            {
                //if it finds a match for "totalPoints=" (and there is only one in the document
                if (itemsInFile[i].StartsWith("totalPoints="))
                {
                    //check that the number after "totalpoints" can be parsed as an int, if so that value is assigned to 'value'
                    if (int.TryParse(itemsInFile[i].Split("=")[1], out value))
                    {
                        return value; //reutrn the value
                    } 
                }
            }

            return 350; //if it can't be parsed, then the TAP test deafult value of 350 is returned instead
        }


        //this function is functionally similar to the one above, except with major points, instead of total points. please see that function for more details
        public int GenerateTotalMajorPoints()
        {
            int value;

            List<string> itemsInFile = File.ReadAllLines("./info/UserSettings.txt").ToList();

            for (int i = 0; i < itemsInFile.Count; i++)
            {
                if (itemsInFile[i].StartsWith("majorPoints="))
                {
                    if (int.TryParse(itemsInFile[i].Split("=")[1], out value))
                    {
                        return value;
                    }
                }
            }

            return 350;
        }

        //this function will run if information is missing, and then load a back of the file to insure it can work.
        public void EmergencyLoad()
        {
            List<string> itemsInFile = File.ReadAllLines("./info/safeload.txt").ToList();
            File.WriteAllLines("./info/UserSettings.txt", itemsInFile);
        }
    }
}
