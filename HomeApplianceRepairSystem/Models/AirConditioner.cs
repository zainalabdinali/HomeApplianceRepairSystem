using System;
using HomeApplianceRepairSystem.Models;

namespace HomeApplianceRepairSystem.Models
{
    public class AirConditioner : Appliance
    {
        private decimal _gasRefillCost;
        private decimal _laborCost;

        public AirConditioner(
            string serialNumber,
            string customerName,
            string problemDescription,
            DateTime receivedDate,
            decimal gasRefillCost,
            decimal laborCost,
            decimal additionalCost = 0)
            : base(
                serialNumber,
                customerName,
                problemDescription,
                receivedDate,
                additionalCost)
        {
            GasRefillCost = gasRefillCost;
            LaborCost = laborCost;
        }

        public decimal GasRefillCost
        {
            get => _gasRefillCost;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Gas refill cost cannot be negative.");

                _gasRefillCost = value;
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

        public override decimal CalculateRepairCost()
        {
            return GasRefillCost + LaborCost + AdditionalCost;
        }

        public override string GetDetails()
        {
            return base.GetDetails() +
                   $"\nGas Refill Cost: {GasRefillCost:0.00}" +
                   $"\nLabor Cost: {LaborCost:0.00}";
        }
    }
}
