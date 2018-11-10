using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Models
{
    public static class GameToFile
    {
        private const string DATA_FILENAME = "MSweeperGame.dat";
        private static IFormatter formatter = new BinaryFormatter();

        public static void Save(Game msGame)
        {
            try
            {
                FileStream writerFileStream = new FileStream(DATA_FILENAME, FileMode.Create, FileAccess.Write);
                formatter.Serialize(writerFileStream, msGame);
                writerFileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to save the Game");
            }
        }

        public static bool isFileExist()
        {
            return File.Exists(DATA_FILENAME);
        }

        public static void RemoveFile()
        {
            if (isFileExist())
                File.Delete(DATA_FILENAME);
        }

        public static Game Load()
        {
            if (isFileExist())
            {
                try
                {
                    FileStream readerFileStream = new FileStream(DATA_FILENAME, FileMode.Open, FileAccess.Read);
                    Game msGame = (Game)formatter.Deserialize(readerFileStream);
                    readerFileStream.Close();
                    return msGame;

                }
                catch (Exception)
                {
                    Console.WriteLine("There seems to be a file that contains game information.");
                } 

            }
            return null;
        }

        
    }
}
