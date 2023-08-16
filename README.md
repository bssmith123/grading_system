# grading_system
system used to input grades and calculate a total, also allows you to add/delete a major, as well as adjust the classes assigned to a major, and the points required to pass

NOTE FOR THIS PROGRAM TO FUNCTION THE FOLLOWING NEEDS TO EXIST:
"UserSettings.txt" MUST be placed in a subfolder named "info" one level below the application. This is because this text document stores all the information that is inputed by the user
and it will persist between sessions of using the application (essentially it saves the information), and it is looking for "./info/UserSettings.txt"
Thus if it is named anything else or placed anywhere else, it will not function properly.

safeload.txt is not a necessary file, however it exists in case something with UserSettings.txt is wrong, and it will read from safeload and write the information bck to UserSettings. It is recommended to have this file
thought strictly speaking not manditory.
