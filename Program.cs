using System.Collections.Generic;

namespace Task03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var contacts = new List<Contact>();
            var view = new ContactView();
            var controller = new ContactController(contacts, view);

            controller.Run();
        }
    }
}