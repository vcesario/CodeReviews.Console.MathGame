public static class Core
{
    private static Random random = new Random();
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

    private static void ErasePreviousLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop);
    }

    private static void RunMatch(char operation)
    {
        int score = 0;
        int rounds = 3;

        for (int i = 0; i < rounds; i++)
        {
            int opA = 0;
            int opB = 0;
            int result = 0;

            switch (operation)
            {
                case '+':
                    opA = random.Next(1000);
                    opB = random.Next(1000);
                    result = opA + opB;
                    break;
                case '-':
                    opA = random.Next(1000);
                    do
                    {
                        opB = random.Next(1000);
                        result = opA - opB;
                    } while (result < 0);
                    break;
                case '*':
                    opA = random.Next(100);
                    opB = random.Next(100);
                    result = opA * opB;
                    break;
                case '/':
                    do
                    {
                        opB = random.Next(100);
                    }
                    while (opB <= 0);
                    result = random.Next(11);
                    opA = opB * result;
                    break;
                default:
                    break;
            }

            int attempt = PromptOperation(opA, opB, operation, i + 1, rounds);

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

            Console.ReadLine();
        }

        float finalScore = (float)score / rounds * 100;
        ShowGameOver(finalScore);
    }

    private static int PromptOperation(int a, int b, char operation, int currentRound, int totalRounds)
    {
        Console.Clear();
        PrintHeader();

        Console.WriteLine($"Round: {currentRound.ToString("D2")}/{totalRounds.ToString("D2")}\n");
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

    private static void ShowGameOver(float finalScore)
    {
        Console.Clear();
        PrintHeader();

        Console.WriteLine("\t\t* * *   G A M E   O V E R !   * * *\n\n");
        Console.WriteLine($"Final score: {finalScore}%\n");

        Console.WriteLine("[Return to menu...]");
        Console.ReadLine();
    }

    private static void PrintHeader()
    {
        Console.WriteLine("= = = = =\tV I C T O R ' S   M A T H A P A L O O Z A\t= = = = =");
        Console.WriteLine("\n\n");
    }
}