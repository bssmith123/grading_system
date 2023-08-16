using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tap_university_tech_test
{
    internal class GetLastValue
    {

        //this function is going to get the last value of the "total" points needed to pass from the text document, that way it will always be updated when the console is launched
        public int GetLastValueTotal()
        {
            //creates a stream reader and points it to the user settings document
            StreamReader sr = new StreamReader("./info/UserSettings.txt");
            string line; //the line of the reader
            string tempLine = ""; //a temp string to be used later
            line = sr.ReadLine(); //reads the first line of the text document
            int lastValue = 0; //the last value, insitalized to 0 to avoid any accidental null references
                               //userSettings.Add(line);

            //while there is text in the text document
            while (line != null)
            {

                //checking to see if the current line starts with the phrase 'totalPoints'
                if (line.StartsWith("totalPoints"))
                {
                    tempLine = line.Split("=")[1]; //sets the temp string equal to what is after the equal sign in the line (for example totalPoints=230 means that tempLine = 230)
                    lastValue = Convert.ToInt32(tempLine); //make sure it is read as a value, not a string
                    tempLine = ""; //reset templine to an empty string
                }
                line = sr.ReadLine(); //read the next line
            }
            sr.Close(); //close the streamreader

            //return the value that was found
            return lastValue;
        }



        //this function is functionally similar to the one above, except instead of total points, this one will get the last points of the major needed to pass
        public int GetLastValueMajor()
        {

            StreamReader sr = new StreamReader("./info/UserSettings.txt");
            string line;
            string tempLine = "";
            line = sr.ReadLine();
            int lastValue = 0;

            while (line != null)
            {
                Console.WriteLine(line);
                if (line.StartsWith("majorPoints"))
                {
                    tempLine = line.Split("=")[1];
                    lastValue = Convert.ToInt32(tempLine);
                    tempLine = "";
                }
                line = sr.ReadLine();
            }
            sr.Close();

            return lastValue;
        }
    }
}
