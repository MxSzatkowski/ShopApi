namespace ShopsApi.Models
{
    public class ShopQuery
    {
        public string SearchPhase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; } 
        public SortDirection SortDirection { get; set; }
    }
}
