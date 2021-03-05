using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace _178390_AndreaBriffa_DDOOCP
{
    public partial class Log_In : Form
    {
        public delegate void addUser(User user);
        public delegate void addGuest(Guest guest);
        public delegate void breakLogin(bool decision);

        private bool valid = false; // Depicts whether a player has registered to play or not

        // Constructor which initializes the Form for a User to Log into his account
        public Log_In()
        {
            InitializeComponent();
            Icon = new Icon(Library.iconDirectory); // Set the Icon of the Form

            btn_Play_As_Guest.Visible = false; /* The User is logging into his account,
            thus there is no need for a "Play as Guest" button */
            btn_Play_As_Guest.Enabled = false; // Disable the "Play as Guest" button
            // Removing the "Exit Game" button
            btn_Exit.Visible = false;
            btn_Exit.Enabled = false;

            // Subscribing to event handler when the Checkbox's 'checked' state changes
            chck_Box_Show_Password.CheckedChanged += (sender, args) =>
            {
                Library.passCheckBox(sender as CheckBox, tf_Password);
            };

            // Subscribing to event - When the register as a user button is selected
            link_Register.Click += (s, a) =>
            {
                // Load Register Form Temporarily
                Visible = false;
                Register_Form regForm = new Register_Form();
                regForm.ShowDialog();
                Visible = true;
            };

            // Subscribing to the Log in button clicked action
            btn_LogIn.Click += (sender, args) =>
            {
                // Retrieve stored users
                List<User> users = Welcome_Form.current_Users;
                bool logged = false;
                
                // Iterating through the users, comparing the inputted username and password
                foreach (User user in users)
                {
                    if (user.userName.Equals(tf_Username.Text.Trim()) && user.password.Equals(tf_Password.Text.Trim()))
                    {
                        Visible = false;
                        User_Account_Form userAccount = new User_Account_Form(user); /* Create instance of User_Account Form,
                        Passing the logged in user as Anchor argument */
                        userAccount.ShowDialog(); // Show the User Account Form
                        logged = true; // User has logged in
                        Close();
                    }
                }
                if (users.Count == 0) // If there are no users
                {
                    MessageBox.Show("There are no active users in the system", "Notification", 0, MessageBoxIcon.Information);
                }
                if (!logged && users.Count!=0) // If no user has logged and there are stored users
                {
                    MessageBox.Show("Username or Password do not Match!", "Error", 0, MessageBoxIcon.Error);
                }
            };
        }

        /* Constructor which initializes the Form for a Player to log in as a User or play as a Guest
         * Recieves the Player number to register, and the callback functions */
        public Log_In(string playerNo, addUser appendUser, addGuest appendGuest, breakLogin breakGame)
        {
            InitializeComponent();

            Icon = new Icon(Library.iconDirectory); // Set the Icon of the Form

            lbl_Sign_In.Text = playerNo + " - " + lbl_Sign_In.Text; // Prompt the Player to sign in

            // Subscribe to event - When the User selects to play as a Guest by pressing the "Play as Guest" button
            btn_Play_As_Guest.Click += (sender, args) =>
            {
                DialogResult result = MessageBox.Show("Are you sure you want to play as Guest?", "Information",
                    MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    appendGuest(new Guest(playerNo));  // Add a new Guest through callback function
                    valid = true;  // A player has registered
                    Close();  // Close the form
                }
            };

            // If the user attempts to exit the Game setup
            btn_Exit.Click += (sender, args) =>
            {
                DialogResult result = MessageBox.Show("Are you sure you want to Exit the Game?", "Notification",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(result == DialogResult.Yes)
                {
                    breakGame(true);  // Break the Game Setup Process
                    valid = true; // No Player has registered, but this avoid a user prompt upon Form Closing
                    Close();
                }
            };

            // Subscribing to the event - When the user Presses the login button
            btn_LogIn.Click += (sender, args) =>
            {
                // Load current users
                List<User> users = Welcome_Form.current_Users;
                bool logged = false;

                // Iterating through the users, comparing the inputted username and password
                foreach (User user in users)
                {
                    // If the user's details match the inputs
                    if(user.userName.Equals(tf_Username.Text.Trim()) && user.password.Equals(tf_Password.Text.Trim()))
                    {
                        if (!user.playing) // If the user has not registered yet
                        {
                            appendUser(user);
                            valid = true;  // A Player has logged in
                            user.playing = true;
                            Welcome_Form.current_Users = users; // Updates Users
                            Close();
                            logged = true; // The user logged In
                            break;
                        }
                        else
                        {
                            MessageBox.Show("User already Logged In!", "Error", 0, MessageBoxIcon.Error);
                            logged = true; // The user is already logged in
                        }
                    }
                }
                if (users.Count == 0)
                {
                    MessageBox.Show("There are no active users in the system","Notification",0,MessageBoxIcon.Information);
                }
                if(!logged && users.Count != 0)
                {
                    MessageBox.Show("Username or Password do not Match!", "Error", 0, MessageBoxIcon.Error);
                    // If no users managed to log In
                }
            };
            // Executing this event handler once the Checkbox's 'Checked' state changes
            chck_Box_Show_Password.CheckedChanged += (sender, args) =>
            {
                Library.passCheckBox(sender as CheckBox, tf_Password);
            };
            // Subscribing to the Form Closing event
            FormClosing += (sender, args) =>
            {
                if (!valid)  // If the Player has not logged in or registered as Guest
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to exit the Game?","Notification",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        breakGame(true);  // Break the Game Setup Process
                    }
                    else
                    {
                        args.Cancel = true;  // Cancel the Form Closing Process
                    }
                }
            };
            // If the Register Link is clicked
            link_Register.Click += (s, a) =>
            {
                Visible = false;
                Register_Form regForm = new Register_Form();
                regForm.ShowDialog();
                Visible = true;
            };
        }
    }
}
