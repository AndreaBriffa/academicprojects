using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _178390_AndreaBriffa_DDOOCP
{
    public partial class Game_Settings_Form : Form
    {
        // Used for a callback method to initialize game properties
        public delegate void setGameSettings(int numOfPairs, Language lang1, Language lang2);
        // Used for a callback method in case the game setup process was terminated
        public delegate void breakLogin(bool decision); 

        private bool valid = false; // Determines whether a user prompt shows when the form is closing

        // Recieves the callBack Methods
        public Game_Settings_Form(setGameSettings appendSettings, breakLogin breakGame)
        {
            InitializeComponent();

            Icon = new Icon(Library.iconDirectory); // Set the Form Icon

            List<Language> languages = Welcome_Form.current_Languages; // List of Stored Languages

            lbl_Max.Text = "<= " + languages[0].words.Count(); // Set Label Text
            lbl_Alert.Visible = false;  // Set the alert label to invisible

            // Add each language's name to each combobox, which allow the user to choose two languages
            foreach(Language lang in languages)
            {
                cmb_Lang1.Items.Add(lang.name);
                cmb_Lang2.Items.Add(lang.name);
            }
            // Setting the first and second languages as default languages
            cmb_Lang1.SelectedIndex = 0;
            cmb_Lang2.SelectedIndex = 1;

            // Subscribing to the event when the text inside the "number of Pairs" text field changes
            tf_Input_Pairs.TextChanged += (s, a) =>
            {
                try
                {
                    int num = Convert.ToInt32(tf_Input_Pairs.Text.Trim()); // Throws exception if value is not numeric
                    if (num < 2 || num > languages[0].words.Count) // If the number of Pairs is not within valid range
                    {
                        lbl_Alert.Text = "Invalid Number";
                        lbl_Alert.Visible = true;
                    }
                    else // If the number is within valid range
                    {
                        lbl_Alert.Visible = false;
                    }
                }
                catch (Exception) // Handles the exception
                {
                    lbl_Alert.Text = "Integer Numerical Values Only";
                    lbl_Alert.Visible = true;
                }
            };

            // Subscribing to the event when the "Submit Settings" button is clicked
            btn_Submit_Settings.Click += (sender, args) =>
            {
                if (cmb_Lang1.SelectedIndex == cmb_Lang2.SelectedIndex) // The languages cannot be the same
                {
                    Console.WriteLine("\nInvalid Languages (Same Languages) - Testing\n");
                    MessageBox.Show("Cannot choose the same Languages!", "Error", 0, MessageBoxIcon.Error);
                }
                else if(!lbl_Alert.Visible) // If the inputs are valid and the languages are different
                {
                    DialogResult result = MessageBox.Show("Are you sure with the entered settings?", "Information",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes) // User verification is required
                    {
                        valid = true;  // The user verified the inputs
                        // The inputs are sent back to the Welcome Form through the callback function
                        appendSettings(Convert.ToInt32(tf_Input_Pairs.Text.Trim()), languages[cmb_Lang1.SelectedIndex], languages[cmb_Lang2.SelectedIndex]);
                        Close();
                    }
                    else
                    {
                        Console.WriteLine("\nPlayer did not verify Game Settings\nSettings not Submitted - Testing\n");
                    }
                }
                else if(lbl_Alert.Visible) // If the alert label was shown to the user, meaning there is an error
                {
                    Console.WriteLine("\nInvalid number of Pairs - Testing\n");
                    MessageBox.Show("Please input Valid Number of Pairs!", "Error", 0, MessageBoxIcon.Information);
                }
            };

            // Subscribing to the Form Closing event
            FormClosing += (sender, args) =>
            {
                if (!valid) // If the user did not input and validate the game settings
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to quit the Game?", "Notification",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.No)
                    {
                        args.Cancel = true;
                    }
                    else // If the user wants to stop the game setup
                    {
                        breakGame(true); // Callback function, which will skip the game setup and the game within the Welcome Form
                    }
                }
                else
                {
                    Console.WriteLine("\nGame Settings Submitted by Players\nForm Closing - Testing\n");
                }
            };
        }
    }
}
