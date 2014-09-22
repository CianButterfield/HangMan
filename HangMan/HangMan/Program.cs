using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangMan
{
    class Program
    {
        public static string name = "";

        static void Main(string[] args)
        {  
            Console.Title = "HANGMAN";
            Greet();
            HangMan();
            //Console.ReadKey();
        }

        static string WordSelector()
        {
            //List of words
            List<string> wordList = new List<string>() {"attack", "belief", "birth", "comfort", "cork", "desire", "distribution", "earth", "experience", "feeling", "fold", "government", "humor", 
                "insect", "knowledge", "learning", "manager", "nation", "observation", "paper", "question", "reason", "science", "thunder", "unit", "vocabulary", "wishing", "xylophone", "yellow", "zebra"};
            //Get random word from list
            Random r = new Random();
            string word = wordList[r.Next(0, wordList.Count + 1)];
            return word;
        }

        static void Greet()
        {
            //Get name, write hello & explanation
            Console.Write("Enter your name: ");
            name = Console.ReadLine();
            Console.WriteLine("Hello, " + name + ".\nYou will be guessing letters of a word until you guess 7 incorrect letters, or  you guess all of the letters");
        }

        static string Output(string word, string allGuess)
        {
            //String to build on
            string output = string.Empty;
            //Loop through letters of word
            for (int i = 0; i < word.Length; i++)
            {
                //If your guess is in i, add letter to output
                if (allGuess.Contains(word[i]))
                {
                    output += word[i];
                }
                //Otherwise add an underscore
                else
                {
                    output += "_";
                }
                //Space for counting letters 
                output += " ";
            }
            Console.Write("Word: " + output + "\n");
            //Remove spaces so it looks like an English word
            return output.Replace(" ", string.Empty);
        }

        static void PlayAgain()
        {
            Console.Title = "Play again?";
            //Ask if they want to play again
            Console.WriteLine("\nWould you like to play again? Type yes, y or Y to play again.");
            
            string playAgain = Console.ReadLine();
            //If y, Y, yes or Yes, play again, otherwise quit
            if (playAgain == "y" || playAgain == "yes" || playAgain == "Y")
            {
                HangMan();
            }
        }

        static void HangMan()
        {
            //Get word to guess
            string word = WordSelector();
            
            //Guess count to check if user has lost
            int guessCount = 7;

            //String for tracking all guesses
            string allGuess = string.Empty;            

            //Bool for checking if user has won
            bool playing = true;
            while (playing)
            {
                if (guessCount > 0)
                {
                    //Output word as underscores + correct letters
                    string output = Output(word, allGuess);

                    //Get user guess
                    string guess = Console.ReadLine().ToLower();
                    //Add guess to string of guesses
                    if (allGuess.Contains(guess))
                    {
                        Console.WriteLine("You already guessed that letter.");
                    }

                    //Check to see if output = word
                    if (output == word)
                    {
                        Console.Clear();
                        Console.WriteLine("You win!!");
                        playing = false;
                    }

                    //Check if char or string
                    //Run this if guess is word
                    if (guess.Length > 1)
                    {
                        //They guessed the word
                        if (guess == word)
                        {
                            Console.Clear();
                            Console.WriteLine("You win!");
                            playing = false;
                        }
                        //Incorrect word guess
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Wrong guess, buddy");
                            guessCount--;
                            Console.WriteLine("Guesses left: " + guessCount + "\n");
                            Console.WriteLine("Letters guessed: " + allGuess);       
                        }
                    }
                    //Guess = character
                    else
                    {
                        if (!allGuess.Contains(guess))
                        {
                            allGuess += guess + " ";
                            //Correct letter guessed
                            if (word.Contains(guess))
                            {
                                //Clear console for readability
                                Console.Clear();
                                Console.WriteLine("You got a letter\n");
                                Console.WriteLine("Guesses left: " + guessCount);
                                Console.WriteLine("Letters guessed: " + allGuess);
                            }
                            //Incorrect letter guessed
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Incorrect guess.\n");
                                guessCount--;
                                Console.WriteLine("Guesses left: " + guessCount);
                                Console.WriteLine("Letters guessed: " + allGuess);
                            }

                        }
                    }
                }
                //Failed at guessing at letters
                else
                {
                    Console.Clear();
                    Console.WriteLine("You suck at guessing letters. \nThe word was: " + word);
                    playing = false;
                }
            }


            //give the user a moment to read
            System.Threading.Thread.Sleep(2500);
            //add high score
            AddHighScore(7 - guessCount);
            //display highscores
            DisplayHighScores();

            PlayAgain();
        }

        static void AddHighScore(int playerScore)
        {

            //create a gateway to the database
            CianEntities db = new CianEntities();

            //create a new high score object
            // fill it with our user's data
            HighScore newHighScore = new HighScore();
            newHighScore.DateCreated = DateTime.Now;
            newHighScore.Game = "Hangman";
            newHighScore.Name = name;
            newHighScore.Score = playerScore;

            //add it to the database
            db.HighScores.Add(newHighScore);

            //save our changes
            db.SaveChanges();
        }

        static void DisplayHighScores()
        {
            //clear the console
            Console.Clear();
            Console.Title = "ΦHangman High ScoresΦ";
            Console.WriteLine("Hangman High Scores");
            Console.WriteLine("≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡");

            //create a new connection to the database
            CianEntities db = new CianEntities();
            //get the high score list
            List<HighScore> highScoreList = db.HighScores.Where(x => x.Game == "Hangman").OrderBy(x => x.Score).Take(10).ToList();

            foreach (HighScore highScore in highScoreList)
            {
                Console.WriteLine("{0}. {1} - {2}", highScoreList.IndexOf(highScore) + 1, highScore.Name, highScore.Score);
            }
        }
    }
}