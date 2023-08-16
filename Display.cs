using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tap_university_tech_test
{
    internal class Display
    {
        //this fucntion will display the user settings based on the text document that stores the information
        public void UserSettings()
        {
            Console.WriteLine(); //used for spaceing to make it look pretty

            //creates a new stream reader and says to read the associated text document
            StreamReader sr = new StreamReader("./info/UserSettings.txt");
            string line; //string that will contain the line we are on in the document
            string tempLine = ""; //a temp string used to modify information
            line = sr.ReadLine(); //read the first line of the document

            //while there is information left in the document (it hasn't reached the end)
            while (line != null)
            {


                //all of these if statements are just displaying information
                //essentially we check if the line starts with some information
                //then split the line so we can contantinate it to display information.
                //(then reset templine so it doesn't cause an issue)
                if (line.StartsWith("studentmajor"))
                {
                    tempLine = line.Split("=")[1];
                    Console.WriteLine("Available Major: " + tempLine);
                    tempLine = "";
                }

                if(line.StartsWith("majorclassgroup"))
                {
                    tempLine = line.Split("=")[1];
                    Console.WriteLine("Current Major/class pairs (major, followed by classes): " + tempLine);
                    tempLine = "";
                }

                if (line.StartsWith("class"))
                {
                    tempLine = line.Split("=")[1];
                    Console.WriteLine("Class to be graded: " + tempLine);
                    tempLine = "";
                }

                if (line.StartsWith("totalPoints"))
                {
                    tempLine = line.Split("=")[1];

                    Console.WriteLine("The total amount of points needed to pass is: " + tempLine);
                    tempLine = "";
                }

                if (line.StartsWith("majorPoints"))
                {
                    tempLine = line.Split("=")[1];
                    Console.WriteLine("The total amount of points needed to pass a major is: " + tempLine);
                    tempLine = "";
                }
                line = sr.ReadLine(); //reads the next line

            }

            sr.Close(); //closes the reader

        }
    }
}
