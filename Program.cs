using System;
using System.Security.Principal;

public class Program
{
    public static string[] files = new string[] { "clutter.asm", "clutter.bin", "nasm.exe" };

    public static void Main()
    {
        if (!(new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
        {
            Console.WriteLine("The program is not run with Administrator privileges. Press ENTER to exit.");
            Console.ReadLine();
            Environment.Exit(0);
            return;
        }

        foreach (string file in files)
        {
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
            }
        }

        Console.Title = "CustomMBROverwriter | Made by https://github.com/GabryB03/";
        Console.WriteLine("[Confirmation 1/2] Are you sure you want to overwrite your MBR? (y/n)");
        string color = "", text = "";

        if (!WantToOverwrite())
        {
            Environment.Exit(0);
            return;
        }

        color = GetBIOSColor();
        Console.WriteLine("Please, insert the text to be written (it can include backslash characters, for example '\\r\\n' for a new line).");
        text = Console.ReadLine();
        MBROverwriter.OverwriteMBR(text, color);
        Console.WriteLine("MBR has been succesfully overwrited. The system can not be accessed anymore from now.\r\n" +
            "Press ENTER in order to exit from the program.");
        Console.ReadLine();
    }

    private static bool WantToOverwrite()
    {
        string response = "";

        while (response != "y" && response != "n")
        {
            response = Console.ReadLine();

            if (response != "y" && response != "n")
            {
                Console.WriteLine("Please, digit a valid answer.");
            }
            else if (response == "n")
            {
                return false;
            }
        }

        response = "";
        Console.WriteLine("[Confirmation 2/2] Are you sure you want to overwrite your MBR? (y/n)");

        while (response != "y" && response != "n")
        {
            response = Console.ReadLine();

            if (response != "y" && response != "n")
            {
                Console.WriteLine("Please, digit a valid answer.");
            }
            else if (response == "n")
            {
                return false;
            }
        }

        return true;
    }

    private static string GetBIOSColor()
    {
        string color = "";
        Console.WriteLine("Please, choose the BIOS color to use for the text:\r\n" +
            "0) Black\r\n" +
            "1) Blue\r\n" +
            "2) Green\r\n" +
            "3) Cyan\r\n" +
            "4) Red\r\n" +
            "5) Magenta\r\n" +
            "6) Brown\r\n" +
            "7) Light Gray\r\n" +
            "8) Dark Gray\r\n" +
            "9) Light Blue\r\n" +
            "10) Light Green\r\n" +
            "11) Light Cyan\r\n" +
            "12) Light Red\r\n" +
            "13) Light Magenta\r\n" +
            "14) Yellow\r\n" +
            "15) White");

        while (!IsBIOSColorValid(color))
        {
            color = Console.ReadLine();

            if (!IsBIOSColorValid(color))
            {
                Console.WriteLine("Please, insert a valid BIOS color.");
            }
        }

        return color;
    }     

    private static bool IsBIOSColorValid(string color)
    {
        return sbyte.TryParse(color, out sbyte theColor) && theColor >= 0 && theColor <= 15;
    }
}