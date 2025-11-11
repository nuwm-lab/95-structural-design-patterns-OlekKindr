using System;
using System.Collections.Generic;

namespace LabWork
{
    // Даний проект є шаблоном для виконання лабораторних робіт
    // з курсу "Об'єктно-орієнтоване програмування та патерни проектування"
    // Необхідно змінювати і дописувати код лише в цьому проекті
    // Відео-інструкції щодо роботи з github можна переглянути 
    // за посиланням https://www.youtube.com/@ViktorZhukovskyy/videos 

    // Цільовий інтерфейс (Target Interface)
    // Визначає загальний інтерфейс для роботи з базами даних
    public interface IDatabase
    {
        void Connect(string connectionString);
        void Disconnect();
        void ExecuteQuery(string query);
        List<string> FetchData(string query);
    }

    // Adaptee класи - різні бази даних зі своїми специфічними інтерфейсами

    // MySQL база даних
    public class MySqlDatabase
    {
        private string _connectionString;
        private bool _isConnected;

        public void OpenConnection(string connStr)
        {
            _connectionString = connStr;
            _isConnected = true;
            Console.WriteLine($"[MySQL] Відкрито з'єднання з MySQL: {connStr}");
        }

        public void CloseConnection()
        {
            _isConnected = false;
            Console.WriteLine("[MySQL] З'єднання з MySQL закрито");
        }

        public void RunSqlQuery(string sqlQuery)
        {
            if (!_isConnected)
            {
                Console.WriteLine("[MySQL] Помилка: З'єднання не встановлено");
                return;
            }
            Console.WriteLine($"[MySQL] Виконано SQL запит: {sqlQuery}");
        }

        public List<string> GetSqlResults(string sqlQuery)
        {
            if (!_isConnected)
            {
                Console.WriteLine("[MySQL] Помилка: З'єднання не встановлено");
                return new List<string>();
            }
            Console.WriteLine($"[MySQL] Отримано дані за запитом: {sqlQuery}");
            return new List<string> { "MySQL Result 1", "MySQL Result 2", "MySQL Result 3" };
        }
    }

    // PostgreSQL база даних
    public class PostgreSqlDatabase
    {
        private string _host;
        private bool _connected;

        public void EstablishConnection(string host, int port, string database, string user, string password)
        {
            _host = host;
            _connected = true;
            Console.WriteLine($"[PostgreSQL] Підключено до PostgreSQL на {host}:{port}, база: {database}");
        }

        public void TerminateConnection()
        {
            _connected = false;
            Console.WriteLine("[PostgreSQL] З'єднання з PostgreSQL завершено");
        }

        public void PerformQuery(string pgQuery)
        {
            if (!_connected)
            {
                Console.WriteLine("[PostgreSQL] Помилка: З'єднання не активне");
                return;
            }
            Console.WriteLine($"[PostgreSQL] Виконано запит: {pgQuery}");
        }

        public List<string> RetrieveData(string pgQuery)
        {
            if (!_connected)
            {
                Console.WriteLine("[PostgreSQL] Помилка: З'єднання не активне");
                return new List<string>();
            }
            Console.WriteLine($"[PostgreSQL] Завантажено дані за запитом: {pgQuery}");
            return new List<string> { "PostgreSQL Record A", "PostgreSQL Record B" };
        }
    }

    // MongoDB база даних (NoSQL)
    public class MongoDatabase
    {
        private string _connectionUri;
        private bool _isActive;

        public void InitializeConnection(string uri, string dbName, string collection)
        {
            _connectionUri = uri;
            _isActive = true;
            Console.WriteLine($"[MongoDB] Ініціалізовано підключення до MongoDB: {uri}, БД: {dbName}, Колекція: {collection}");
        }

        public void ShutdownConnection()
        {
            _isActive = false;
            Console.WriteLine("[MongoDB] Підключення до MongoDB вимкнено");
        }

        public void ExecuteOperation(string operation)
        {
            if (!_isActive)
            {
                Console.WriteLine("[MongoDB] Помилка: Підключення не активне");
                return;
            }
            Console.WriteLine($"[MongoDB] Виконано операцію: {operation}");
        }

        public List<string> FindDocuments(string filter)
        {
            if (!_isActive)
            {
                Console.WriteLine("[MongoDB] Помилка: Підключення не активне");
                return new List<string>();
            }
            Console.WriteLine($"[MongoDB] Знайдено документи за фільтром: {filter}");
            return new List<string> { "{id: 1, name: 'Document1'}", "{id: 2, name: 'Document2'}" };
        }
    }

    // Adapter класи - адаптують специфічні інтерфейси БД до загального інтерфейсу

    // Адаптер для MySQL
    public class MySqlAdapter : IDatabase
    {
        private readonly MySqlDatabase _mySqlDatabase;

        public MySqlAdapter(MySqlDatabase mySqlDatabase)
        {
            _mySqlDatabase = mySqlDatabase;
        }

        public void Connect(string connectionString)
        {
            _mySqlDatabase.OpenConnection(connectionString);
        }

        public void Disconnect()
        {
            _mySqlDatabase.CloseConnection();
        }

        public void ExecuteQuery(string query)
        {
            _mySqlDatabase.RunSqlQuery(query);
        }

        public List<string> FetchData(string query)
        {
            return _mySqlDatabase.GetSqlResults(query);
        }
    }

    // Адаптер для PostgreSQL
    public class PostgreSqlAdapter : IDatabase
    {
        private readonly PostgreSqlDatabase _postgreSqlDatabase;
        private string _connectionString;

        public PostgreSqlAdapter(PostgreSqlDatabase postgreSqlDatabase)
        {
            _postgreSqlDatabase = postgreSqlDatabase;
        }

        public void Connect(string connectionString)
        {
            _connectionString = connectionString;
            // Парсимо рядок підключення (спрощена версія)
            _postgreSqlDatabase.EstablishConnection("localhost", 5432, "mydb", "user", "password");
        }

        public void Disconnect()
        {
            _postgreSqlDatabase.TerminateConnection();
        }

        public void ExecuteQuery(string query)
        {
            _postgreSqlDatabase.PerformQuery(query);
        }

        public List<string> FetchData(string query)
        {
            return _postgreSqlDatabase.RetrieveData(query);
        }
    }

    // Адаптер для MongoDB
    public class MongoDbAdapter : IDatabase
    {
        private readonly MongoDatabase _mongoDatabase;
        private string _connectionString;

        public MongoDbAdapter(MongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public void Connect(string connectionString)
        {
            _connectionString = connectionString;
            // Парсимо рядок підключення (спрощена версія)
            _mongoDatabase.InitializeConnection(connectionString, "mydb", "mycollection");
        }

        public void Disconnect()
        {
            _mongoDatabase.ShutdownConnection();
        }

        public void ExecuteQuery(string query)
        {
            _mongoDatabase.ExecuteOperation(query);
        }

        public List<string> FetchData(string query)
        {
            return _mongoDatabase.FindDocuments(query);
        }
    }

    // Клієнтський код
    public class DatabaseClient
    {
        public void UseDatabase(IDatabase database, string connectionString)
        {
            database.Connect(connectionString);
            database.ExecuteQuery("CREATE TABLE IF NOT EXISTS users");
            List<string> data = database.FetchData("SELECT * FROM users");
            
            Console.WriteLine("Отримані дані:");
            foreach (var item in data)
            {
                Console.WriteLine($"  - {item}");
            }
            
            database.Disconnect();
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Демонстрація патерну Adapter для підключення до різних баз даних ===\n");

            DatabaseClient client = new DatabaseClient();

            // Використання MySQL через адаптер
            Console.WriteLine("--- Робота з MySQL ---");
            MySqlDatabase mySql = new MySqlDatabase();
            IDatabase mySqlAdapter = new MySqlAdapter(mySql);
            client.UseDatabase(mySqlAdapter, "Server=localhost;Database=mydb;User=root;Password=pass;");

            // Використання PostgreSQL через адаптер
            Console.WriteLine("--- Робота з PostgreSQL ---");
            PostgreSqlDatabase postgreSql = new PostgreSqlDatabase();
            IDatabase postgreSqlAdapter = new PostgreSqlAdapter(postgreSql);
            client.UseDatabase(postgreSqlAdapter, "Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=pass;");

            // Використання MongoDB через адаптер
            Console.WriteLine("--- Робота з MongoDB ---");
            MongoDatabase mongo = new MongoDatabase();
            IDatabase mongoAdapter = new MongoDbAdapter(mongo);
            client.UseDatabase(mongoAdapter, "mongodb://localhost:27017");

            Console.WriteLine("\n=== Демонстрація завершено ===");
            Console.WriteLine("\nПатерн Adapter дозволяє працювати з різними базами даних");
            Console.WriteLine("через єдиний інтерфейс IDatabase, незважаючи на їх різні API.");
        }
    }
}
