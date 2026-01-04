namespace QuanLyContainer_API.Model
{
    public class Container
    {
        public int ContainerID { get; set; }
        public int HopDongID { get; set; }
        public int LoaiHangID { get; set; }
        public decimal? TrongLuong { get; set; }
        public string? TrangThai { get; set; }
        public int? KhoID { get; set; }
        public int? PhuongTienID { get; set; }
        public int? ChuyenDiID { get; set; }
    }
}
