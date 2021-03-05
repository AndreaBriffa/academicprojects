using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace _178390_AndreaBriffa_DDOOCP
{
    public partial class Modify_Word_Sets : Form
    {
        private bool changesMade = false; // If unsaved changes were made to the Word Sets

        private int initalLanguageNumber; // Stores the current amount of Languages
        private int initialRowNumber; // Initial Number of Word Pairs
        
        public Modify_Word_Sets()
        {
            InitializeComponent();

            Icon = new Icon(Library.iconDirectory); // Set the Icon of the form

            // If the Backup Languages File exists
            if (File.Exists(Library.backupLanguagesDirectory))
            {
                DateTime lastUpdateBackup = File.GetLastWriteTime(Library.backupLanguagesDirectory); // Get last update date
                lbl_Last_Backup_Date.Text = lastUpdateBackup.ToString(); // Display the last update date
            } // If the Languages backup file exists
            else
            {
                lbl_Last_Backup_Date.Text = "No backup yet!";
            } // If the Languages backup file does not exist


            dtGirdViewWordSets.DataSource = getLanguagesDataTable();  // Sets the DataGridView's DataSource

            initalLanguageNumber = dtGirdViewWordSets.Columns.Count; // Set the number of current languages

            initialRowNumber = Welcome_Form.current_Languages[0].words.Count;

            dtGirdViewWordSets.CellClick += (sender, args) =>  // Occurs When a cell is clicked
            {
                changesMade = true;
            };

            // Update "changesmade" to true if the user added a languageor a row
            dtGirdViewWordSets.ColumnAdded += (sender, args) =>  
            {
                if (dtGirdViewWordSets.Columns.Count > initalLanguageNumber)
                {
                    changesMade = true;
                }
            };

            dtGirdViewWordSets.RowsAdded += (s, a) =>
            {
                // The row count is subtracted by 1 as to account for the empty row at the bottom
                if(dtGirdViewWordSets.Rows.Count-1 > initialRowNumber)
                {
                    changesMade = true;
                }
            };

            btn_Add_Language.Click += (sender, args) => // Add a Language to the existing Languages
            {
                string lang_Name = tf_Language_Name.Text;
                // If the New Language's name is valid (Unique and not empty)
                if (checkLanguageName(lang_Name))
                {
                    // Adding the new Language to the DataGridView
                    DataTable langDT = getLanguagesDataTable();
                    langDT.Columns.Add(tf_Language_Name.Text);
                    dtGirdViewWordSets.DataSource = langDT;
                }
            };

            btn_Save_Language.Click += (sender, args) => // Save the Current Languages - Update the Languages File
            {
                // If the Word Sets do not contain empty cells
                if (checkValidLanguages())
                {
                    saveLanguages();
                }
            };

            btn_Delete_Row.Click += (sender, args) =>
            {
                if (dtGirdViewWordSets.Rows.Count-1 > 2) // Leaving out the empty row at the bottom
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete row " + 
                        (dtGirdViewWordSets.SelectedCells[0].RowIndex + 1).ToString(), "Notification",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        Console.WriteLine($"\nUser verified deletion of Row {dtGirdViewWordSets.SelectedCells[0].RowIndex + 1} - Testing\n");
                        try
                        {
                            // Attempting to Remove Row
                            dtGirdViewWordSets.Rows.RemoveAt(dtGirdViewWordSets.SelectedCells[0].RowIndex);
                            // If the Languages are valid
                            if (checkValidLanguages())
                            {
                                saveLanguages();
                                // Updating the initial Row Number
                                initialRowNumber = getCurrentLanguages()[0].words.Count;
                            }
                        }
                        catch (Exception) { } // Attempted to delete empty row
                    }
                }
                else
                {
                    MessageBox.Show("Minimum amount of Word Pairs reached", "Alert", 0, MessageBoxIcon.Warning);
                }
            }; // Delete a row, consisting of an entry of each Language

            btn_Delete.Click += (sender, args) =>
            {
                deleteLanguage();
            }; // Delete a Language

            btn_Backup.Click += (s, a) =>
            {
                if (checkValidLanguages())
                {
                    if (Welcome_Form.backup_Languages.Count != 0)
                    { // If there are existing Languages set as backup
                        DialogResult result = MessageBox.Show("Are you sure you want to override last backup?", "Warning",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes) // If the User wants to override the Backup
                        {
                            // Update the backup files and the latest backup Date label
                            Library.writeBackupLanguages(getCurrentLanguages());
                            lbl_Last_Backup_Date.Text = File.GetLastWriteTime(Library.backupLanguagesDirectory).ToString();
                            Welcome_Form.backup_Languages = Library.getLanguagesBackup();
                        }
                    }
                    else // If there are no current languages backed up
                    {
                        // Write the backup files and set the latest backup Date label
                        Library.writeBackupLanguages(getCurrentLanguages());
                        Welcome_Form.backup_Languages = Library.getLanguagesBackup();
                        lbl_Last_Backup_Date.Text = File.GetLastWriteTime(Library.backupLanguagesDirectory).ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Languages not valid for Backup", "Error", 0, MessageBoxIcon.Error);
                }
            }; // Backup the current Languages to a Backup File

            btn_Restore.Click += (s, a) =>
            {
                if (Welcome_Form.backup_Languages.Count != 0) // If there are Languages saved as Backup
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to ovverride the current Langauges with the last Backup?",
                        "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes) // If the user wants to override the current languages with the stored backup
                    {
                        // Update the DataGridView
                        dtGirdViewWordSets.DataSource = getBackupLanguagesDataTable();
                        Library.writeLanguages(getCurrentLanguages()); // Update the Languages File
                        // Update the current Languages
                        Welcome_Form.current_Languages = Library.getLanguages();
                    }
                }
                else // If there are no Languages backed up
                {
                    DialogResult result = MessageBox.Show($"There are no Backup Languages.{Environment.NewLine}" +
                        $"Would you like to Backup the current Langauges?", "Notification",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes) // If the User wants to back up the Current Languages
                    {
                        if (checkValidLanguages())
                        {
                            // Writing current Languages to Languages Backup File
                            Library.writeBackupLanguages(getCurrentLanguages());
                            lbl_Last_Backup_Date.Text = File.GetLastWriteTime(Library.backupLanguagesDirectory).ToString();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Word Sets - Cells Empty !", "Error", 0, MessageBoxIcon.Error);
                        }
                    }
                }
            }; // Replace the current languages with those stored as backup

            btn_Restore_Default_Languages.Click += (s, a) =>
            {
                DialogResult result = MessageBox.Show("Are you sure you want to restore default languages (English + Italian)?",
                    "Warning",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    // Overwriting Languages File
                    Library.writeInitialLanguages(); // Writes Default Languages to Languages file
                    Welcome_Form.current_Languages = Library.getLanguages(); // Updating current List of Languages
                    dtGirdViewWordSets.DataSource = getLanguagesDataTable(); // Update the DataGridView
                    changesMade = false; // Changes made are saved
                }
            }; // Replace the current languages with those that serve as Backup

            FormClosing += (sender, args) =>
            {
                if (changesMade) // If there are unsaved changes remaining
                {
                    DialogResult result = MessageBox.Show("Do you want to Save the Changes Made?",
                        "Notification", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if(result == DialogResult.Yes) // If the User wants to save the changes
                    {
                        if (checkValidLanguages()) // If the inputted languages and word sets are valid
                        {
                            saveLanguages();
                        }
                        else // If the inputted languages and word sets are invalid
                        {
                            args.Cancel = true;
                        }
                    }
                    // If the User selected to Cancel the Form Closing event
                    else if(result == DialogResult.Cancel){
                        args.Cancel = true;
                    }
                }
            }; // Event Handler upon Form Closing event
        }

        private DataTable getLanguagesDataTable()
        {
            DataTable langDT = new DataTable();
            List<Language> languages = Welcome_Form.current_Languages;

            // Add the Languages' Names as Column Headers
            foreach(Language lang in languages)
            {
                langDT.Columns.Add(lang.name);
            }

            // Add the Word Translations / rows
            for(int i=0; i < languages[0].words.Count; i++)
            {
                DataRow row = langDT.NewRow();
                foreach(Language lang in languages)
                {
                    row[lang.name] = lang.words[i];
                }
                langDT.Rows.Add(row);
            }

            return langDT;
        }  // Returns a Datatable object to fill the DataGrid View with current word sets
        
        private DataTable getBackupLanguagesDataTable()
        {
            DataTable langDT = new DataTable();
            List<Language> languages = Welcome_Form.backup_Languages;

            // Add the Languages' Names as Column Headers
            foreach (Language lang in languages)
            {
                langDT.Columns.Add(lang.name);
            }

            // Add the Word Translations / rows
            for (int i = 0; i < languages[0].words.Count; i++)
            {
                DataRow row = langDT.NewRow();
                foreach (Language lang in languages)
                {
                    row[lang.name] = lang.words[i];
                }
                langDT.Rows.Add(row);
            }

            return langDT;
        } // Returns a Datatable object to fill the DataGrid View with backup up Languages / word sets

        private void saveLanguages()
        {
            // Update the Languages File and the current List of Languages
            Library.writeLanguages(getCurrentLanguages());
            Welcome_Form.current_Languages = Library.getLanguages();
            changesMade = false; // The changes Made are now Saved
            // The initial number of Words per Language and Number of Languages are updated
            initalLanguageNumber = getCurrentLanguages().Count;
            initialRowNumber = getCurrentLanguages()[0].words.Count;
        } // Save the Current Languages to the Languages File

        private bool checkValidLanguages()
        {
            DataTable newDT = (DataTable)dtGirdViewWordSets.DataSource;
            // For each Column
            foreach (DataColumn dc in newDT.Columns)
            {
                // For each Row
                foreach (DataRow dr in newDT.Rows)
                {
                    if (dr[dc.ToString()].ToString().Trim().Equals(""))  // Checks whether the cell contains an empty string
                    {
                        MessageBox.Show("Invalid Word Sets - Fill in all Cells!", "Alert", 0, MessageBoxIcon.Asterisk);
                        return false;
                    }
                }
            }

            if (dtGirdViewWordSets.Rows.Count > 100)
            {
                MessageBox.Show($"Maximum amount of Rows(100)! Please remove excess Row!" +
                    $"{Environment.NewLine}Modify existing rows.", "Alert", 0, MessageBoxIcon.Asterisk);
                return false; 
            }

            return true;
        }  // Checks if the Language's words are valid
        
        public bool checkLanguageName(string name)
        {
            // Checking if Language name is empty
            if (name.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Language Name", "Alert", 0, MessageBoxIcon.Information);
                return false;
            }
            // Checking if the Language name already exists
            else
            {
                // Adding existing Languages' names to Hashset
                HashSet<string> existingLanguages = new HashSet<string>();
                foreach (Language lang in Welcome_Form.current_Languages)
                {
                    existingLanguages.Add(lang.name.ToUpper());
                }

                // Checking whether the Language name is present in the Hashset
                if (existingLanguages.Contains(name.ToUpper().Trim()))
                {
                    MessageBox.Show("Language Name already exists!", "Alert", 0, MessageBoxIcon.Information);
                    return false;
                }
            }
            // Language name is valid
            return true;
        }  // Checks if the Language's name is empty or already exists

        public List<Language> getCurrentLanguages()
        {
            DataTable newDT = (DataTable)dtGirdViewWordSets.DataSource;
            List<Language> languages = new List<Language>(); // Stores List of Languages
            int counter = -1;

            // For each Column / Language
            foreach (DataColumn dc in newDT.Columns)
            {
                // Add The new Language together with its name
                languages.Add(new Language(dc.ToString().Trim()));
                counter++;
                // For each Word Pair inside the DataGridView
                foreach (DataRow dr in newDT.Rows)
                {
                    languages[counter].words.Add(dr[dc.ToString()].ToString().Trim()); // Adds the value of the specified column's cells
                }
            }

            return languages;
        } // Get a List of Languages that are currently present in the DataGridView

        private void deleteLanguage()
        {
            if (dtGirdViewWordSets.Columns.Count > 2) // If there are more than two languages
            {
                DialogResult result = MessageBox.Show($"Are you sure you want to delete the Language - " +
                    $"{dtGirdViewWordSets.Columns[dtGirdViewWordSets.CurrentCell.ColumnIndex].Name}",
                    "Notification", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    // Get DataSource of Current DataGridView
                    DataTable newDT = (DataTable)dtGirdViewWordSets.DataSource;
                    // Remove the selected Language (column) by the user
                    newDT.Columns.RemoveAt(dtGirdViewWordSets.CurrentCell.ColumnIndex);
                    // Update the Languages file with the current Langauges
                    Library.writeLanguages(getCurrentLanguages());
                    // Update the Current Languages
                    Welcome_Form.current_Languages = Library.getLanguages();
                }
            }
            else // If there are only two existing languages
            {
                MessageBox.Show($"Please add another Language to Delete one." +
                    $"{Environment.NewLine}Minimum number of Languages is two (2)!", 
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }  // Deletes the selected Language
    }
}