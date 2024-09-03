
using Library.DataModel.Models;
using System;
using System.Text;

var h =IsNumber2Power(8);
PrintOddNumbers();
ReplicateString("ert", 4);
Console.WriteLine(h);

static bool IsNumber2Power(int num)
{
    if (num <= 0)
    {
        return false;
    }
    return (num & (num - 1)) == 0;

}

 static void PrintOddNumbers()
{
    for (int i = 1; i < 100; i += 2)
    {
        Console.WriteLine(i);
    }
}

static string ReplicateString(string input, int times)
{
    if (times <= 0)
        return string.Empty;

    var replicatedStringBuilder = new StringBuilder(input.Length * times);

    for (int i = 0; i < times; i++)
    {
        replicatedStringBuilder.Append(input);
    }
    return replicatedStringBuilder.ToString();
}