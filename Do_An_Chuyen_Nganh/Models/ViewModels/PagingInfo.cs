namespace Do_An_Chuyen_Nganh.Models.ViewModels
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }

        // Page hiện tại
        public int CurrentPage { get; set; }

        // Tổng số page: Tổng số sp / số sp có thể hiện ở 1 page ( ở đây giới hạn 9)
        public int TotalPages  => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
    }
}
