using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace _178390_AndreaBriffa_DDOOCP
{
    public static class Library
    {
        ///////////////////               Static Fields               /////////////////////
        
        public static string usersDirectory = Directory.GetCurrentDirectory().ToString() + @"\data\users.xml";
        public static string languagesDirectory = Directory.GetCurrentDirectory().ToString() + @"\data\languages.xml";
        public static string gameDirectory = Directory.GetCurrentDirectory().ToString() + @"\data\games.xml";
        public static string backupLanguagesDirectory = Directory.GetCurrentDirectory().ToString() + @"\data\backupLanguages.xml";
        public static string iconDirectory = Directory.GetCurrentDirectory().ToString() + @"\images\icon.ico";

        ///////////////////               Delegates                   /////////////////////

        public delegate List<T> readFile<T>();
        public delegate void writeFile<T>(List<T> list);
        public delegate void appendFile<T>(T instance);

        ///////////////////               Static Methods              /////////////////////

        // Read Methods - Return List of instances from XML files
        public static readFile<User> getUsers = () =>
        {
            List<User> users = new List<User>();
            XmlSerializer xmlReader = new XmlSerializer(typeof(List<User>));
            if(File.Exists(usersDirectory))
            {
                using (FileStream textReader = File.OpenRead(usersDirectory))
                {
                    users = (List<User>)xmlReader.Deserialize(textReader);
                    textReader.Dispose();
                    textReader.Close();
                }
            }
            else
            {
                Console.WriteLine("\nUsers' File does not exist\nUnable to retrieve List - Testing\n");
                Console.WriteLine("\nReturning Empty List of Users - Testing\n");
            }
            return users;
        };// Returns List of Stored Users
        public static readFile<Language> getLanguages = () =>
        {
            List<Language> languages = new List<Language>();
            XmlSerializer xmlReader = new XmlSerializer(typeof(List<Language>));
            if(File.Exists(languagesDirectory))
            {
                using (FileStream textReader = File.OpenRead(languagesDirectory))
                {
                    languages = (List<Language>)xmlReader.Deserialize(textReader);
                    textReader.Dispose();
                    textReader.Close();
                }
            }
            else
            {
                writeInitialLanguages();
                languages = getLanguages();
            }
            return languages;
        };// Returns List of Stored Languages
        public static readFile<GameRecord> getGames = () =>
        {
            List<GameRecord> games = new List<GameRecord>();
            XmlSerializer xmlReader = new XmlSerializer(typeof(List<GameRecord>));
            if(File.Exists(gameDirectory))
            {
                using (FileStream textReader = File.OpenRead(gameDirectory))
                {
                    games = (List<GameRecord>)xmlReader.Deserialize(textReader);
                    textReader.Dispose();
                    textReader.Close();
                }
            }
            else
            {
                Console.WriteLine("\nGame Records file does not Exist\nUnable to retrieve List - Testing\n");
                Console.WriteLine("\nReturning Empty List of Game Records - Testing\n");
            }
            return games;
        };// Returns List of Stored Games
        public static readFile<Language> getLanguagesBackup = () =>
        {
            XmlSerializer xmlReader = new XmlSerializer(typeof(List<Language>));
            List<Language> languages = new List<Language>();
            if(File.Exists(backupLanguagesDirectory))
            {
                using (FileStream textReader = File.OpenRead(backupLanguagesDirectory))
                {
                    languages =  (List<Language>)xmlReader.Deserialize(textReader);
                    textReader.Dispose();
                    textReader.Close();
                }
            }
            else
            {
                Console.WriteLine("\nBackup Languages File does not exist\nUnable to retrieve List - Testing\n");
                Console.WriteLine("\nReturning Empty List of Backed up Languages - Testing\n");
            }
            return languages;
        }; // Reds a list of languages stored as backup

        // Write Methods - Write List of instances to XML files
        public static writeFile<User> writeUsers = users =>
        {
            XmlSerializer xmlWriter = new XmlSerializer(typeof(List<User>));
            using (Stream textWriter = new FileStream(usersDirectory, FileMode.Create, FileAccess.ReadWrite))
            {
                xmlWriter.Serialize(textWriter, users);
                textWriter.Dispose();
                textWriter.Close();
                Console.WriteLine("\nUsers' File updated - Testing\n");
            }
        };// Writes List of Stored Players
        public static writeFile<Language> writeLanguages = languages =>
        {
            XmlSerializer xmlWriter = new XmlSerializer(typeof(List<Language>));
            using (Stream textWriter = new FileStream(languagesDirectory, FileMode.Create, FileAccess.ReadWrite))
            {
                xmlWriter.Serialize(textWriter, languages);
                textWriter.Dispose();
                textWriter.Close();
                Console.WriteLine("\nLanguages File updated - Testing\n");
            }
        }; // Writes List of Stored Languages
        public static writeFile<GameRecord> writeGames = games =>
        {
            XmlSerializer xmlWriter = new XmlSerializer(typeof(List<GameRecord>));
            using (Stream textWriter = new FileStream(gameDirectory, FileMode.Create, FileAccess.ReadWrite))
            {
                xmlWriter.Serialize(textWriter, games);
                textWriter.Dispose();
                textWriter.Close();
                Console.WriteLine("\nGames File updated - Testing\n");
            }
        };// Writes List of Stored Games
        public static writeFile<Language> writeBackupLanguages = languages =>
        {
            XmlSerializer xmlWriter = new XmlSerializer(typeof(List<Language>));
            using(Stream textWriter = new FileStream(backupLanguagesDirectory,FileMode.Create, FileAccess.ReadWrite))
            {
                xmlWriter.Serialize(textWriter, languages);
                textWriter.Dispose();
                textWriter.Close();
                Console.WriteLine("\nBackup Languages File updated - Testing\n");
            }
        }; // Backs up List of Languages

        // Append Methods - Append class instance to XML files
        public static appendFile<User> appendUser = user =>
        {
            Welcome_Form.current_Users.Add(user); // Add User instance to current Users
            writeUsers(Welcome_Form.current_Users);
        };// Appends player to stored Players
        public static appendFile<GameRecord> appendGame = game =>
        {
            Welcome_Form.current_Games.Add(game); // Add Game Record instance to current Game Records
            writeGames(Welcome_Form.current_Games); // Write updated List of Game Records to Game Records File
        };// Appends Game to stored games
        
        // Writes initial Languages to Languages File
        public static void writeInitialLanguages() // Writes the initial/default List of Languages to the Languages File
        {
            List<Language> languages = new List<Language>();
            Language english = new Language("English");
            english.words.Add("Hi");
            english.words.Add("How are you?");
            english.words.Add("Good");
            english.words.Add("Bad");
            english.words.Add("Bye");

            Language italian = new Language("Italian");
            italian.words.Add("Ciao");
            italian.words.Add("Come stai?");
            italian.words.Add("Bene");
            italian.words.Add("Male");
            italian.words.Add("Arrivederci");
            languages.Add(english);
            languages.Add(italian);


            writeLanguages(languages);
            Console.WriteLine("\nLanguages File re-written with Default Languages - Testing\n");
        }

        // Change PassChar depending upon checkbox
        public static void passCheckBox(CheckBox chckBox, TextBox tf_Password)
        {
            if (chckBox.Checked) // If the Checkbox is checked
            {
                tf_Password.PasswordChar = (char)0; // Setting the text clear and readable
            }
            else
            {
                tf_Password.PasswordChar = '*'; // Obfuscating the text and unreadable
            }
        }

        // Method that displays an increasing number effect within a label - Make the value transition, creating a loading effect
        public static void transitionInteger(int target, Label label)
        {
            // Setting the initial numerical Value to 1
            int value = 1;
            label.Text = value.ToString();

            // Creating a Timer, which updates the numerical value inside the Label each time it reaches its' Interval
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

            try
            {
                // Updating the Timer's Interval. The closer to the target, the longer the interval
                timer.Interval = Convert.ToInt32((value * 70) / target) + 1;

                timer.Tick += (sender, args) =>
                {
                    if (value != target) // If the current value is not equal to the target
                    {
                        value++; // The current value is incremented
                        label.Text = value.ToString(); // The text inside the Label is updated
                        timer.Interval = Convert.ToInt32((value * 70) / target) + 1; // The Timer's interval is updated
                    }
                    else
                    {
                        // If the numerical value inside the Label is equal to the target, The Timer is Stopped
                        timer.Stop();
                    }
                };
            }
            catch (DivideByZeroException) // If the numerical target is equal to Zero
            {
                label.Text = "0"; // Set the text inside the Label Directly to 0
                timer.Stop(); // Stop the Timer
            }
            timer.Start(); // Start the Timer
        }
    }
}
