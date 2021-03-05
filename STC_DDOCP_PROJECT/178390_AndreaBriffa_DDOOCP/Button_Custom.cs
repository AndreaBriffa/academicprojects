namespace _178390_AndreaBriffa_DDOOCP
{
    public partial class Button_Custom : System.Windows.Forms.Button
    {
        public string word { get; set; } // Word associated to the Button
        public string language { get; set; } // The language of the word associated with the Button
        public bool activated { get; set; } 
        /* Whether the button has been selected/pressed or not.
        This will solve the issue of a User clicking a button twice, and the program registering the same button as a pair,
        by checking whether the button has been activated or not. A button is de-activated by default.*/

        public Button_Custom()
        {
            activated = false;
        }
    }
}
