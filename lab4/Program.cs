using System;
using System.Collections.Generic;

namespace DesignPatternsDemo
{
    // ---------- Factory Method ----------
    abstract class Report
    {
        public abstract void Generate();
    }

    class PdfReport : Report
    {
        public override void Generate() => Console.WriteLine("PDF Report Generated");
    }

    class HtmlReport : Report
    {
        public override void Generate() => Console.WriteLine("HTML Report Generated");
    }

    class CsvReport : Report
    {
        public override void Generate() => Console.WriteLine("CSV Report Generated");
    }

    abstract class ReportFactory
    {
        public abstract Report CreateReport();
    }

    class PdfReportFactory : ReportFactory
    {
        public override Report CreateReport() => new PdfReport();
    }

    class HtmlReportFactory : ReportFactory
    {
        public override Report CreateReport() => new HtmlReport();
    }

    class CsvReportFactory : ReportFactory
    {
        public override Report CreateReport() => new CsvReport();
    }

    // ---------- Composite ----------
    abstract class MenuComponent
    {
        public string Name;
        public MenuComponent(string name) => Name = name;
        public virtual void Add(MenuComponent component) { }
        public virtual void Display(int depth = 0) => Console.WriteLine(new string('-', depth) + Name);
    }

    class MenuItem : MenuComponent
    {
        public MenuItem(string name) : base(name) { }
    }

    class MenuGroup : MenuComponent
    {
        private List<MenuComponent> _children = new();
        public MenuGroup(string name) : base(name) { }

        public override void Add(MenuComponent component) => _children.Add(component);

        public override void Display(int depth = 0)
        {
            base.Display(depth);
            foreach (var child in _children)
                child.Display(depth + 2);
        }
    }

    // ---------- Strategy ----------
    interface IDiscountStrategy
    {
        decimal ApplyDiscount(decimal price);
    }

    class SeasonalDiscount : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal price) => price * 0.8m;
    }

    class LoyaltyDiscount : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal price) => price * 0.9m;
    }

    class PromotionalDiscount : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal price) => price - 15;
    }

    class PriceCalculator
    {
        private IDiscountStrategy _discountStrategy;

        public PriceCalculator(IDiscountStrategy discountStrategy)
        {
            _discountStrategy = discountStrategy;
        }

        public decimal CalculatePrice(decimal price) => _discountStrategy.ApplyDiscount(price);
    }

    // ---------- Main ----------
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("--- Factory Method ---");
            ReportFactory factory = new PdfReportFactory();
            factory.CreateReport().Generate();

            factory = new HtmlReportFactory();
            factory.CreateReport().Generate();

            factory = new CsvReportFactory();
            factory.CreateReport().Generate();

            Console.WriteLine("\n--- Composite ---");
            var mainMenu = new MenuGroup("Main Menu");
            var fileMenu = new MenuGroup("File");
            fileMenu.Add(new MenuItem("Open"));
            fileMenu.Add(new MenuItem("Save"));
            var editMenu = new MenuGroup("Edit");
            editMenu.Add(new MenuItem("Cut"));
            editMenu.Add(new MenuItem("Paste"));
            mainMenu.Add(fileMenu);
            mainMenu.Add(editMenu);
            mainMenu.Display();

            Console.WriteLine("\n--- Strategy ---");
            var calculator = new PriceCalculator(new SeasonalDiscount());
            Console.WriteLine($"Seasonal: {calculator.CalculatePrice(100)}");

            calculator = new PriceCalculator(new LoyaltyDiscount());
            Console.WriteLine($"Loyalty: {calculator.CalculatePrice(100)}");

            calculator = new PriceCalculator(new PromotionalDiscount());
            Console.WriteLine($"Promotional: {calculator.CalculatePrice(100)}");
        }
    }
}
