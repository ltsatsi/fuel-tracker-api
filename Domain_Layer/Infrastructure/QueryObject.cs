namespace FuelTrackerAPI.Infrastructure
{
    public class QueryObject
    {
        // vehicle search
        public string? Registration { get; set; } = null;
        public string? Make { get; set; } = null;
        public int? Year { get; set; } = null;

        // fuel search
        public string? Station { get; set; } = null;

        // pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        // filtering
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
    } // end class
} // end interface
