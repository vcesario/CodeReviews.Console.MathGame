public static class Core
{
    struct HistoryEntry
    {
        public char operation;
        public int difficulty;
        public float score;
        public TimeSpan duration;

        public HistoryEntry(char operation, int difficulty, float score, TimeSpan duration)
        {
            this.operation = operation;
            this.difficulty = difficulty;
            this.score = score;
            this.duration = duration;
        }
    }

    private static List<HistoryEntry> m_History = new List<HistoryEntry>();
    private static Random m_Random = new Random();

    private static readonly int k_NoOfRounds = 7;
    private static int m_Difficulty = 1;

    public static void Run(string[] args)
    {
        int option = -1;

        while (option != 0)
        {
            option = PromptMenuOption();

            switch (option)
            {
                case 1:
                    RunMatch('+');
                    break;
                case 2:
                    RunMatch('-');
                    break;
                case 3:
                    RunMatch('*');
                    break;
                case 4:
                    RunMatch('/');
                    break;
                case 5:
                    RunMatch('#');
                    break;
                case 6:
                    m_Difficulty = PromptDifficulty();
                    break;
                case 7:
                    ShowHistory();
                    break;
                default:
                    break;
            }
        }
    }

    private static int PromptMenuOption()
    {
        Console.Clear();
        PrintHeader();

        Console.WriteLine("1. Addition match");
        Console.WriteLine("2. Subtraction match");
        Console.WriteLine("3. Multiplication match");
        Console.WriteLine("4. Division match");
        Console.WriteLine("5. Random match");
        Console.WriteLine($"6. Change difficulty ({m_Difficulty})");
        Console.WriteLine("7. See history");
        Console.WriteLine("0. Exit\n\n");

        Console.WriteLine("Enter menu option:\n");

        bool isValidOption = false;
        int option = 0;

        do
        {
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                ErasePreviousLine();
            }
            else
            {
                if (int.TryParse(input, out option))
                {
                    if (option < 0)
                    {
                        ErasePreviousLine();
                    }
                    else
                    {
                        isValidOption = true;
                    }
                }
                else
                {
                    ErasePreviousLine();
                }
            }
        } while (!isValidOption);

        return option;
    }

    private static void RunMatch(char matchOperation)
    {
        int score = 0;
        int rounds = k_NoOfRounds;

        DateTime startTime = DateTime.Now;
        DateTime lastTime = DateTime.MinValue;

        for (int i = 0; i < rounds; i++)
        {
            int opA = 0;
            int opB = 0;
            int result = 0;
            char roundOperation;

            if (matchOperation.Equals('#'))
            {
                char[] availableOperations = new char[] { '+', '-', '*', '/' };
                roundOperation = availableOperations[m_Random.Next(availableOperations.Length)];
            }
            else
            {
                roundOperation = matchOperation;
            }

            switch (roundOperation)
            {
                case '+':
                    switch (m_Difficulty)
                    {
                        case 1:
                            opA = m_Random.Next(101);
                            opB = m_Random.Next(101);
                            result = opA + opB;
                            break;
                        case 2:
                            opA = m_Random.Next(10, 1001);
                            opB = m_Random.Next(10, 1001);
                            result = opA + opB;
                            break;
                        case 3:
                            opA = m_Random.Next(100, 10001);
                            opB = m_Random.Next(100, 10001);
                            result = opA + opB;
                            break;
                        default:
                            break;
                    }
                    break;
                case '-':
                    switch (m_Difficulty)
                    {
                        case 1:
                            opB = m_Random.Next(101);
                            result = m_Random.Next(101);
                            opA = opB + result;
                            break;
                        case 2:
                            opB = m_Random.Next(10, 1001);
                            result = m_Random.Next(10, 1001);
                            opA = opB + result;
                            break;
                        case 3:
                            opB = m_Random.Next(100, 10001);
                            result = m_Random.Next(100, 10001);
                            opA = opB + result;
                            break;
                        default:
                            break;
                    }
                    break;
                case '*':
                    switch (m_Difficulty)
                    {
                        case 1:
                            opA = m_Random.Next(101);
                            opB = m_Random.Next(11);
                            result = opA * opB;
                            break;
                        case 2:
                            opA = m_Random.Next(10, 1001);
                            opB = m_Random.Next(0, 101);
                            result = opA * opB;
                            break;
                        case 3:
                            opA = m_Random.Next(100, 10001);
                            opB = m_Random.Next(10, 1001);
                            result = opA * opB;
                            break;
                        default:
                            break;
                    }
                    break;
                case '/':
                    switch (m_Difficulty)
                    {
                        case 1: // from 0 / 1 to 100 / 10
                            opB = m_Random.Next(1, 11);
                            result = m_Random.Next(11);
                            opA = opB * result;
                            break;
                        case 2: // from 0 / 1 to 1000 / 100
                            opB = m_Random.Next(1, 101);
                            result = m_Random.Next(11);
                            opA = opB * result;
                            break;
                        case 3: // from 100 / 10 to 10000 / 100
                            opB = m_Random.Next(10, 101);
                            result = m_Random.Next(10, 101);
                            opA = opB * result;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            int attempt = PromptOperation(opA, opB, roundOperation, i + 1, rounds, startTime);

            Console.WriteLine($"Your answer:\t{attempt}");
            Console.WriteLine($"Right answer:\t{result}\n");

            if (attempt == result)
            {
                Console.Write("SUCCESS! [Continue...]");
                score++;
            }
            else
            {
                Console.Write("[Continue...]");
            }

            lastTime = DateTime.Now;
            Console.ReadLine();
        }

        float finalScore = (float)score / rounds * 100;
        TimeSpan matchDuration = lastTime - startTime;

        StoreMatch(matchOperation, m_Difficulty, finalScore, matchDuration);

        ShowGameOver(finalScore, matchDuration);
    }

    private static int PromptOperation(int a, int b, char operation, int currentRound, int totalRounds, DateTime startTime)
    {
        Console.Clear();
        PrintHeader();

        Console.WriteLine($"Round: {currentRound.ToString("D2")}/{totalRounds.ToString("D2")}"
            + $"\t\tTime elapsed: {(DateTime.Now - startTime).ToString(@"hh\:mm\:ss")}\n");
        Console.WriteLine($"{a} {operation} {b} = ?\n\n");

        bool isValidOption = false;
        int attempt = 0;

        do
        {
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                ErasePreviousLine();
            }
            else
            {
                if (int.TryParse(input, out attempt))
                {
                    ErasePreviousLine();
                    isValidOption = true;
                }
                else
                {
                    ErasePreviousLine();
                }
            }
        } while (!isValidOption);

        return attempt;
    }

    private static void ShowGameOver(float finalScore, TimeSpan duration)
    {
        Console.Clear();
        PrintHeader();

        Console.WriteLine("\t\t* * *   G A M E   O V E R !   * * *\n\n");
        Console.WriteLine($"Final score:\t{finalScore.ToString("#.##")}%\nCompleted in:\t{duration.ToString(@"hh\:mm\:ss")}");

        Console.WriteLine("[Return to menu...]");
        Console.ReadLine();
    }

    private static int PromptDifficulty()
    {
        Console.Clear();
        PrintHeader();

        Console.WriteLine($"Current difficulty: {m_Difficulty}\n");
        Console.WriteLine("Enter new difficulty (1 - 3):\n");

        bool isValidOption = false;
        int option = 0;
        do
        {
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                ErasePreviousLine();
            }
            else
            {
                if (int.TryParse(input, out option) && option >= 1 && option <= 3)
                {
                    isValidOption = true;
                }
                else
                {
                    ErasePreviousLine();
                }
            }
        } while (!isValidOption);

        return option;
    }

    private static void ShowHistory()
    {
        Console.Clear();
        PrintHeader();

        Console.WriteLine("\t\t\tMATCH HISTORY\n");

        if (m_History.Count == 0)
        {
            Console.WriteLine("No matches found.\n");
        }
        else
        {
            Console.WriteLine(@"ID     MODE      DIFFICULTY  SCORE   DURATION");
            for (int i = 0; i < m_History.Count; i++)
            {
                string modeLabel = OperationToLabel(m_History[i].operation);
                string difficultyLabel = DifficultyToLabel(m_History[i].difficulty);

                Console.WriteLine(@$"{("#" + (i + 1).ToString()).PadRight(7)}{modeLabel.PadRight(10)}{difficultyLabel.PadRight(12)}"
                    + @$"{(m_History[i].score.ToString("#.##") + "%").PadRight(8)}{m_History[i].duration.ToString(@"hh\:mm\:ss")}");
            }
            Console.WriteLine();
        }

        Console.WriteLine("[Return to menu...]");
        Console.ReadLine();
    }

    private static void StoreMatch(char operation, int difficulty, float score, TimeSpan duration)
    {
        HistoryEntry entry = new(operation, difficulty, score, duration);
        m_History.Add(entry);
    }

    private static void PrintHeader()
    {
        Console.WriteLine("= = = = =\tV I C T O R ' S   M A T H A P A L O O Z A\t= = = = =");
        Console.WriteLine("\n\n");
    }
    private static void ErasePreviousLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop);
    }
    private static string OperationToLabel(char operation)
    {
        switch (operation)
        {
            case '+':
                return "Add";
            case '-':
                return "Subtract";
            case '*':
                return "Multiply";
            case '/':
                return "Divide";
            case '#':
                return "Random";
            default:
                return "Unknown";
        }
    }
    private static string DifficultyToLabel(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                return "Easy";
            case 2:
                return "Medium";
            case 3:
                return "Hard";
            default:
                return "Unknown";
        }
    }
}