namespace Singleton
{
    class Program
    {
        static void Main(string[] args)
        {
            var customManager = CustomerManager.CreateAsSingleton();
            customManager.Save();
        }
    }

    class CustomerManager
    {
        private static CustomerManager _cManager;

        // Defensing or Safe programing;
        static object _lockObject = new object();

        private CustomerManager()
        {

        }

        public static CustomerManager CreateAsSingleton()
        {
            // return _cManager ?? (_cManager = new CustomerManager());

            // Defensing or Safe programing;
            lock (_lockObject)
            {
                if (_cManager == null)
                {
                    _cManager = new CustomerManager();
                }
            }
            return _cManager;
        }

        public void Save()
        {
            Console.WriteLine("Saing !");
            Console.ReadKey();
        }
    }
}