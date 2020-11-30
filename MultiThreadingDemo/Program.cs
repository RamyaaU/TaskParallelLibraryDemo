using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskParallelLibraryDemo
{
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            Console.WriteLine("Employee Payroll Using threads!");
            //retrieve from gutenberg.org
            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/54700/54700-0.txt");

            #region ParallelTasks

            Parallel.Invoke(() =>
            {
                Console.WriteLine("Begin first task...");
                GetLongestWord(words);
            },
            () =>
                      {
                          Console.WriteLine("Begin second task...");
                          GetMostCommomWords(words);
                      }, //close second aaction

                        () =>
                        {
                            Console.WriteLine("Begin third task...");
                            GetCountForWord(words, "sleep");
                        } //close third action
                     ); //close parallel.invoke
            #endregion
        }

        /// <summary>
        /// Gets the count for word.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <param name="term">The term.</param>
        private static void GetCountForWord(string[] words, string term)
        {
            var findWord = from word in words
                           where word.ToUpper().Contains(term.ToUpper())
                           select word;

            Console.WriteLine($@"Task 3 The word  ""{term}"" occurs{findWord.Count()} times.");
        }

        /// <summary>
        /// Gets the most commom words.
        /// </summary>
        /// <param name="words">The words.</param>
        private static void GetMostCommomWords(string[] words)
        {
            var FrequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into g
                                 orderby g.Count() descending
                                 select g.Key;

            var commonwords = FrequencyOrder.Take(10);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Task 2 - The most common words are: ");
            foreach (var v in commonwords)
            {
                sb.AppendLine(" " + v);

            }
            Console.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Gets the longest word.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <returns></returns>
        private static string GetLongestWord(string[] words)
        {
            var longestWord = from w in words
                              orderby w.Length descending
                              select w.First();

            Console.WriteLine($"Task 1 - The longest word is {longestWord}.");
            return (string)longestWord;
        }

        /// <summary>
        /// Creates the word array.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        static string[] CreateWordArray(string url)
        {
            Console.WriteLine($"Retrieving from {url}");

            //download web page
            string blog = new WebClient().DownloadString(url);

            //separate string into an array of words removing some punctuation
            return blog.Split(
                new char[] { ',', '.', ';', ':', '_', '-', '/' },
                StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
    
