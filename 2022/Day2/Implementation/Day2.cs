namespace RockPaperScissors;

public class Day2
{
    public static Result GetRoundResult(Selection mainSelection, Selection opposingSelection)
    {
        if (mainSelection == opposingSelection)
        {
            return Result.Draw;
        }

        if (
            (mainSelection == Selection.Rock && opposingSelection == Selection.Scissors)
            || (mainSelection == Selection.Paper && opposingSelection == Selection.Rock)
            || (mainSelection == Selection.Scissors && opposingSelection == Selection.Paper)
        )
        {
            return Result.Win;
        }

        return Result.Loss;
    }

    public static int GetRoundScore(Selection mainSelection, Result roundResult){
        int score = 0;
        
        score += mainSelection switch
        {
            Selection.Rock => 1,
            Selection.Paper => 2,
            Selection.Scissors => 3,
            _ => throw new InvalidDataException()
        };

        score += roundResult switch
        {
            Result.Loss => 0,
            Result.Draw => 3,
            Result.Win => 6,
            _ => throw new InvalidDataException()
        };

        return score;
    }

    public static int ParseStrategyLine(string line, bool isForPart1)
    {
        if(line.Length < 3)
        {
            throw new Exception("Unexpected line length");
        }

        Selection opposingSelection = line[0] switch
        {
            'A' => Selection.Rock,
            'B' => Selection.Paper,
            'C' => Selection.Scissors,
            _ => throw new Exception("Unexpected character")
        };

        Selection mainSelection;
        if (isForPart1)
        {
            mainSelection = line[2] switch
            {
                'X' => Selection.Rock,
                'Y' => Selection.Paper,
                'Z' => Selection.Scissors,
                _ => throw new Exception("Unexpected character")
            };
        }
        else
        {
            mainSelection = (opposingSelection, line[2]) switch
            {
                // X => Loss
                (Selection.Rock,'X') => Selection.Scissors,
                (Selection.Paper,'X') => Selection.Rock,
                (Selection.Scissors,'X') => Selection.Paper,
                
                // Y => Draw
                (_,'Y') => opposingSelection,

                // Z => Win
                (Selection.Rock, 'Z') => Selection.Paper,
                (Selection.Paper, 'Z') => Selection.Scissors,
                (Selection.Scissors, 'Z') => Selection.Rock,

                _ => throw new Exception("Unexpected character combination")
            };
        }

        return GetRoundScore(mainSelection, GetRoundResult(mainSelection, opposingSelection));
    }



    private static void Main(string[] args)
    {
        using StreamReader inputStream = new("input.txt");
        string? nextLine;
        int totalScore = 0;
        while((nextLine = inputStream.ReadLine()) is not null)
        {
            totalScore += ParseStrategyLine(nextLine,isForPart1:false);
        }
        Console.WriteLine(totalScore);
    }
}
