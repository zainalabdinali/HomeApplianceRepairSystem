using System;
using HomeApplianceRepairSystem.Models;
using HomeApplianceRepairSystem.Services;
using HomeApplianceRepairSystem.Enums;


namespace HomeApplianceRepairSystem
{
    internal class Program
    {
        private static readonly MaintenanceCenter center =
            new MaintenanceCenter();

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();

                Console.WriteLine("==================================");
                Console.WriteLine("   HOME APPLIANCE REPAIR SYSTEM");
                Console.WriteLine("==================================");
                Console.WriteLine("1. Add Appliance");
                Console.WriteLine("2. Display All Appliances");
                Console.WriteLine("3. Find Appliance");
                Console.WriteLine("4. Start Repair");
                Console.WriteLine("5. Complete Repair");
                Console.WriteLine("6. Display Maintenance Summary");
                Console.WriteLine("0. Exit");
                Console.WriteLine("==================================");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddAppliance();
                            break;

                        case "2":
                            center.DisplayAllAppliances();
                            Pause();
                            break;

                        case "3":
                            FindAppliance();
                            break;

                        case "4":
                            StartRepair();
                            break;

                        case "5":
                            CompleteRepair();
                            break;

                        case "6":
                            center.DisplaySummary();
                            Pause();
                            break;

                        case "0":
                            running = false;
                            Console.WriteLine("Goodbye!");
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                    Pause();
                }
            }
        }

        // إضافة جهاز جديد
        private static void AddAppliance()
        {
            Console.Clear();

            Console.WriteLine("===== ADD APPLIANCE =====");
            Console.WriteLine("1. Refrigerator");
            Console.WriteLine("2. Washing Machine");
            Console.WriteLine("3. Air Conditioner");
            Console.Write("Choose device type: ");

            string type = Console.ReadLine();

            if (type != "1" && type != "2" && type != "3")
            {
                Console.WriteLine("Invalid device type.");
                Pause();
                return;
            }

            Console.Write("Serial Number: ");
            string serial = Console.ReadLine();

            Console.Write("Customer Name: ");
            string customer = Console.ReadLine();

            Console.Write("Problem Description: ");
            string problem = Console.ReadLine();

            Console.Write("Additional Cost: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal additionalCost)
                || additionalCost < 0)
            {
                Console.WriteLine("Invalid additional cost.");
                Pause();
                return;
            }

            Appliance appliance; switch (type)
            {
                case "1":
                    Console.Write("Compressor Cost: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal compressorCost)
                        || compressorCost < 0)
                    {
                        Console.WriteLine("Invalid compressor cost.");
                        Pause();
                        return;
                    }

                    appliance = new Refrigerator(
                        serial, customer, problem,
                        DateTime.Now, compressorCost, additionalCost);
                    break;

                case "2":
                    Console.Write("Parts Cost: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal partsCost)
                        || partsCost < 0)
                    {
                        Console.WriteLine("Invalid parts cost.");
                        Pause();
                        return;
                    }

                    appliance = new WashingMachine(
                        serial, customer, problem,
                        DateTime.Now, partsCost, additionalCost);
                    break;

                default:
                    Console.Write("Gas Refill Cost: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal gasCost)
                        || gasCost < 0)
                    {
                        Console.WriteLine("Invalid gas refill cost.");
                        Pause();
                        return;
                    }

                    appliance = new AirConditioner(
                        serial, customer, problem,
                        DateTime.Now, gasCost, additionalCost);
                    break;
            }

            center.AddAppliance(appliance);

            Console.WriteLine("\nAppliance added successfully!");
            Console.WriteLine($"Estimated repair cost: {appliance.CalculateRepairCost():0.00}");

            Pause();
        }

        // البحث عن جهاز
        private static void FindAppliance()
        {
            Console.Clear();

            Console.Write("Enter serial number: ");
            string serial = Console.ReadLine();

            Appliance appliance = center.FindAppliance(serial);

            if (appliance == null)
            {
                Console.WriteLine("Appliance not found.");
            }
            else
            {
                Console.WriteLine("\n" + appliance.GetDetails());
            }

            Pause();
        }

        // بدء الصيانة
        private static void StartRepair()
        {
            Console.Clear();

            Console.Write("Enter serial number: ");
            string serial = Console.ReadLine();

            Appliance appliance = center.FindAppliance(serial);

            if (appliance == null)
            {
                Console.WriteLine("Appliance not found.");
            }
            else
            {
                appliance.StartRepair();
                Console.WriteLine("Repair started successfully.");
            }

            Pause();
        }

        // إنهاء الصيانة
        private static void CompleteRepair()
        {
            Console.Clear();

            Console.Write("Enter serial number: ");
            string serial = Console.ReadLine();

            Appliance appliance = center.FindAppliance(serial);

            if (appliance == null)
            {
                Console.WriteLine("Appliance not found.");
            }
            else
            {
                appliance.CompleteRepair();
                Console.WriteLine("Repair completed successfully.");
            }

            Pause();
        }

        // انتظار المستخدم قبل العودة للقائمة
        private static void Pause()
        {
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}