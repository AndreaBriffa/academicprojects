using System;

namespace _178390_AndreaBriffa_DDOOCP
{
    public class GameRecord
    {
        public DateTime gameDate {get; set;} // The day the game took place
        public int gameElapsedTime {get; set;}  // The game's total duration
        public string player_1_Username {get; set;} // Player 1's Username
        public string player_2_Username { get; set; } // Player 2's Username
        public Language language_1 {get; set;} // The first Language used in the Game
        public Language language_2 {get; set;}  // The Second Language used in the Game
        public int numOfPairs {get; set;} // The number of word pairs the Game consisted of
        public string winner { get; set; } // The Winner's Username
        public int gameNo { get; set; } // The Game Number i.e. 11th Game so far
        public int player_1_Score { get; set; } // The final score of Player 1
        public int player_2_Score { get; set; } // The final score of Player 2

        // Primary Constructor
        public GameRecord() { }

        // Secondary Constructor
        public GameRecord(int gameElapsedTime, string player_1_Username, 
            string player_2_Username, Language language_1, Language language_2, 
            int numOfPairs, string winner, int gameNo, int player_1_Score, int player_2_Score)
        {
            gameDate = DateTime.UtcNow;
            this.gameElapsedTime = gameElapsedTime;
            this.player_1_Username = player_1_Username;
            this.player_2_Username = player_2_Username;
            this.language_1 = language_1;
            this.language_2 = language_2;
            this.numOfPairs = numOfPairs;
            this.winner = winner;
            this.gameNo = gameNo;
            this.player_1_Score = player_1_Score;
            this.player_2_Score = player_2_Score;
        }
    }
}
