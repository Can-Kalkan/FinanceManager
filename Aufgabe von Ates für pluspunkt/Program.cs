using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        FinancialManager financialManager = new FinancialManager();
        // der code ist schlecht
        while (true)
        {
            Console.WriteLine("Hauptmenü:");
            Console.WriteLine("1. Transaktion hinzufügen");
            Console.WriteLine("2. Kategorien verwalten");
            Console.WriteLine("3. Budgets festlegen");
            Console.WriteLine("4. Finanzbericht generieren");
            Console.WriteLine("5. Suche und Filter");
            Console.WriteLine("6. Beenden");

            Console.Write("Ihre Auswahl: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    financialManager.AddTransaction();
                    break;
                case "2":
                    financialManager.ManageCategories();
                    break;
                case "3":
                    financialManager.SetBudgets();
                    break;
                case "4":
                    financialManager.GenerateFinancialReport();
                    break;
                case "5":
                    financialManager.SearchAndFilter();
                    break;
                case "6":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Ungültige Auswahl. Bitte versuchen Sie es erneut.");
                    break;
            }
        }
    }
}

class FinancialManager
{
    private List<Transaction> transactions = new List<Transaction>();
    private List<Category> categories = new List<Category>();
    private Dictionary<string, decimal> budgets = new Dictionary<string, decimal>();

    public void AddTransaction()
    {
        Console.Write("Datum (YYYY-MM-DD): ");
        string date = Console.ReadLine();

        Console.Write("Betrag: ");
        decimal amount = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Beschreibung: ");
        string description = Console.ReadLine();

        Console.Write("Kategorie: ");
        string category = Console.ReadLine();

        transactions.Add(new Transaction(date, amount, description, category));
        Console.WriteLine("Transaktion hinzugefügt.\n");
    }

    public void ManageCategories()
    {
        Console.WriteLine("Kategorien verwalten:");
        Console.WriteLine("1. Kategorie hinzufügen");
        Console.WriteLine("2. Kategorien anzeigen");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Neue Kategorie: ");
                string newCategory = Console.ReadLine();
                categories.Add(new Category(newCategory));
                Console.WriteLine("Kategorie hinzugefügt.\n");
                break;
            case "2":
                Console.WriteLine("Verfügbare Kategorien:");
                foreach (var category in categories)
                {
                    Console.WriteLine(category.Name);
                }
                Console.WriteLine();
                break;
            default:
                Console.WriteLine("Ungültige Auswahl. Bitte versuchen Sie es erneut.\n");
                break;
        }
    }

    public void SetBudgets()
    {
        Console.WriteLine("Budgets festlegen:");

        foreach (var category in categories)
        {
            Console.Write($"Budget für {category.Name}: ");
            decimal budget = Convert.ToDecimal(Console.ReadLine());
            budgets[category.Name] = budget;
        }

        Console.WriteLine("Budgets festgelegt.\n");
    }

    public void GenerateFinancialReport()
    {
        Console.WriteLine("Finanzbericht generieren:");

        // Annahme: Die Daten sind nach Datum sortiert.
        var sortedTransactions = transactions.OrderBy(t => t.Date).ToList();

        if (sortedTransactions.Count == 0)
        {
            Console.WriteLine("Keine Transaktionen vorhanden. Bericht konnte nicht generiert werden.\n");
            return;
        }

        // Berichtsdatei erstellen
        string reportFileName = $"reports/{DateTime.Now.ToString("yyyy-MM")}-report.txt";
        using (StreamWriter writer = new StreamWriter(reportFileName))
        {
            writer.WriteLine($"Finanzbericht - {DateTime.Now.ToString("MMMM yyyy")}");
            writer.WriteLine(new string('-', 40));

            // Gesamteinnahmen und Gesamtausgaben berechnen
            decimal totalIncome = 0;
            decimal totalExpense = 0;

            foreach (var transaction in sortedTransactions)
            {
                writer.WriteLine($"{transaction.Date} - {transaction.Description}: {transaction.Amount:C}");

                if (transaction.Amount > 0)
                {
                    totalIncome += transaction.Amount;
                }
                else
                {
                    totalExpense += transaction.Amount;
                }

                // Weitere Details hinzufügen, falls erforderlich
            }

            writer.WriteLine(new string('-', 40));
            writer.WriteLine($"Gesamteinnahmen: {totalIncome:C}");
            writer.WriteLine($"Gesamtausgaben: {totalExpense:C}");
            writer.WriteLine($"Gesamtfinanzstatus: {(totalIncome + totalExpense):C}");
        }

        Console.WriteLine($"Finanzbericht wurde unter '{reportFileName}' gespeichert.\n");
    }


    public void SearchAndFilter()
    {
        Console.WriteLine("Suche und Filter:");

        Console.WriteLine("1. Suche nach Datum");
        Console.WriteLine("2. Suche nach Kategorie");
        Console.WriteLine("3. Suche nach Betrag");
        Console.WriteLine("4. Zurück zum Hauptmenü");

        Console.Write("Ihre Auswahl: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Geben Sie das Datum ein (YYYY-MM-DD): ");
                string searchDate = Console.ReadLine();

                var transactionsByDate = transactions.Where(t => t.Date == searchDate).ToList();

                DisplayFilteredTransactions(transactionsByDate);
                break;

            case "2":
                Console.Write("Geben Sie die Kategorie ein: ");
                string searchCategory = Console.ReadLine();

                var transactionsByCategory = transactions.Where(t => t.Category.ToLower() == searchCategory.ToLower()).ToList();

                DisplayFilteredTransactions(transactionsByCategory);
                break;

            case "3":
                Console.Write("Geben Sie den Betrag ein: ");
                decimal searchAmount = Convert.ToDecimal(Console.ReadLine());

                var transactionsByAmount = transactions.Where(t => t.Amount == searchAmount).ToList();

                DisplayFilteredTransactions(transactionsByAmount);
                break;

            case "4":
                return;

            default:
                Console.WriteLine("Ungültige Auswahl. Bitte versuchen Sie es erneut.\n");
                break;
        }
    }

    private void DisplayFilteredTransactions(List<Transaction> filteredTransactions)
    {
        if (filteredTransactions.Count == 0)
        {
            Console.WriteLine("Keine passenden Transaktionen gefunden.\n");
            return;
        }

        Console.WriteLine("Gefundene Transaktionen:");
        foreach (var transaction in filteredTransactions)
        {
            Console.WriteLine($"{transaction.Date} - {transaction.Description}: {transaction.Amount:C}");
        }

        Console.WriteLine();
    }

}

class Transaction
{
    public string Date { get; }
    public decimal Amount { get; }
    public string Description { get; }
    public string Category { get; }

    public Transaction(string date, decimal amount, string description, string category)
    {
        Date = date;
        Amount = amount;
        Description = description;
        Category = category;
    }
}

class Category
{
    public string Name { get; }

    public Category(string name)
    {
        Name = name;
    }
}
