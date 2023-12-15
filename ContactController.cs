using System;
using System.Collections.Generic;
using System.Linq;

namespace Task03
{
    public class ContactController
    {
        private List<Contact> contacts;
        private ContactView view;
        private JsonDataManager jsonDataManager;
        private XmlDataManager xmlDataManager;
        private SQLiteDataManager sqliteDataManager;

        public ContactController(List<Contact> contacts, ContactView view)
        {
            this.contacts = contacts;
            this.view = view;
            this.jsonDataManager = new JsonDataManager();
            this.xmlDataManager = new XmlDataManager();
            this.sqliteDataManager = new SQLiteDataManager();
        }

        public void Run()
        {
            Console.WriteLine("Enter the number of action and press [Enter]. Then follow instructions.");

            while (true)
            {
                view.DisplayMenu();
                int choice = view.GetMenuChoice();

                switch (choice)
                {
                    case 1:
                        view.ShowContacts(contacts);
                        break;
                    case 2:
                        SearchContacts();
                        break;
                    case 3:
                        AddNewContact();
                        break;
                    case 4:
                        SaveLoadOptions();
                        break;
                    case 5:
                        Console.WriteLine("Exiting the program. Goodbye!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid menu number.");
                        break;
                }
            }
        }

        private void SearchContacts()
        {
            string searchField = view.GetSearchField();
            string searchTerm = view.GetSearchTerm();

            Console.WriteLine("Searching...");

            List<Contact> searchResults = new List<Contact>();

            switch (searchField)
            {
                case "1":
                    searchResults = contacts.Where(c => c.Name.Contains(searchTerm)).ToList();
                    break;
                case "2":
                    searchResults = contacts.Where(c => c.Surname.Contains(searchTerm)).ToList();
                    break;
                case "3":
                    searchResults = contacts.Where(c => (c.Name + " " + c.Surname).Contains(searchTerm)).ToList();
                    break;
                case "4":
                    searchResults = contacts.Where(c => c.Phone.Contains(searchTerm)).ToList();
                    break;
                case "5":
                    searchResults = contacts.Where(c => c.Email.Contains(searchTerm)).ToList();
                    break;
                default:
                    Console.WriteLine("Invalid search choice.");
                    return;
            }

            view.ShowSearchResults(searchResults);
        }

        private void AddNewContact()
        {
            Contact newContact = view.GetNewContactInfo();
            contacts.Add(newContact);
            Console.WriteLine("Contact added.");
        }



        private void SaveLoadOptions()
        {
            int saveLoadChoice = view.GetSaveLoadChoice();

            switch (saveLoadChoice)
            {
                case 1:
                    jsonDataManager.SaveToFile("contacts.json", contacts);
                    Console.WriteLine("Contacts saved to JSON file.");
                    break;
                case 2:
                    contacts = jsonDataManager.LoadFromFile<Contact>("contacts.json");
                    Console.WriteLine("Contacts loaded from JSON file.");
                    break;
                case 3:
                    xmlDataManager.SaveToFile("contacts.xml", contacts);
                    Console.WriteLine("Contacts saved to XML file.");
                    break;
                case 4:
                    contacts = xmlDataManager.LoadFromFile<Contact>("contacts.xml");
                    Console.WriteLine("Contacts loaded from XML file.");
                    break;
                case 5:
                    sqliteDataManager.SaveToDatabase("contacts.db", contacts);
                    Console.WriteLine("Contacts saved to SQLite database.");
                    break;
                case 6:
                    contacts = sqliteDataManager.LoadFromDatabase<Contact>("contacts.db");
                    Console.WriteLine("Contacts loaded from SQLite database.");
                    break;
                case 7:
                    Console.WriteLine("Exiting the program. Goodbye!");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option number.");
                    break;
            }
        }
    }
}