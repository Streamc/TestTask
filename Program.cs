using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestTask
{
    public class Program
    {

        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        static void Main(string[] args)
        {
            string path = @"C:\Test\mxc.txt";
            //string path2 = @"C:\Test\mxc2.txt";

            ReadOnlyStream inputStream1 = GetInputStream(path);


            IList<LetterStats> letterStats = FillLetterStatsDouble(inputStream1);
            // IList<LetterStats> letterStats = FillLetterStatsDouble(inputStream1);



            var removeletterd = RemoveCharStatsByType(letterStats, CharType.Consonants);

            IList<LetterStats> LetterStats_grouped = GroupStats(letterStats); // группа статистики
            IList<LetterStats> lstatsremoved = GroupStats(removeletterd); //  удаленные гласные или согласные группами



            foreach (var result in LetterStats_grouped)
            {
                Console.WriteLine("Name: {0}, Count: {1}", result.Letter, result.Count);
            }




            foreach (var result in lstatsremoved)
            {
                Console.WriteLine("NameDeleted: {0}, Countdeleted: {1}", result.Letter, result.Count);
            }


            //  IList<LetterStats> LetterStats_groupedStats = FillLetterStats_groupedStats(inputStream2);

            RemoveCharStatsByType(letterStats, CharType.Vowel);
            // RemoveCharStatsByType(LetterStats_groupedStats, CharType.Consonants);

            //   PrintStatistic(letterStats);
            //  PrintStatistic(LetterStats_groupedStats);*/

            // TODO : Необжодимо дождаться нажатия клавиши, прежде чем завершать выполнение программы.
            Console.ReadLine();
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static ReadOnlyStream GetInputStream(string fileFullPath)
        {

            ReadOnlyStream nx = new ReadOnlyStream(fileFullPath);
            nx.fileFullPath = fileFullPath;
            // FileStream sr = new FileStream(fileFullPath, FileMode.OpenOrCreate);
            // nx.fstream = sr;
            return nx;

        }

        private static IList<LetterStats> GroupStats(IList<LetterStats> letterStats)
        {
            IList<LetterStats> LetterStats_grouped = new List<LetterStats>();
            var query = letterStats.SelectMany(x => x.Letter)
               .GroupBy(s => s)
               .Select(g => new { Name = g.Key, Count = g.Count() }).ToList();

            foreach (var result in query)
            {
                //Console.WriteLine("Name: {0}, Count: {1}", result.Name, result.Count);
                LetterStats nx;
                nx.Letter = result.Name.ToString();
                nx.Count = result.Count;
                LetterStats_grouped.Add(nx);

            }
            return LetterStats_grouped;

        }



        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(ReadOnlyStream stream)
        {

            List<LetterStats> lstats = new List<LetterStats>();
            LetterStats nx = new LetterStats();
            var sr = new StreamReader(stream.fstream);
            // stream.ResetPositionToStart();
            //StreamReader sr = new StreamReader(streamex._localStream);
            // while (!stream.IsEof)
            while (sr.Peek() > 0)
            {
                //   char c = stream.ReadNextChar();
                // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - регистрозависимый.
                nx.Letter = ((char)sr.Read()).ToString();
                nx.Count = 1;
                //Console.WriteLine(nx.Letter);
                if (char.IsLetter(Convert.ToChar(nx.Letter)))
                    lstats.Add(nx);
            }

            return lstats;

        }
        private static IList<LetterStats> FillLetterStatsDouble(ReadOnlyStream stream)
        {

            List<LetterStats> lstats = new List<LetterStats>();
            LetterStats nx = new LetterStats
            {
                Letter = "",
                Count = 1
            };

            var sr = new StreamReader(stream.fstream);
            // stream.ResetPositionToStart();
            //StreamReader sr = new StreamReader(streamex._localStream);
            // while (!stream.IsEof)
            while (sr.Peek() > 0)
            {


                //   char c = stream.ReadNextChar();
                // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - регистрозависимый.
                var buff = ((char)sr.Read()).ToString();
                if ((buff == nx.Letter) && char.IsLetter(Convert.ToChar(buff))) lstats.Add(nx);

                nx.Count = 1;
                nx.Letter = buff;
                //Console.WriteLine(nx.Letter);
                //if (char.IsLetter(Convert.ToChar(buff)))
            }

            return lstats;

        }




        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillLetterStats_groupedStats(IReadOnlyStream stream)
        {
            stream.ResetPositionToStart();
            while (!stream.IsEof)
            {
                char c = stream.ReadNextChar();
                // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - НЕ регистрозависимый.
            }

            //return ???;

            throw new NotImplementedException();
        }

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        private static IList<LetterStats> RemoveCharStatsByType(IList<LetterStats> letters, CharType charType)
        {


            IList<LetterStats> result = new List<LetterStats>();
            char[] gx = new char[55];


            IList<LetterStats> resultnot = letters;
            // TODO : Удалить статистику по запрошенному типу букв.
            switch (charType)
            {
                case CharType.Consonants:
                    gx = "BCDFGHJKLMNPQRSTVWXYZbcdfghjklmnpqrstvwxyz".ToCharArray();
                    break;
                case CharType.Vowel:
                    gx = "AEIOUYaeiou".ToCharArray();
                    break;
            }

            var gg = gx.ToString();


            foreach (char bukva in gx)

            {

                LetterStats bb1 = resultnot.Where(t => t.Letter == bukva.ToString()).FirstOrDefault();

                if (resultnot.Contains(bb1))

                {

                    resultnot = resultnot.Where(t => t.Letter != bb1.Letter).ToList();
                }

            }

            return resultnot;



        }

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            // TODO : Выводить на экран статистику. Выводить предварительно отсортировав по алфавиту!
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        private static void IncStatistic(LetterStats letterStats)
        {
            letterStats.Count++;
        }


    }
}
