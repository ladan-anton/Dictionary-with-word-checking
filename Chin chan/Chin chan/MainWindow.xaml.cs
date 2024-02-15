using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace RussianToChinese
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> wordsDictionary = new Dictionary<string, string>();
        private string dictionaryFilePath = "dictionary.bin";
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            LoadDictionaryFromFile();
        }

        private void LoadDictionaryFromFile()
        {
            if (File.Exists(dictionaryFilePath))
            {
                using (Stream stream = File.Open(dictionaryFilePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    wordsDictionary = (Dictionary<string, string>)formatter.Deserialize(stream);
                }
            }
            else
            {
                // Создаем новый пустой словарь, если файл не существует
                wordsDictionary = new Dictionary<string, string>();
            }
        }

        private void SaveDictionaryToFile()
        {
            using (Stream stream = File.Open(dictionaryFilePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, wordsDictionary);
            }
        }

        private void RandomWordButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> russianWords = new List<string>(wordsDictionary.Keys);

            if (russianWords.Count > 0)
            {
                string randomRussianWord = russianWords[random.Next(russianWords.Count)];
                russianWordTextBlock.Text = randomRussianWord;
            }
            else
            {
                MessageBox.Show("Словарь пуст.");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            SaveDictionaryToFile(); // Сохраняем словарь при закрытии окна приложения
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> russianWords = new List<string>(wordsDictionary.Keys);

            if (russianWords.Count > 0)
            {
                
                string randomRussianWord = russianWords[random.Next(russianWords.Count)];
                string expectedRussianWord = randomRussianWord;
                string expectedChineseChar = wordsDictionary[randomRussianWord];

                russianWordTextBlock.Text = expectedRussianWord;

                string enteredChineseChar = chineseCharTextBox.Text;

                if (expectedChineseChar == enteredChineseChar)
                {
                    MessageBox.Show("Правильно!");
                }
                else
                {
                    MessageBox.Show("Неправильно. Правильный ответ: " + expectedChineseChar);
                }
            }
            else
            {
                MessageBox.Show("Словарь пуст.");
            }
        }

        private void AddWordButton_Click(object sender, RoutedEventArgs e)
        {
            string russianWord = russianWordTextBox.Text;
            string chineseChar = chineseCharTextBox.Text;

            if (!wordsDictionary.ContainsKey(russianWord))
            {
                wordsDictionary.Add(russianWord, chineseChar);
                MessageBox.Show("Слово добавлено в словарь.");
            }
            else
            {
                MessageBox.Show("Это слово уже есть в словаре.");
            }
        }
    }
}