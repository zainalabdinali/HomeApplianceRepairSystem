using System;
using System.Collections.Generic;
using System.Linq;
using HomeApplianceRepairSystem.Enums;
using HomeApplianceRepairSystem.Models;

namespace HomeApplianceRepairSystem.Services
{
    public class MaintenanceCenter
    {
        private readonly List<Appliance> _appliances;

        // عدد الأجهزة المسجلة في المركز
        private static int _totalDevices;

        public MaintenanceCenter()
        {
            _appliances = new List<Appliance>();
        }

        // خاصية static لعدد الأجهزة المسجلة
        public static int TotalDevices => _totalDevices;

        // قائمة الأجهزة للقراءة فقط من خارج الفئة
        public IReadOnlyList<Appliance> Appliances => _appliances.AsReadOnly();

        // إضافة جهاز جديد
        public void AddAppliance(Appliance appliance)
        {
            if (appliance == null)
                throw new ArgumentNullException(nameof(appliance));

            bool serialExists = _appliances.Any(
                a => a.SerialNumber.Equals(
                    appliance.SerialNumber,
                    StringComparison.OrdinalIgnoreCase));

            if (serialExists)
                throw new InvalidOperationException(
                    "An appliance with this serial number already exists.");

            //الاشتراك بحدث تغيير حالة الجهاز
            appliance.StatusChanged += OnApplianceStatusChanged;
            //إضافة الجهاز إلى القائمة
            _appliances.Add(appliance);
            _totalDevices++;
        }
        // التعامل مع حدث تغيير حالة الجهاز
        private void OnApplianceStatusChanged(
            Appliance appliance,
            MaintenanceStatus oldStatus,
            MaintenanceStatus newStatus)
        {
            Console.WriteLine("\n===== STATUS CHANGE NOTIFICATION =====");

            Console.WriteLine($"Serial Number: {appliance.SerialNumber}");
            Console.WriteLine($"Old Status: {oldStatus}");
            Console.WriteLine($"New Status: {newStatus}");

            Console.WriteLine("======================================");
        }

        // البحث عن جهاز برقمه التسلسلي
        public Appliance FindAppliance(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return null;

            return _appliances.FirstOrDefault(
                a => a.SerialNumber.Equals(
                    serialNumber.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }

        // عرض جميع الأجهزة باستخدام تعدد الأشكال
        public void DisplayAllAppliances()
        {
            if (_appliances.Count == 0)
            {
                Console.WriteLine("No appliances have been registered.");
                return;
            }

            foreach (Appliance appliance in _appliances)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine(appliance.GetDetails());
            }

            Console.WriteLine("--------------------------------");
        }

        // حساب تكلفة صيانة جميع الأجهزة
        public decimal CalculateTotalRepairCost()
        {
            decimal total = 0;

            foreach (Appliance appliance in _appliances)
            {
                total += appliance.CalculateRepairCost();
            }

            return total;
        }

        // حساب تكلفة الأجهزة التي اكتملت صيانتها
        public decimal CalculateCompletedRepairCost()
        {
            decimal total = 0;

            foreach (Appliance appliance in _appliances)
            {
                if (appliance.Status == MaintenanceStatus.Repaired)
                {
                    total += appliance.CalculateRepairCost();
                }
            }

            return total;
        }

        // عدد الأجهزة حسب الحالة
        public int CountByStatus(MaintenanceStatus status)
        {
            return _appliances.Count(a => a.Status == status);
        }

        // عرض ملخص المركز
        public void DisplaySummary()
        {
            Console.WriteLine("\n===== MAINTENANCE CENTER SUMMARY =====");
            Console.WriteLine($"Total registered devices: {TotalDevices}");
            Console.WriteLine(
                $"Received: {CountByStatus(MaintenanceStatus.Received)}");
            Console.WriteLine(
                $"Under maintenance: {CountByStatus(MaintenanceStatus.InMaintenance)}");
            Console.WriteLine(
                $"Repaired: {CountByStatus(MaintenanceStatus.Repaired)}");
            Console.WriteLine($"Total estimated repair cost: {CalculateTotalRepairCost():0.00}");
            Console.WriteLine(
                $"Completed repair cost: {CalculateCompletedRepairCost():0.00}");
            Console.WriteLine("=======================================\n");
        }
    }
}