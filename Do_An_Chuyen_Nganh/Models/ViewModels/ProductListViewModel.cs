namespace Do_An_Chuyen_Nganh.Models.ViewModels
{
   // Tạo ViewModel này để lưu dữ liệu nhằm phân trang
    public class ProductListViewModel
    {
        // truyền categoryId vào để lúc nó phân trang qua cái page 2 nó gán vào
        public int categoryId { get; set; }
        public IEnumerable<Product> Products { get; set;} = Enumerable.Empty<Product>();

        public PagingInfo PagingInfo { get; set;} = new PagingInfo();
    }
}
