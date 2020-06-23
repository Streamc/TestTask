using System;
using System.IO;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        public Stream _localStream; //Why? 

        public string fileFullPath;
        public  FileStream fstream;
        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// 

        public ReadOnlyStream( )
        {
            //  using (FileStream fstream = new FileStream($"{fileFullPath}", FileMode.OpenOrCreate))
            //  {
            FileStream fstream = new FileStream($"{fileFullPath}", FileMode.OpenOrCreate);

            // преобразуем строку в байты
            byte[] array = System.Text.Encoding.Default.GetBytes(fileFullPath);
            // запись массива байтов в файл
            // fstream.Write(array, 0, array.Length);
            _localStream = fstream;
            fileFullPath = fstream.Name;

            IsEof = true;

            // TODO : Заменить на создание реального стрима для чтения файла!
            //_localStream = null;

        }




        public ReadOnlyStream(string fileFullPath)
        {
            //  using (FileStream fstream = new FileStream($"{fileFullPath}", FileMode.OpenOrCreate))
            //  {
            FileStream fstreamconst = new FileStream($"{fileFullPath}", FileMode.OpenOrCreate);
            
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(fileFullPath);
                // запись массива байтов в файл
                // fstream.Write(array, 0, array.Length);
                _localStream = fstream;
            fstream = fstreamconst;

            IsEof = true;

            // TODO : Заменить на создание реального стрима для чтения файла!
            //_localStream = null;

        }

        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof
        {
            get; // TODO : Заполнять данный флаг при достижении конца файла/стрима при чтении
            private set;
        }

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            // TODO : Необходимо считать очередной символ из _localStream
            // throw new NotImplementedException();
            LetterStats nx = new LetterStats();
           // nx.Letter= this._localStream
            StreamReader sr = new StreamReader(_localStream);
         
          //  while (sr.Peek() >= 0)
                

            // nx.Letter = (string)sr.Read();

            // int        Console.Write((char)sr.Read()); f = sr.Read()
           //Console.Write((char)sr.Peek());
            Console.Write((char)sr.Peek());
                //Console.Write(555);

            
           //IsEof = true;
            return (char)sr.Peek();
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (_localStream == null)
            {
                IsEof = true;
                return;
            }

            _localStream.Position = 0;
            IsEof = false;
        }
    }
}
