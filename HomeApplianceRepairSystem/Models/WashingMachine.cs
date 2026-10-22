using System;
using HomeApplicancerepairSystem.Models;

namespace HomeApplianceRepairSystem.Models
{
    public class WashingMachine : Appliance
    {
        private decimal _partsCost;
        private decimal _laborCost;

        public WashingMachine(
            string serialNumber,
            string customerName,
            string problemDescription,
            DateTime receivedDate,
            decimal partsCost,
            decimal laborCost,
            decimal additionalCost = 0)
            : base(
                serialNumber,
                customerName,
                problemDescription,
                receivedDate,
                additionalCost)
        {
            PartsCost = partsCost;
            LaborCost = laborCost;
        }

        public decimal PartsCost
        {
            get => _partsCost;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Parts cost cannot be negative.");

                _partsCost = value;
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
            return PartsCost + LaborCost + AdditionalCost;
        }

        public override string GetDetails()
        {
            return base.GetDetails() +
                   $"\nParts Cost: {PartsCost:0.00}" +
                   $"\nLabor Cost: {LaborCost:0.00}";
        }
    }
}