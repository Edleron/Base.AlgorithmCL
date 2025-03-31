using System;

namespace BasicDelegateExample
{

    protected delegate void MyDelegate();
    protected MyDelegate myDelegateInstance;
    class Program
    {
        public void Main(string[] args)
        {
            mydelegateInstance = new MyDelegate(HiMetod);
            mydelegateInstance += HiErobosLogic;
            mydelegateInstance += HiNickLogic;
        }

        public void HiCodeLogic() {
            Console.WriteLine("Hello World!");
        }

        public void HiErobosLogic() {
            Console.WriteLine("Hello Erobos!");
        }

        public void HiNickLogic() {
            Console.WriteLine("Hello Nick!");
        }

        public void HiMetodStarted() {
           mydelegateInstance.Invoke();
        }
    }
}
