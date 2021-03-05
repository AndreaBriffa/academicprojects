using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace _178390_AndreaBriffa_DDOOCP
{
    public partial class Game : Form
    {
        private bool finished = false; /*Determines whether the game is finished. If so the user may close the game form
        without a prompt showing up*/
        private bool began = false; /* Determines whether the game has begun or not. Once set to true,
        the timer and stopwatches are triggered. The game begins when a player pressess a button */

        private int pairsLeft = 0; // The number of pairs left. No pairs left mean the game has finished
        private int numOfPairs = 0; // Stores the number of word pairs the users chose to play with

        private string winner = ""; // The username of the winner

        private List<Button_Custom> selectedButtons = new List<Button_Custom>(); /* Stores the recent buttons that have been
        pressed by a player */
        private List<Tuple<Button_Custom, Button_Custom>> buttonPairs = new List<Tuple<Button_Custom, Button_Custom>>();
        /* Stores a list of Button pairs, defined using a Tuple structure. When there are two selected buttons,
         * it is checked whether they are from the same tuple, determining a match or not */

        private System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer(); // The game timer

        private Stopwatch gameWatch = new Stopwatch(); // The game stopwatch
        private Stopwatch p1_Watch = new Stopwatch(); /* Player 1's stopwatch,
        keeping track of the time Player 1 spent his/her playing his turns */
        private Stopwatch p2_Watch = new Stopwatch(); /* Player 2's stopwatch,
        keeping track of the time Player 2 spent his/her playing his turns */

        private Label gameTime = new Label(); // Label stating "Game Time Elapsed"
        private Label timeElapsed = new Label(); // Label displaying the numerical value of the Game's elapsed time
        private Label p1_userName = new Label(); // Display player 1's username
        private Label p2_userName = new Label(); // Display player 2's username
        private Label p1_Score = new Label(); // Label Stating "Score"
        private Label p2_Score = new Label(); // Label Stating "Score"
        private Label p1_Score_Count = new Label(); // Displays the numerical value of Player 1's score
        private Label p2_Score_Count = new Label(); // Displays the numerical value of Player 2's score
        private Label p1_Time = new Label(); // Label stating "Elapsed Time"
        private Label p2_Time = new Label(); // Label stating "Elapsed Time"
        private Label p1_Elapsed_Time = new Label(); // Displays the numerical value of Player 1's elapsed Time
        private Label p2_Elapsed_Time = new Label(); // Displays the numerical value of Player 2's elapsed Time

        private Language language_1; // A reference to the first language the users chose to include in the word sets
        private Language language_2; // A reference to the first language the users chose to include in the word sets

        private Player currentPlayer; // A reference to the Player currently guessing a pair of words
        private Player player1; // Reference to Player 1
        private Player player2; // Reference to Player 2

        /* A reason why duplicate labels were used to state the Score, time elapsed etc is to only set the definition of the units
         * i.e. "Time Elapsed" and "Score" once, and then updating the secondary label with the actual units i.e. numerical values.
         * This reduced load on the UI threads over time as only numerical values are updated i.e. "23", "24" , "25" not both 
         * defenition and units i.e. "Game Time Elapsed : 23" , "Game Time Elapsed : 24" , "Game Time Elapsed : 25" etc. */

        public Game(List<Guest> guests, List<User> users, Language language_1, Language language_2, int numOfPairs)
        {
            InitializeComponent();
            Console.WriteLine("\nInstance of Game created - Testing");

            Icon = new Icon(Library.iconDirectory); // Set the Icon of the Form

            int rowAmount = 0;  // Amount of Rows
            int rowPartition = 0; // The height of a row
            int columnPartition = 0; // The width of a column
            int perRow = 0; // The amount of Buttons per row
            int wordCounter = 0; // Used to iterate through word sets (set1, set2) when assigning word sets to buttons
            int btnsLeft = numOfPairs * 2; // Indicates how many buttons are left to add to the UI and assign event handlers to
            this.numOfPairs = numOfPairs; // Initializing the number of word pairs
            pairsLeft = numOfPairs; // How many pairs are left to guess


            this.language_1 = language_1; // Initializing the first language chosen by the user
            this.language_2 = language_2; // Initializing the second language chosen by the user
            
            List<Button_Custom> buttons = new List<Button_Custom>(); // Stores a list of Custom buttons

            Random random = new Random(); // Randomizes index for randomizing association of word sets with buttons

            List<string> set1 = language_1.words; // Temporarily stores the words from language 1
            List<string> set2 = language_2.words; // Temporarily stores the words from language 2
            List<string> pair1 = new List<string>(); // Used for storing randomized words from language 1
            List<string> pair2 = new List<string>(); // Used for storing randomized words from language 2

            // Populating button list with buttons
            for (int i=0; i<numOfPairs*2; i++)
            {
                buttons.Add(new Button_Custom());
            } 
            List<Button_Custom> tempButtons = buttons; // Initializing the temporary buttons list
            Console.WriteLine("\nList of button references initialized - Testing\n");

            gameTimer.Interval = 10; // Setting timer interval
            // Setting out the interval function
            gameTimer.Tick += (sender, args) =>
            {
                gameTime.Text = (gameWatch.ElapsedMilliseconds / 1000).ToString();
                p1_Elapsed_Time.Text = (p1_Watch.ElapsedMilliseconds / 1000).ToString();
                p2_Elapsed_Time.Text = (p2_Watch.ElapsedMilliseconds / 1000).ToString();
                // Console.WriteLine("\nTime Elapsed Labels Updated - Testing\n");
            };
            // Setting out the Form Closing event
            FormClosing += (sender, args) =>
            {
                if (!finished)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to quit the Game?", "Notification", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    if (result == DialogResult.No)
                    {
                        args.Cancel = true;
                    }
                }
                else
                {
                    Console.WriteLine("\nGame Finished - Testing\n");
                }
            };

            // Set the reference to the registered players. User properties are not inherited - Only those from Base Class
            // By default guests will start playing before registered users since they are guests
            switch (guests.Count)  // Depending upon the Number of guests
            {
                case 0:
                    Console.WriteLine("\nBoth Players are Users - Testing\n");
                    player1 = users[0];
                    player2 = users[1];
                    break;
                case 1:
                    Console.WriteLine("\nOne player is a User whilst other is Guest - Testing\n");
                    player1 = guests[0];
                    player2 = users[0];
                    break;
                case 2:
                    Console.WriteLine("\nBoth Players are Guest - Testing\n");
                    player1 = guests[0];
                    player2 = guests[1];
                    break;
            }
            // By default Player 1 will start playing first
            currentPlayer = player1;

            // Set the UI Properties depending upon the Number of Pairs
            // The number of buttons per row
            // The height of a row
            // The amount of rows
            if (numOfPairs <= 22)
            {
                perRow = 6;
                rowPartition  = Screen.FromControl(this).Bounds.Height / 12; // The height of a row
                rowAmount = (int)Math.Ceiling(Convert.ToDouble(numOfPairs * 2) / perRow);
            }
            else if(numOfPairs <= 44)
            {
                perRow = 8;
                rowPartition =  Screen.FromControl(this).Bounds.Height / 14; // The height of a row
                rowAmount = (int)Math.Ceiling(Convert.ToDouble(numOfPairs * 2) / perRow);
            }
            else if(numOfPairs<=66)
            {
                perRow = 10;
                rowPartition = Screen.FromControl(this).Bounds.Height / 14; // The height of a row
                rowAmount = (int)Math.Ceiling(Convert.ToDouble(numOfPairs * 2) / perRow);
            }
            else
            {
                perRow = 12;
                rowPartition = Screen.FromControl(this).Bounds.Height / 14; // The height of a row
                rowAmount = (int)Math.Ceiling(Convert.ToDouble(numOfPairs * 2) / perRow);
            }
            columnPartition = Width / perRow; // Setting the width of the columns

            /* Setting width and height of Form
             * Setting AutoSize to true to fit each UI element inside the Form
             * Fixing the Form's border, not allowing the Form to resize */
            Height = 0;
            Width = 0;
            AutoSize = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            // Adding buttons to Form
            for (int y=1; y<rowAmount+1; y++) // Iterates through each row
            {
                for (int x = 0; x<perRow; x++) // Loops the number of buttons per row
                {
                    if(btnsLeft != 0) /* If there are reamining buttons to display. This avoids the chance of the last row 
                        consisting of three buttons where other rows consist of 6, thus for the last three buttons there will
                        be none to display, throwing an Exception */
                    {
                        int top = rowPartition * y; // Setting the top offset of the Button, depending upon the row number
                        int left = columnPartition * x; // Setting the left offset of the Button, depending upon the column number
                        int height = rowPartition-10; // Setting the height of the button
                        int width = height; // Setting the button's shape as a square

                        Button_Custom button = buttons[(numOfPairs * 2) - btnsLeft]; /* Creating a reference to a button from
                        the buttons list sequentially */

                        button.Location = new Point(left, top); // Setting the Location of the button
                        button.Name = "btn_" + ((numOfPairs*2) - btnsLeft); // Setting the button's name
                        button.Text = "?"; // Setting the initial text displayed in front of the button
                        button.Size = new Size(width, height); // Setting the size of the button
                        button.Padding = new Padding(0); // Removing any of the button's padding
                        button.ForeColor = Color.RoyalBlue; // Setting the button's text color
                        button.BackColor = Color.White; // Setting the button's background color to White
                        // Subscribing an event handler to the button's Click event
                        button.Click += (sender, args) =>
                        {
                            if (!button.activated) // If the button is not aleady activated
                            {
                                button.Text = button.word; // Display the word associated with the button
                                button.Refresh(); // Allow time for button to change its' text property, as an UI function is heavy
                                buttonClicked(sender as Button_Custom); // Send the button as an argument to the method
                            }
                            else
                            {
                                Console.WriteLine("\nButton already activated - Testing\n");
                            }
                        };

                        Controls.Add(button); // Add the button to the Form's Controls
                        btnsLeft--; // Decrementing the amount of button's left
                    }
                    else // If there are no more buttons to add to the form, break the loop
                    {
                        break;
                    }
                }
            }

            // Setting the Label Properties
            p1_userName.Name = "p1_userName";
            p1_userName.Text = player1.userName;
            p1_userName.Location = new Point((int)(columnPartition * 0.1), (int)(rowPartition * 0.1));
            p1_userName.Size = new Size(columnPartition, rowPartition / 4);
            p1_userName.ForeColor = Color.White;
            p1_userName.BackColor = Color.Transparent;

            p2_userName.Name = "p2_userName";
            p2_userName.Text = player2.userName;
            p2_userName.Location = new Point(columnPartition*4, (int)(rowPartition * 0.1));
            p2_userName.Size = new Size(columnPartition, rowPartition / 4);
            p2_userName.ForeColor = Color.White;
            p2_userName.BackColor = Color.Transparent;

            p1_Score.Name = "lbl_P1_Score";
            p1_Score.Text = "Score : ";
            p1_Score.Location = new Point((int)(columnPartition * 0.1), (int)((rowPartition * 0.1) + (rowPartition / 4)));
            p1_Score.Size = new Size(columnPartition, rowPartition / 4);
            p1_Score.ForeColor = Color.White;
            p1_Score.BackColor = Color.Transparent;

            p2_Score.Name = "lbl_P2_Score";
            p2_Score.Text = "Score : ";
            p2_Score.Location = new Point(columnPartition * 4, (int)((rowPartition * 0.1)+(rowPartition / 4)));
            p2_Score.Size = new Size(columnPartition, rowPartition / 4);
            p2_Score.ForeColor = Color.White;
            p2_Score.BackColor = Color.Transparent;

            p1_Score_Count.Name = "lbl_P1_Score_Count";
            p1_Score_Count.Text = "0";
            p1_Score_Count.Location = new Point((int)(columnPartition * 1.1), (int)((rowPartition * 0.1) + (rowPartition / 4)));
            p1_Score_Count.Size = new Size(columnPartition, rowPartition / 4);
            p1_Score_Count.ForeColor = Color.White;
            p1_Score_Count.BackColor = Color.Transparent;

            p2_Score_Count.Name = "lbl_P2_Score_Count";
            p2_Score_Count.Text = "0";
            p2_Score_Count.Location = new Point((columnPartition * 5), (int)((rowPartition * 0.1) + (rowPartition / 4)));
            p2_Score_Count.Size = new Size(columnPartition, rowPartition / 4);
            p2_Score_Count.ForeColor = Color.White;
            p2_Score_Count.BackColor = Color.Transparent;

            p1_Time.Name = "lbl_P1_Time";
            p1_Time.Text = "Elapsed Time : ";
            p1_Time.Location = new Point((int)(columnPartition * 0.1), (int)((rowPartition * 0.1) + (rowPartition / 2)));
            p1_Time.Size = new Size(columnPartition, rowPartition / 4);
            p1_Time.ForeColor = Color.White;
            p1_Time.BackColor = Color.Transparent;

            p2_Time.Name = "lbl_P2_Time";
            p2_Time.Text = "Elapsed Time : ";
            p2_Time.Location = new Point(columnPartition * 4, (int)((rowPartition * 0.1) + (rowPartition / 2)));
            p2_Time.Size = new Size(columnPartition, rowPartition / 4);
            p2_Time.ForeColor = Color.White;
            p2_Time.BackColor = Color.Transparent;

            p1_Elapsed_Time.Name = "lbl_P1_Elapsed_Time";
            p1_Elapsed_Time.Text = "0";
            p1_Elapsed_Time.Location = new Point((int)(columnPartition * 1.1), (int)((rowPartition * 0.1) + (rowPartition / 2)));
            p1_Elapsed_Time.Size = new Size(columnPartition, rowPartition / 4);
            p1_Elapsed_Time.ForeColor = Color.White;
            p1_Elapsed_Time.BackColor = Color.Transparent;

            p2_Elapsed_Time.Name = "lbl_P2_Elapsed_Time";
            p2_Elapsed_Time.Text = "0";
            p2_Elapsed_Time.Location = new Point(columnPartition * 5, (int)((rowPartition * 0.1) + (rowPartition / 2)));
            p2_Elapsed_Time.Size = new Size(columnPartition, rowPartition / 4);
            p2_Elapsed_Time.ForeColor = Color.White;
            p2_Elapsed_Time.BackColor = Color.Transparent;

            timeElapsed.Name = "lbl_Time_Elapsed";
            timeElapsed.Text = "Time Elapsed: ";
            timeElapsed.Location = new Point(columnPartition*2,(int)(rowPartition*0.1));
            timeElapsed.Size = new Size(columnPartition, rowPartition/4);
            timeElapsed.ForeColor = Color.White;
            timeElapsed.BackColor = Color.Transparent;

            gameTime.Name = "lbl_Game_Time";
            gameTime.Text = "0";
            gameTime.Location = new Point(columnPartition * 3, (int)(rowPartition * 0.1));
            gameTime.Size = new Size(columnPartition, rowPartition / 4);
            gameTime.ForeColor = Color.White;
            gameTime.BackColor = Color.Transparent;

            // Adding the Labels to the Form's Controls
            Controls.Add(p1_userName);
            Controls.Add(p2_userName);
            Controls.Add(p1_Score);
            Controls.Add(p2_Score);
            Controls.Add(p1_Score_Count);
            Controls.Add(p2_Score_Count);
            Controls.Add(p1_Time);
            Controls.Add(p2_Time);
            Controls.Add(p1_Elapsed_Time);
            Controls.Add(p2_Elapsed_Time);
            Controls.Add(timeElapsed);
            Controls.Add(gameTime);
            Console.WriteLine("\nControls added to Form - Testing\n");

            /* Each set contains contains a selected Language's words
             * Each pair will contains a randomized set of the selected Language's words
             * The scope is to randomize the language's words that are going to be used
             */
            for (int i = 0; i < numOfPairs; i++)
            {
                int index = random.Next(0, set1.Count);
                pair1.Add(set1[index]);
                pair2.Add(set2[index]);
                set1.RemoveAt(index);
                set2.RemoveAt(index);
            }
            Console.WriteLine("\nWords randomized - Testing\n");

            /* Assigning a random pair of buttons to a Tuple.
             * Each two buttons in a Pair are unlikely to be positioned adjacently, as they are chosen randomly from the
             * list of ramianing buttons, however they contain matching words in different languages.
             * The tuples are used to identify which buttons are joined in a pair */
            while (tempButtons.Count != 0)
            {
                Button_Custom btn_1 = tempButtons[random.Next(0, (tempButtons.Count - 1))];
                tempButtons.Remove(btn_1);
                Button_Custom btn_2 = tempButtons[random.Next(0, (tempButtons.Count - 1))];
                tempButtons.Remove(btn_2);

                // Setting the words assigned to each button, and the word's language
                btn_1.word = pair1[wordCounter];
                btn_2.word = pair2[wordCounter];
                btn_1.language = language_1.name;
                btn_2.language = language_2.name;

                // Assigning two randomly positioned buttons with matching words to a Tuple, which is addded to a list
                buttonPairs.Add(new Tuple<Button_Custom, Button_Custom>(btn_1, btn_2));
                wordCounter++; // Points to the next pair of words
            }
            Console.WriteLine("\nButton Pair tuples created - Testing\n");

            Console.WriteLine("\nGame UI created - Testing\n");
            // Prompts the Player to Start Playing
            MessageBox.Show($"{player1.userName}, you may start the game!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        // This method is called every time a button is clicked, and recieves the button as an argument
        private void buttonClicked(Button_Custom button)
        {
            if (!began) // If the game hasn't begun yet, start the timer, stopwatches and set 'began' to true
            {
                Console.WriteLine("\nGame has not started yet - Testing\n");
                gameTimer.Start();
                gameWatch.Start();
                p1_Watch.Start();
                began = true;
                Console.WriteLine("\nPlayer began game\nTimer and Player 1 Stopwatch started - Testing\n");
            }
            else
            {
                Console.WriteLine("\nGame has already started - Testing\n");
            }

            selectedButtons.Add(button); // Add the pressed button to a List consisting of pressed buttons by the Player
            button.activated = true; // States that the button has been activated the player, to avoid being activated again sequentially

            if (selectedButtons.Count==2)  // If a pair (2) of buttons has been selected
            {
                currentPlayer.first_Word.Add(selectedButtons[0].word);
                currentPlayer.second_Word.Add(selectedButtons[1].word);
                currentPlayer.first_Language.Add(selectedButtons[0].language);
                currentPlayer.second_Language.Add(selectedButtons[1].language);
                currentPlayer.game_No.Add(Welcome_Form.current_Games.Count + 1);

                // Get the indexes of the tuples storing the selected buttons
                // Index of First button's Tuple
                int index1 = buttonPairs.IndexOf(buttonPairs.FirstOrDefault((t) => t.Item1 == selectedButtons[0] || t.Item2 == selectedButtons[0]));
                // Index of Second button's Tuple
                int index2 = buttonPairs.IndexOf(buttonPairs.FirstOrDefault((t) => t.Item1 == selectedButtons[1] || t.Item2 == selectedButtons[1]));

                if (index1 == index2) // If the buttons are from the same tuple, thus a match
                {
                    // Pauses thread temporarily to allow user to view the button's associated words
                    Thread.Sleep(400);
                    // Set the buttons to invisible
                    selectedButtons[0].Visible = false;
                    selectedButtons[1].Visible = false;
                    selectedButtons[0].Enabled = false;
                    selectedButtons[1].Enabled = false;

                    // Update the current player's score
                    currentPlayer.score++;
                    currentPlayer.guessed.Add(true);

                    // Update the Player score label, and decrement the remaining amount of word-pairs to guess
                    updateScoreLabels();
                    pairsLeft--;

                    if (pairsLeft == 0) // If all of the pairs have been guessed
                    {
                        finished = true; // State that the game is finished
                        // Stop the Timers and stopwatches
                        gameTimer.Stop();
                        gameWatch.Stop();
                        p1_Watch.Stop();
                        p2_Watch.Stop();

                        // Add the players' elapsed times and scores to the pertinent players
                        player1.times.Add(Convert.ToInt32(p1_Watch.ElapsedMilliseconds));
                        player2.times.Add(Convert.ToInt32(p2_Watch.ElapsedMilliseconds));
                        player1.scores.Add(player1.score);
                        player2.scores.Add(player2.score);

                        /* Determining which player one the game
                         * Increment the games won/lost property
                         * State the winner */
                        if (player1.score > player2.score)
                        {
                            player1.gamesWon++;
                            player2.gamesLost++;
                            winner = player1.userName;
                            player1.won_Lost.Add(true);
                            player2.won_Lost.Add(false);
                            MessageBox.Show($"{player1.userName} is the Winner!", "Winner", 0, MessageBoxIcon.Information);
                        }else if(player2.score > player1.score)
                        {
                            player2.gamesWon++;
                            player1.gamesLost++;
                            winner = player2.userName;
                            player1.won_Lost.Add(false);
                            player2.won_Lost.Add(true);
                            MessageBox.Show($"{player2.userName} is the Winner!", "Winner", 0, MessageBoxIcon.Information);
                        }
                        else // Else if the score is equal, the winner is based on their elapsed time
                        {
                            if (p1_Watch.ElapsedMilliseconds < p2_Watch.ElapsedMilliseconds) // If player 1 took less time to play
                            {
                                player1.gamesWon++;
                                player2.gamesLost++;
                                winner = player1.userName;
                                player1.won_Lost.Add(true);
                                player2.won_Lost.Add(false);
                                MessageBox.Show($"{player1.userName} is the Winner!", "Winner", 0, MessageBoxIcon.Information);
                            }
                            else // If player 2 took less time to play
                            {
                                player2.gamesWon++;
                                player1.gamesLost++;
                                winner = player2.userName;
                                player1.won_Lost.Add(false);
                                player2.won_Lost.Add(true);
                                MessageBox.Show($"{player2.userName} is the Winner!", "Winner", 0, MessageBoxIcon.Information);
                            }
                        }
                        // Get the existing Users
                        List<User> users = Welcome_Form.current_Users;
                        /* Check whether player 1/2 are registered users, by checking their username
                         * If so, update the user's properties */
                        foreach(User user in users)
                        {
                            if (user.userName.Equals(player1.userName))
                            {
                                Console.WriteLine("\nPlayer 1 is a registered User - Testing\n");
                                user.gamesWon = player1.gamesWon;
                                user.gamesLost = player1.gamesLost;
                                user.scores = player1.scores;
                                user.times = player1.times;
                                user.first_Language = player1.first_Language;
                                user.second_Language = player1.second_Language;
                                user.first_Word = player1.first_Word;
                                user.second_Word = player1.second_Word;
                                user.guessed = player1.guessed;
                                user.game_No = player1.game_No;
                                user.gamesPlayed++;
                                user.won_Lost = player1.won_Lost;
                            }else if (user.userName.Equals(player2.userName))
                            {
                                Console.WriteLine("\nPlayer 2 is a registered User - Testing\n");
                                user.gamesWon = player2.gamesWon;
                                user.gamesLost = player2.gamesLost;
                                user.scores = player2.scores;
                                user.times = player2.times;
                                user.first_Language = player2.first_Language;
                                user.second_Language = player2.second_Language;
                                user.first_Word = player2.first_Word;
                                user.second_Word = player2.second_Word;
                                user.guessed = player2.guessed;
                                user.game_No = player2.game_No;
                                user.gamesPlayed++;
                                user.won_Lost = player2.won_Lost;
                            }
                        }
                        // Write the updated list of Users to the Users' file
                        Library.writeUsers(users);
                        // Append a new instance of Game Record to the Games' file
                        Library.appendGame(new GameRecord(Convert.ToInt32(gameWatch.ElapsedMilliseconds), player1.userName,
                            player2.userName, language_1, language_2, numOfPairs, winner, Welcome_Form.current_Games.Count+1,player1.score,player2.score));
                        Welcome_Form.current_Games = Library.getGames();
                        // Prompt given to the user to go back to the home screen
                        DialogResult result = MessageBox.Show("Do you wish to return to the Home Page?", "Notification", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            Console.WriteLine("\nPlayer decided to return to Welcome Form - Testing\n");
                            Close();
                        }
                        else
                        {
                            Console.WriteLine("\nPlayer decided to remain within Game Form - Testing\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"\nThere are {pairsLeft} pairs left to Guess - Testing\n");
                    }
                    /* Since a pair of buttons have been selected, empty the List containing them, 
                    index1 order to store the references of the upcoming buttons to be selected */
                    selectedButtons.Clear();
                    Console.WriteLine("\nList of Selected Buttons cleared - Testing\n");
                }
                else // If the buttons are not from the same tuple
                {
                    currentPlayer.guessed.Add(false);
                    
                    // Pause the Current thread temporarily for the player to view the buttons' words, then refresh their properties
                    Thread.Sleep(400);
                    foreach(Button_Custom button_Cust in selectedButtons)
                    {
                        button_Cust.Text = "?";
                        button_Cust.activated = false;
                    }

                    // Clear the List storing references to the selected buttons, and switch the current Player
                    selectedButtons.Clear();
                    switchPlayer();
                }
            }
            else
            {
                Console.WriteLine("\nOnly 1 button has been selected - Testing\n");
            }
        }
        
        // Switches the reference to the Current Player
        private void switchPlayer()
        {
            if (currentPlayer == player1)
            {
                currentPlayer = player2;
                p1_Watch.Stop();
                p2_Watch.Start();
            }
            else
            {
                currentPlayer = player1;
                p2_Watch.Stop();
                p1_Watch.Start();
            }
        }

        // Updates the Form's Player Score labels
        private void updateScoreLabels()
        {
            p1_Score_Count.Text = player1.score.ToString();
            p1_Score_Count.Refresh();
            p2_Score_Count.Text = player2.score.ToString();
            p2_Score_Count.Refresh();
        }
    }
}