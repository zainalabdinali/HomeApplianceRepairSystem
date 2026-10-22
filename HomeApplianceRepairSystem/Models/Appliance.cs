using System;
using HomeApplianceRepairSystem.Enums;
using HomeApplianceRepairSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HomeApplicancerepairSystem.Models
{
    public abstract class Appliance : IRepairable
    {
        // الحقول الخاصة لتطبيق مبدأ التغليف
        private readonly string _serialNumber;
        private string _customerName;
        private string _problemDescription;
        private MaintenanceStatus _status;
        private DateTime _receivedDate;
        private DateTime? _completionDate;
        private decimal _additionalCost;

        // تعريف المفوض الذي يحدد شكل الدالة المستقبلة لإشعارات تغيير الحالة
        public delegate void MaintenanceStatusChangedHandler(
            Appliance appliance,
            MaintenanceStatus oldStatus,
            MaintenanceStatus newStatus);

        // تعريف الحدث الذي يُطلق عند تغير حالة الجهاز
        public event MaintenanceStatusChangedHandler StatusChanged;

        // المنشئ: تهيئة الجهاز عند تسجيله في مركز الصيانة
        protected Appliance(
            string serialNumber,
            string customerName,
            string problemDescription,
            DateTime receivedDate,
            decimal additionalCost = 0)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                throw new ArgumentException(
                    "Serial number cannot be empty.",
                    nameof(serialNumber));

            _serialNumber = serialNumber.Trim();

            CustomerName = customerName;
            ProblemDescription = problemDescription;
            ReceivedDate = receivedDate;
            AdditionalCost = additionalCost;

            // تحديد الحالة الابتدائية للجهاز
            _status = MaintenanceStatus.Received;

            // تاريخ الإنجاز يبقى فارغاً حتى انتهاء الصيانة
            _completionDate = null;
        }

        // الرقم التسلسلي: يمكن قراءته ولا يمكن تغييره بعد إنشاء الجهاز
        public string SerialNumber => _serialNumber;

        // خاصية اسم الزبون مع التحقق من صحة القيمة
        public string CustomerName
        {
            get => _customerName;

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "Customer name cannot be empty.");

                _customerName = value.Trim();
            }
        }

        // خاصية وصف العطل مع منع إدخال وصف فارغ
        public string ProblemDescription
        {
            get => _problemDescription;

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "Problem description cannot be empty.");

                _problemDescription = value.Trim();
            }
        }

        // خاصية قراءة حالة الجهاز الحالية
        public MaintenanceStatus Status => _status;

        // تاريخ استلام الجهاز: لا يمكن تعديله من خارج الفئة
        public DateTime ReceivedDate
        {
            get => _receivedDate;

            private set
            {
                if (value.Date > DateTime.Today)
                    throw new ArgumentException(
                        "Received date cannot be in the future.");

                _receivedDate = value;
            }
        }

        // تاريخ انتهاء الصيانة: تكون قيمته فارغة قبل إتمام الإصلاح
        public DateTime? CompletionDate => _completionDate;

        // خاصية التكاليف الإضافية مع منع القيم السالبة
        public decimal AdditionalCost
        {
            get => _additionalCost;

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Additional cost cannot be negative.");

                _additionalCost = value;
            }
        }
         // دالة مجردة تحسب تكلفة الإصلاح حسب نوع الجهاز
        public abstract decimal CalculateRepairCost();// بدء صيانة الجهاز
        public void StartRepair()
        {
            // لا يمكن بدء الصيانة إلا إذا كان الجهاز مستلماً
            if (_status != MaintenanceStatus.Received)
                throw new InvalidOperationException(
                    "Only received appliances can start repair.");

            // تغيير الحالة وإطلاق الحدث لإشعار الجهات المشتركة به
            ChangeStatus(MaintenanceStatus.InMaintenance);
        }

        // إنهاء صيانة الجهاز
        public void CompleteRepair()
        {
            // لا يمكن إنهاء الصيانة إلا إذا كان الجهاز قيد الإصلاح
            if (_status != MaintenanceStatus.InMaintenance)
                throw new InvalidOperationException(
                    "Only appliances under maintenance can be completed.");

            // تغيير الحالة إلى تم الإصلاح
            ChangeStatus(MaintenanceStatus.Repaired);

            // تسجيل تاريخ ووقت انتهاء الصيانة
            _completionDate = DateTime.Now;
        }

        // تغيير حالة الجهاز وإطلاق حدث لإبلاغ المشتركين به
        protected void ChangeStatus(MaintenanceStatus newStatus)
        {
            // عدم تنفيذ أي إجراء إذا كانت الحالة الجديدة مطابقة للحالية
            if (_status == newStatus)
                return;

            // حفظ الحالة القديمة قبل تحديثها
            MaintenanceStatus oldStatus = _status;

            // تحديث حالة الجهاز
            _status = newStatus;

            // إطلاق الحدث وإرسال الجهاز والحالتين القديمة والجديدة
            StatusChanged?.Invoke(
                this,
                oldStatus,
                newStatus);
        }

        // عرض المعلومات العامة للجهاز
        public virtual string GetDetails()
        {
            return
                $"Device Type: {GetType().Name}\n" +
                $"Serial Number: {SerialNumber}\n" +
                $"Customer: {CustomerName}\n" +
                $"Problem: {ProblemDescription}\n" +
                $"Status: {Status}\n" +
                $"Received Date: {ReceivedDate:yyyy-MM-dd}\n" +
                $"Completion Date: " +
                $"{(CompletionDate.HasValue ? CompletionDate.Value.ToString("yyyy-MM-dd HH:mm") : "Not completed")}\n" +
                $"Repair Cost: {CalculateRepairCost():0.00}";
        }
    }
}