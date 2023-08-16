using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tap_university_tech_test
{
    internal class UpdatePoints
    {

        //this fucntion will update the total Major points needed to pass the exam, it accepts an oldvalue (that was already writen), and a newvalue (the target)
        public int UpdateUserSettingsMajorPoints(int oldValue, int newValue)
        {
            //creates a string called text that holds all the informatio in the text document
            string text = File.ReadAllText("./info/UserSettings.txt");
            string oldString = "majorPoints=" + oldValue; //value of the old string in the text document
            string newString = "majorPoints=" + newValue; //value of the new string in the text document (what will replace the old one)
            text = text.Replace(oldString, newString); //replaces the text
            File.WriteAllText("./info/UserSettings.txt", text); //rewrites all of the infomration to the text document

            return newValue;
        }

        //this function is functionally identical to the one above, except it does total points, not major points, please see the above function for more details
        public int UpdateUserSettingsTotalPoints(int oldValue, int newValue)
        {
            string text = File.ReadAllText("./info/UserSettings.txt");
            string oldString = "totalPoints=" + oldValue;
            string newString = "totalPoints=" + newValue;
            text = text.Replace(oldString, newString);
            File.WriteAllText("./info/UserSettings.txt", text);

            return newValue;
        }

    }
}
