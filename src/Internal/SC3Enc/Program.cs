using SciAdvNet.SC3;
using SciAdvNet.SC3.Text;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace SC3Enc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var instance = new Program();
            instance.Run(args);
        }

        private void Run(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Insufficient arguments.");
                Environment.Exit(0);
            }

            string inputFileName = args[0];
            WriteStringTable(inputFileName);
        }

        private void WriteStringTable(string inputFileName)
        {
            var lines = ReadLines(inputFileName);
            var deserializer = new CustomizedDeserializer();

            var stringTable = new int[lines.Length];
            int stringTableLength = lines.Length * sizeof(int);

            using (var stream = File.Create("output.bin"))
            using (var writer = new BinaryWriter(stream))
            {
                // Fill the table with zeros first
                writer.Write(new byte[stringTableLength]);

                for (int i = 0; i < lines.Length; i++)
                {
                    string s = lines[i];

                    SC3String sc3String;
                    ImmutableArray<byte> bytes;
                    try
                    {
                        sc3String = deserializer.Deserialize(s);
                        bytes = sc3String.Encode(SC3Game.SteinsGateHD);
                    }
                    catch
                    {
                        stream.Dispose();
                        Console.WriteLine("String encoding failed.");
                        Environment.Exit(0);
                    }

                    int offset = (int)stream.Position;
                    stringTable[i] = offset;

                    writer.Write(bytes.ToArray());
                }

                // Write the actual table
                writer.Seek(0, SeekOrigin.Begin);
                foreach (int value in stringTable)
                {
                    writer.Write(value);
                }
            }
        }

        private string[] ReadLines(string inputFileName)
        {
            string[] lines = { };
            try
            {
                lines = File.ReadAllLines(inputFileName);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }

            return lines;
        }
    }
}
