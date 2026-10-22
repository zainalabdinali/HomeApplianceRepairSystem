using System;
using HomeApplianceRepairSystem.Models;

namespace HomeApplianceRepairSystem.Models
{
    public class Refrigerator : Appliance
    {
        private decimal _compressorCost;
        private decimal _laborCost;

        public Refrigerator(
            string serialNumber,
            string customerName,
            string problemDescription,
            DateTime receivedDate,
            decimal compressorCost,
            decimal laborCost,
            decimal additionalCost = 0)
            : base(
                serialNumber,
                customerName,
                problemDescription,
                receivedDate,
                additionalCost)
        {
            CompressorCost = compressorCost;
            LaborCost = laborCost;
        }

        public decimal CompressorCost
        {
            get => _compressorCost;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Compressor cost cannot be negative.");

                _compressorCost = value;
            }
        }

        public decimal LaborCost
        {
            get => _laborCost;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Labor cost cannot be negative.");

                _laborCost = value;
            }
        }
        //حساب تكلفة إصلاح البراد 
        public override decimal CalculateRepairCost()
        {
            return CompressorCost + LaborCost + AdditionalCost;
        }

        public override string GetDetails()
        {
            return base.GetDetails() +
                   $"\nCompressor Cost: {CompressorCost:0.00}" +
                   $"\nLabor Cost: {LaborCost:0.00}";
        }
    }
}
