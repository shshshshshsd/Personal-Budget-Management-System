using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Transaction
    {
        public DateTime Date { get; set; }
       
        public decimal Amount { get; set; }
        public string Category { get; set; }
    }
    internal class TransactionManager
    {
        // إضافة معاملة جديدة إلى ملف CSV
        public void AddTransactionToFile(DateTime date ,decimal amount, string category)
        {
            var csv = new StringBuilder();

            // إضافة رأس الأعمدة إذا كان الملف فارغًا
            if (!File.Exists("transactions.csv"))
            {
                csv.AppendLine("Date,Amount,Category"); // إضافة رأس الأعمدة
            }

            // إضافة بيانات المعاملة
            csv.AppendLine($"{date.ToString("dd/MM/yyyy")},{amount},{category}");

            // حفظ البيانات في الملف
            File.AppendAllText("transactions.csv", csv.ToString());

            // تحقق من وجود الملف وتسجيل رسالة
            if (File.Exists("transactions.csv"))
            {
                MessageBox.Show("File created/updated successfully at: " + Path.GetFullPath("transactions.csv"));
            }
            else
            {
                MessageBox.Show("There was an error creating the file.");
            }
        }

        // قراءة المعاملات من ملف CSV
        public List<Transaction> ReadTransactionsFromFile()
        {
            var transactions = new List<Transaction>();

            if (File.Exists("transactions.csv"))
            {
                string[] lines = File.ReadAllLines("transactions.csv");

                // نبدأ من 1 لتخطي رأس الأعمدة
                for (int i = 1; i < lines.Length; i++)
                {
                    var parts = lines[i].Split(',');
                    if (parts.Length == 3)
                    {
                        DateTime date;
                        decimal amount;

                        if (DateTime.TryParseExact(parts[0], "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out date)
                        && decimal.TryParse(parts[1], out amount))
                        {
                            transactions.Add(new Transaction
                            {
                                Date = date,
                                Amount = amount,
                                Category = parts[2]
                            });
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("لا يوجد ملف معاملات حتى الآن.");
            }

            return transactions;
        }
    }
   
}
