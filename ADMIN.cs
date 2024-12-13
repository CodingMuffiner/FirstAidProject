using System;
using System.Collections.Generic;
using System.IO;

public class AdminAccess
{
    private string conditionsFolderPath = @"C:\\Code\\c#\\FirstAidProject\\Conditions";
    private string moderateConditionsFolderPath = @"C:\\Code\\c#\\FirstAidProject\\ModerateConditions";
    private string severeConditionsFolderPath = @"C:\\Code\\c#\\FirstAidProject\\SevereConditions";
    private string contactsFolderPath = @"C:\\Code\\c#\\FirstAidProject\\Infos";
    private string suggestionsFolderPath = @"C:\\Code\\c#\\FirstAidProject\\User\\Suggestions";

    public void AdminMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Admin Access Menu:");
            Console.WriteLine("1. Add Condition");
            Console.WriteLine("2. Delete Condition");
            Console.WriteLine("3. Update Condition");
            Console.WriteLine("4. Add Contact");
            Console.WriteLine("5. Delete Contact");
            Console.WriteLine("6. Update Contact");
            Console.WriteLine("7. View Suggestions");
            Console.WriteLine("8. Back to Main Menu");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddCondition();
                    break;
                case "2":
                    DeleteCondition();
                    break;
                case "3":
                    UpdateCondition();
                    break;
                case "4":
                    AddContact();
                    break;
                case "5":
                    DeleteContact();
                    break;
                case "6":
                    UpdateContact();
                    break;
                case "7":
                    ViewSuggestions();
                    break;
                case "8":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    public void AddCondition()
    {
        Console.Clear();
        Console.Write("Enter the condition name: ");
        string conditionName = Console.ReadLine().Trim();
        if (string.IsNullOrEmpty(conditionName))
        {
            Console.WriteLine("Condition name cannot be empty.");
            return;
        }

        // Ensure that the severity folders exist
        EnsureFolderExists(conditionsFolderPath);
        EnsureFolderExists(moderateConditionsFolderPath);
        EnsureFolderExists(severeConditionsFolderPath);

        // Create condition folder inside each severity folder
        string mildFolderPath = Path.Combine(conditionsFolderPath, conditionName);
        string moderateFolderPath = Path.Combine(moderateConditionsFolderPath, conditionName);
        string severeFolderPath = Path.Combine(severeConditionsFolderPath, conditionName);

        // Create directories for each severity if they don't exist
        Directory.CreateDirectory(mildFolderPath);
        Directory.CreateDirectory(moderateFolderPath);
        Directory.CreateDirectory(severeFolderPath);

        // Get description, recommendation, and steps for each severity
        string description = GetInput($"Enter description for mild {conditionName}: ");
        string recommendation = GetInput($"Enter recommendation for mild {conditionName}: ");
        string steps = GetInput($"Enter steps for mild {conditionName}: ");
        SaveConditionDetails(mildFolderPath, description, recommendation, steps);

        description = GetInput($"Enter description for moderate {conditionName}: ");
        recommendation = GetInput($"Enter recommendation for moderate {conditionName}: ");
        steps = GetInput($"Enter steps for moderate {conditionName}: ");
        SaveConditionDetails(moderateFolderPath, description, recommendation, steps);

        description = GetInput($"Enter description for severe {conditionName}: ");
        recommendation = GetInput($"Enter recommendation for severe {conditionName}: ");
        steps = GetInput($"Enter steps for severe {conditionName}: ");
        SaveConditionDetails(severeFolderPath, description, recommendation, steps);

        Console.WriteLine($"Condition '{conditionName}' added successfully.");
    }

    private string GetInput(string prompt)
    {
        string input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("This field cannot be empty.");
            }
        } while (string.IsNullOrEmpty(input));

        return input;
    }

    public void DeleteCondition()
    {
        Console.Clear();
        Console.WriteLine("Available Mild Conditions:");

        // Fetch all conditions from the mild conditions folder
        List<string> mildConditions = GetConditionsFromFolder(conditionsFolderPath);

        if (mildConditions.Count == 0)
        {
            Console.WriteLine(" No mild conditions available for deletion.");
            Console.WriteLine("Press any key to return to the admin menu.");
            Console.ReadKey();
            return;
        }

        // Display the mild conditions
        for (int i = 0; i < mildConditions.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {mildConditions[i]}");
        }

        Console.WriteLine("\nEnter the number of the condition to delete or press Enter to cancel:");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            return; // User canceled the operation
        }

        if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= mildConditions.Count)
        {
            string conditionToDelete = mildConditions[selectedIndex - 1];

            // Delete the condition from all severity folders
            DeleteConditionFromSeverityFolder(conditionsFolderPath, conditionToDelete); // Mild
            DeleteConditionFromSeverityFolder(moderateConditionsFolderPath, conditionToDelete); // Moderate
            DeleteConditionFromSeverityFolder(severeConditionsFolderPath, conditionToDelete); // Severe

            Console.WriteLine($"Condition '{conditionToDelete}' deleted from all severity levels.");
        }
        else
        {
            Console.WriteLine("Invalid selection. Returning to the admin menu.");
        }

        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    private void EnsureFolderExists(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    private List<string> GetConditionsFromFolder(string folderPath)
    {
        List<string> conditions = new List<string>();

        if (Directory.Exists(folderPath))
        {
            foreach (var dir in Directory.GetDirectories(folderPath))
            {
                conditions.Add(Path.GetFileName(dir));
            }
        }

        return conditions;
    }

    private void DeleteConditionFromSeverityFolder(string severityFolderPath, string conditionName)
    {
        string conditionFolderPath = Path.Combine(severityFolderPath, conditionName);
        if (Directory.Exists(conditionFolderPath))
        {
            Directory.Delete(conditionFolderPath, true); // true to delete recursively
            Console.WriteLine($"{conditionName} deleted from {severityFolderPath}.");
        }
        else
        {
            Console.WriteLine($"Condition {conditionName} not found in {severityFolderPath}.");
        }
    }

    private void SaveConditionDetails(string folderPath, string description, string recommendation, string steps)
    {
        File.WriteAllText(Path.Combine(folderPath, "Description.txt"), description);
        File.WriteAllText(Path.Combine(folderPath, "Recommendation.txt"), recommendation);
        File.WriteAllText(Path.Combine(folderPath, "Steps.txt"), steps);
    }

    public void UpdateCondition()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Enter the name of the condition you want to update (or press Enter to see all conditions):");
            string conditionName = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(conditionName))
            {
                Console.Clear();
                Console.WriteLine("Available Conditions (Mild):");
                var conditions = GetConditionsFromFolder(conditionsFolderPath);

                if (conditions.Count == 0)
                {
                    Console.WriteLine("No conditions found.");
                    Console.WriteLine("\nPress any key to return to the admin menu.");
                    Console.ReadKey();
                    return;
                }

                for (int i = 0; i < conditions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {conditions[i]}");
                }

                Console.WriteLine("\nEnter the number of the condition you want to update:");
                string choice = Console.ReadLine();
                if (int.TryParse(choice, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= conditions.Count)
                {
                    conditionName = conditions[selectedIndex - 1];
                }
                else
                {
                    Console.WriteLine("Invalid selection. Returning to Admin Menu.");
                    return;
                }
            }

            // Update condition in all severity folders
            UpdateConditionInSeverity("Mild", conditionsFolderPath, conditionName);
            UpdateConditionInSeverity("Moderate", moderateConditionsFolderPath, conditionName);
            UpdateConditionInSeverity("Severe", severeConditionsFolderPath, conditionName);

            Console.WriteLine("\nCondition updated successfully. Press any key to continue.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while updating the condition: {ex.Message}");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }

    private void UpdateConditionInSeverity(string severity, string folderPath, string conditionName)
    {
        string conditionFolderPath = Path.Combine(folderPath, conditionName);

        if (!Directory.Exists(conditionFolderPath))
        {
            Console.WriteLine($"{severity} condition '{conditionName}' does not exist.");
            return;
        }

        Console.WriteLine($"\nUpdating {severity} condition: {conditionName}");

        // Update Description
        string descriptionFilePath = Path.Combine(conditionFolderPath, "Description.txt");
        Console.Write("Enter new description (leave blank to keep current): ");
        string newDescription = Console.ReadLine().Trim();
        if (!string.IsNullOrEmpty(newDescription))
        {
            File.WriteAllText(descriptionFilePath, newDescription);
        }

        // Update Recommendation
        string recommendationFilePath = Path.Combine(conditionFolderPath, "Recommendation.txt");
        Console.Write("Enter new recommendation (leave blank to keep current): ");
        string newRecommendation = Console.ReadLine().Trim();
        if (!string.IsNullOrEmpty(newRecommendation))
        {
            File.WriteAllText(recommendationFilePath, newRecommendation);
        }

        // Update Steps
        string stepsFilePath = Path.Combine(conditionFolderPath, "Steps.txt");
        Console.Write("Enter new steps (leave blank to keep current): ");
        string newSteps = Console.ReadLine().Trim();
        if (!string.IsNullOrEmpty(newSteps))
        {
            File.WriteAllText(stepsFilePath, newSteps);
        }

        Console.WriteLine($"Updated {severity} condition '{conditionName}' successfully.");
    }

    public void AddContact()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Enter the following details to add a contact:");

            Console.Write("Add name: ");
            string name = Console.ReadLine();

            Console.Write("Specialty: ");
            string specialty = Console.ReadLine();

            Console.Write("Contact number: ");
            string contactNumber = Console.ReadLine();

            string fileName = $"{name} - {specialty}.txt";
            string filePath = Path.Combine(contactsFolderPath, fileName);

            File.WriteAllText(filePath, contactNumber);

            Console.WriteLine($"\nContact {name} added successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while adding the contact: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to return to the admin menu.");
        Console.ReadKey();
    }

    public void DeleteContact()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Enter the name or specialty of the contact to delete:");

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
                Console.WriteLine("\nDelete results:");
                for (int i = 0; i < foundContacts.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {foundContacts[i]}");
                }

                Console.WriteLine("\nEnter the number to delete or press Enter to cancel.");
                string choice = Console.ReadLine();

                if (string.IsNullOrEmpty(choice))
                {
                    return;
                }
                else if (int.TryParse(choice, out int selection) && selection > 0 && selection <= foundContacts.Length)
                {
                    string selectedContact = foundContacts[selection - 1];
                    string filePath = Path.Combine(contactsFolderPath, selectedContact + ".txt");

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        Console.WriteLine($"Contact {selectedContact} deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Contact not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }
            else
            {
                Console.WriteLine($"No contacts found for '{searchQuery}'.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting the contact: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    public void UpdateContact()
    {
        try
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

            Console.WriteLine("\nEnter the number of the contact to update or press Enter to cancel:");
            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            if (int.TryParse(input, out int contactIndex) && contactIndex > 0 && contactIndex <= contactFiles.Length)
            {
                string selectedContactFile = contactFiles[contactIndex - 1];
                string oldContactName = Path.GetFileNameWithoutExtension(selectedContactFile);

                Console.Write("New Name (leave blank to keep the same): ");
                string newName = Console.ReadLine();
                newName = string.IsNullOrEmpty(newName) ? oldContactName.Split(" - ")[0] : newName;

                Console.Write("New Specialty (leave blank to keep the same): ");
                string newSpecialty = Console.ReadLine();
                newSpecialty = string.IsNullOrEmpty(newSpecialty) ? oldContactName.Split(" - ")[1] : newSpecialty;

                Console.Write("New Contact Number (leave blank to keep the same): ");
                string newContactNumber = Console.ReadLine();
                newContactNumber = string.IsNullOrEmpty(newContactNumber) ? File.ReadAllText(selectedContactFile) : newContactNumber;

                string newFileName = $"{newName} - {newSpecialty}.txt";
                string newFilePath = Path.Combine(contactsFolderPath, newFileName);

                if (newFilePath != selectedContactFile)
                {
                    File.Move(selectedContactFile, newFilePath);
                }

                File.WriteAllText(newFilePath, newContactNumber);

                Console.WriteLine($"\nContact '{oldContactName}' updated successfully!");
            }
            else
            {
                Console.WriteLine("Invalid selection. Returning to Contacts menu.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while updating the contact: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    public void ViewSuggestions()
    {
        EnsureFolderExists(suggestionsFolderPath);

        Console.Clear();
        Console.WriteLine("User  Suggestions:");

        string[] suggestionFiles = Directory.GetFiles(suggestionsFolderPath, "Suggestions*.txt");

        if (suggestionFiles.Length == 0)
        {
            Console.WriteLine("No suggestions available.");
            Console.WriteLine("Press any key to return to the admin menu.");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < suggestionFiles.Length; i++)
        {
            string suggestionContent = File.ReadAllText(suggestionFiles[i]);
            Console.WriteLine($"\nSuggestion {i + 1}:\n{suggestionContent}");
            Console.WriteLine("======================================");
        }

        Console.WriteLine("\nOptions:");
        Console.WriteLine("1. Delete a Suggestion");
        Console.WriteLine("2. Back to Admin Menu");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            DeleteSuggestion(suggestionFiles);
        }
    }

    public void DeleteSuggestion(string[] suggestionFiles)
    {
        Console.WriteLine("\nEnter the number of the suggestion to delete:");
        string input = Console.ReadLine();

        if (int.TryParse(input, out int suggestionIndex) && suggestionIndex > 0 && suggestionIndex <= suggestionFiles.Length)
        {
            string fileToDelete = suggestionFiles[suggestionIndex - 1];
            File.Delete(fileToDelete);
            Console.WriteLine($"Suggestion {suggestionIndex} deleted successfully.");
        }
        else
        {
            Console.WriteLine("Invalid input. Returning to suggestions menu.");
        }

        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }
}