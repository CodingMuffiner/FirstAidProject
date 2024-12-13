using System;
using System.IO;

namespace FirstAidProject
{
    public class Condition
    {
        protected string conditionPath;

        public string ConditionPath
        {
            get { return conditionPath; }
        }

        public Condition(string path)
        {
            conditionPath = path;
        }
    }

    public class MildCondition : Condition
    {
        public MildCondition(string path) : base(path) { }

        public void DisplayDetails()
        {
            // Output details for mild conditions
        }
    }

    public class ModerateCondition : Condition
    {
        public ModerateCondition(string path) : base(path) { }

        public void DisplayDetails()
        {
            // Output details for moderate conditions
        }
    }

    public class SevereCondition : Condition
    {
        public SevereCondition(string path) : base(path) { }

        public void DisplayDetails()
        {
            // Output details for severe conditions
        }
    }

    public class FirstAidToolkit
    {
        private string conditionsFolderPath = @"C:\\Code\\c#\\FirstAidProject\\Conditions";
        private string moderateConditionsFolderPath = @"C:\\Code\\c#\\FirstAidProject\\ModerateConditions";
        private string severeConditionsFolderPath = @"C:\\Code\\c#\\FirstAidProject\\SevereConditions";
        private string contactsFolderPath = @"C:\\Code\\c#\\FirstAidProject\\Infos";

        public void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the First Aid Toolkit.");
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. View All Conditions");
                Console.WriteLine("2. Contacts");
                Console.WriteLine("3. Emergency Hotline");
                Console.WriteLine("4. Suggest an Improvement"); // New option
                Console.WriteLine("5. Admin Access");
                Console.WriteLine("6. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllConditions();
                        break;
                    case "2":
                        ViewContacts();
                        break;
                    case "3":
                        ShowEmergencyHotline();
                        break;
                    case "4": // Handle the new option
                        SuggestImprovement();
                        break;
                    case "5":
                        AdminAccess adminAccess = new AdminAccess();
                        Console.Write("Enter Admin Code: ");
                        string adminCode = Console.ReadLine();
                        if (adminCode == "2580")
                        {
                            adminAccess.AdminMenu();
                        }
                        else
                        {
                            Console.WriteLine("Invalid admin code. Returning to main menu.");
                        }
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }


        private void ShowEmergencyHotline()
        {
            Console.Clear();
            Console.WriteLine("Emergency Hotlines for Cebu, Philippines:\r\n");
            Console.WriteLine("Emergency 911: General emergency services.");
            Console.WriteLine("Philippine Red Cross - Cebu Chapter: (+63) 32-253-9793");
            Console.WriteLine("Cebu City Fire Department: (+63) 32-256-0541");
            Console.WriteLine("Cebu City Police Department: (+63) 32-256-0751");
            Console.WriteLine("Cebu City Medical Center: (+63) 32-255-7141");
            Console.WriteLine("\nPress any key to go back to the menu.");
            Console.ReadKey();
        }

        public void ViewAllConditions()
        {
            Console.Clear();
            Console.WriteLine("Available Conditions:");
            string[] conditionDirectories = Directory.GetDirectories(conditionsFolderPath);
            for (int i = 0; i < conditionDirectories.Length; i++)
            {
                string conditionName = Path.GetFileName(conditionDirectories[i]);
                Console.WriteLine($"{i + 1}. {conditionName}");
            }

            Console.WriteLine("\nSelect the number of the condition you want to check:");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= conditionDirectories.Length)
            {
                string conditionName = Path.GetFileName(conditionDirectories[selectedIndex - 1]);

                Console.WriteLine($"You selected '{conditionName}'. Please choose the severity level:");
                Console.WriteLine("1. Mild");
                Console.WriteLine("2. Moderate");
                Console.WriteLine("3. Severe");
                Console.WriteLine("4. Back to Main Menu");

                string severityChoice = Console.ReadLine();
                string selectedConditionPath = string.Empty;

                Condition selectedCondition = null;

                switch (severityChoice)
                {
                    case "1":
                        selectedConditionPath = Path.Combine(conditionsFolderPath, conditionName);
                        selectedCondition = new MildCondition(selectedConditionPath);
                        break;
                    case "2":
                        selectedConditionPath = Path.Combine(moderateConditionsFolderPath, conditionName);
                        selectedCondition = new ModerateCondition(selectedConditionPath);
                        break;
                    case "3":
                        selectedConditionPath = Path.Combine(severeConditionsFolderPath, conditionName);
                        selectedCondition = new SevereCondition(selectedConditionPath);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        return;
                }

                // Ensure that the selectedConditionPath is valid before proceeding
                if (Directory.Exists(selectedConditionPath))
                {
                    ViewConditionDetails(selectedCondition.ConditionPath);
                }
                else
                {
                    Console.WriteLine($"No details found for '{conditionName}' with the selected severity.");
                }
            }
            else
            {
                Console.WriteLine("Invalid selection. Returning to the main menu.");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        public void ViewConditionDetails(string conditionPath)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("You can view the following information:");
                Console.WriteLine("1. Description");
                Console.WriteLine("2. Recommendations");
                Console.WriteLine("3. Treatment Steps");
                Console.WriteLine("4. Back to Main Menu");

                string detailChoice = Console.ReadLine();

                switch (detailChoice)
                {
                    case "1":
                        string descriptionPath = Path.Combine(conditionPath, "Description.txt");
                        Console.WriteLine(File.ReadAllText(descriptionPath));
                        break;
                    case "2":
                        string recommendationsPath = Path.Combine(conditionPath, "Recommendation.txt");
                        Console.WriteLine(File.ReadAllText(recommendationsPath));
                        break;
                    case "3":
                        string stepsPath = Path.Combine(conditionPath, "Steps.txt");
                        Console.WriteLine(File.ReadAllText(stepsPath));
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue.");
                Console.ReadKey();
            }
        }

        public void SuggestImprovement()
        {
            string suggestionDirectory = @"C:\Code\c#\FirstAidProject\User\Suggestions";
            if (!Directory.Exists(suggestionDirectory))
                Directory.CreateDirectory(suggestionDirectory);

            Console.Clear();
            Console.WriteLine("Suggest an Improvement");
            Console.WriteLine("-----------------------");
            Console.WriteLine("Please enter your suggestion for the First Aid Toolkit:");

            string suggestion = Console.ReadLine();
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Get the next suggestion number
            int suggestionNumber = Directory.GetFiles(suggestionDirectory).Length + 1;
            string fileName = Path.Combine(suggestionDirectory, $"Suggestions{suggestionNumber}.txt");

            // Save the suggestion with a timestamp
            File.WriteAllText(fileName, $"Timestamp: {timestamp}\nSuggestion: {suggestion}");

            Console.WriteLine("\nThank you for your suggestion! It has been recorded.");
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
        }

        public void ViewContacts()
        {
            Console.Clear();
            Console.WriteLine("Contacts and Specialists:");
            Console.WriteLine("1. View All Contacts");
            Console.WriteLine("2. Search Contacts");
            Console.WriteLine("3. Back to Main Menu");

            string contactChoice = Console.ReadLine();

            switch (contactChoice)
            {
                case "1":
                    ViewAllContacts();
                    break;
                case "2":
                    SearchContacts();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        public void ViewAllContacts()
        {
            Console.Clear();
            Console.WriteLine("All available contacts:");

            string[] contactFiles = Directory.GetFiles(contactsFolderPath, "*.txt");
            int index = 1;

            foreach (var file in contactFiles)
            {
                string contactName = Path.GetFileNameWithoutExtension(file);
                Console.WriteLine($"{index}. {contactName}");
                index++;
            }

            Console.WriteLine("\nEnter number of contact to view or press Enter to go back:");
            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                ViewContacts();
                return;
            }

            if (int.TryParse(input, out int contactIndex) && contactIndex > 0 && contactIndex <= contactFiles.Length)
            {
                string selectedContact = contactFiles[contactIndex - 1];
                Console.Clear();
                Console.WriteLine(File.ReadAllText(selectedContact));
            }
            else
            {
                Console.WriteLine("Invalid selection. Returning to the Contacts menu.");
            }

            Console.WriteLine("\nPress any key to return to the Contacts menu.");
            Console.ReadKey();
            ViewContacts();
        }

        private void SearchContacts()
        {
            Console.Clear();
            Console.WriteLine("Enter the name or specialty to search:");

            string searchQuery = Console.ReadLine().ToLower();
            string[] foundContacts = new string[0];

            foreach (var contact in Directory.GetFiles(contactsFolderPath, "*.txt"))
            {
                if (Path.GetFileName(contact).ToLower().Contains(searchQuery))
                {
                    Array.Resize(ref foundContacts, foundContacts.Length + 1);
                    foundContacts[foundContacts.Length - 1] = Path.GetFileNameWithoutExtension(contact);
                }
            }

            if (foundContacts.Length > 0)
            {
                Console.WriteLine("\nSearch results:");
                for (int i = 0; i < foundContacts.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {foundContacts[i]}");
                }

                Console.WriteLine("\nEnter the number to view or press Enter to search again, or type 'exit' to return to contacts.");
                string choice = Console.ReadLine();

                if (string.IsNullOrEmpty(choice))
                {
                    SearchContacts();
                }
                else if (choice.ToLower() == "exit")
                {
                    return;
                }
                else if (int.TryParse(choice, out int selection) && selection > 0 && selection <= foundContacts.Length)
                {
                    string selectedContact = foundContacts[selection - 1];
                    string filePath = Path.Combine(contactsFolderPath, selectedContact + ".txt");
                    Console.Clear();
                    Console.WriteLine(File.ReadAllText(filePath));
                }
                else
                {
                    Console.WriteLine("Invalid selection. Please try again.");
                }
            }
            else
            {
                Console.WriteLine($"No contacts found for '{searchQuery}'.");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
            ViewContacts();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            FirstAidToolkit toolkit = new FirstAidToolkit();
            toolkit.MainMenu();
        }
    }
}