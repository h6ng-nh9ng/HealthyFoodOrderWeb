namespace HealthyFoodOrdering.Models
{
    public static class OrderStatus
    {
        public const string WaitingPayment = "WaitingPayment";
        public const string WaitingConfirm = "WaitingConfirm"; public const string Pending = "Pending";
        public const string Confirmed = "Confirmed";
        public const string Preparing = "Preparing";
        public const string Delivering = "Delivering";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";

        public static readonly IReadOnlyList<string> All = new[]
        {
            WaitingPayment,
            WaitingConfirm,
            Pending,
            Confirmed,
            Preparing,
            Delivering,
            Completed,
            Cancelled
        };

        public static readonly IReadOnlyList<string> Workflow = new[]
        {
            Pending,
            Confirmed,
            Preparing,
            Delivering,
            Completed
        };

        public static bool IsValid(string? status)
        {
            return !string.IsNullOrWhiteSpace(status)
                && All.Contains(status);
        }

        public static string? GetNextStatus(string? status)
        {
            var currentIndex = Workflow
                .Select((value, index) => new { value, index })
                .FirstOrDefault(x => x.value == status)
                ?.index;

            if (currentIndex == null || currentIndex >= Workflow.Count - 1)
            {
                return null;
            }

            return Workflow[currentIndex.Value + 1];
        }

        public static bool CanMoveToNext(string? currentStatus, string? nextStatus)
        {
            return GetNextStatus(currentStatus) == nextStatus;
        }

        public static string GetDisplayName(string? status)
        {
            return status switch
            {
                WaitingPayment => "Cho thanh toan",
                WaitingConfirm => "Cho xac nhan thanh toan",
                Pending => "Cho xu ly",
                Confirmed => "Da xac nhan",
                Preparing => "Dang chuan bi",
                Delivering => "Dang giao",
                Completed => "Hoan thanh",
                Cancelled => "Da huy",
                _ => status ?? "Khong xac dinh"
            };
        }

        public static string GetBadgeClass(string? status)
        {
            return status switch
            {
                WaitingPayment => "badge-status badge-waiting",
                WaitingConfirm => "badge-status badge-confirm",
                Pending => "badge-status badge-pending",
                Confirmed => "badge-status badge-confirmed",
                Preparing => "badge-status badge-preparing",
                Delivering => "badge-status badge-delivering",
                Completed => "badge-status badge-completed",
                Cancelled => "badge-status badge-cancelled",
                _ => "badge-status badge-default"
            };
        }
    }
}
