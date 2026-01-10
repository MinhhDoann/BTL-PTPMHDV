using Microsoft.Data.SqlClient;
using System.Data;
using QuanLyKH_TC_API.Model;

namespace QuanLyKH_TC_API.ADL
{
    public class HopDongDAL
    {
        private readonly string _connectionString;

        public HopDongDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<HopDong> GetAll()
        {
            var list = new List<HopDong>();
            const string sql = @"SELECT HopDongID, KhachHangID, NgayKy, LoaiDichVu, GiaTri, TrangThai FROM HopDong";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new HopDong
                {
                    HopDongID = Convert.ToInt32(reader["HopDongID"]),
                    KhachHangID = Convert.ToInt32(reader["KhachHangID"]),
                    NgayKy = Convert.ToDateTime(reader["NgayKy"]),
                    LoaiDichVu = reader["LoaiDichVu"] as string,
                    GiaTri = reader["GiaTri"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["GiaTri"]),
                    TrangThai = reader["TrangThai"] as string
                });
            }
            return list;
        }

        public HopDong? GetById(int id)
        {
            const string sql = @"SELECT HopDongID, KhachHangID, NgayKy, LoaiDichVu, GiaTri, TrangThai
                                 FROM HopDong WHERE HopDongID = @id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new HopDong
            {
                HopDongID = Convert.ToInt32(reader["HopDongID"]),
                KhachHangID = Convert.ToInt32(reader["KhachHangID"]),
                NgayKy = Convert.ToDateTime(reader["NgayKy"]),
                LoaiDichVu = reader["LoaiDichVu"] as string,
                GiaTri = reader["GiaTri"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["GiaTri"]),
                TrangThai = reader["TrangThai"] as string
            };
        }

        public bool Create(HopDong model)
        {
            const string sql = @"INSERT INTO HopDong(KhachHangID, NgayKy, LoaiDichVu, GiaTri, TrangThai)
                                 VALUES (@KhachHangID, @NgayKy, @LoaiDichVu, @GiaTri, @TrangThai)";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@KhachHangID", SqlDbType.Int).Value = model.KhachHangID;
                cmd.Parameters.Add("@NgayKy", SqlDbType.Date).Value = model.NgayKy;

                cmd.Parameters.Add("@LoaiDichVu", SqlDbType.NVarChar, 100).Value = (object?)model.LoaiDichVu ?? DBNull.Value;

                var pGiaTri = cmd.Parameters.Add("@GiaTri", SqlDbType.Decimal);
                pGiaTri.Precision = 15;
                pGiaTri.Scale = 2;
                pGiaTri.Value = (object?)model.GiaTri ?? DBNull.Value;

                cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value = (object?)model.TrangThai ?? DBNull.Value;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public bool Update(HopDong model)
        {
            const string sql = @"UPDATE HopDong
                                 SET KhachHangID=@KhachHangID, NgayKy=@NgayKy, LoaiDichVu=@LoaiDichVu, GiaTri=@GiaTri, TrangThai=@TrangThai
                                 WHERE HopDongID=@HopDongID";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@HopDongID", SqlDbType.Int).Value = model.HopDongID;
                cmd.Parameters.Add("@KhachHangID", SqlDbType.Int).Value = model.KhachHangID;
                cmd.Parameters.Add("@NgayKy", SqlDbType.Date).Value = model.NgayKy;

                cmd.Parameters.Add("@LoaiDichVu", SqlDbType.NVarChar, 100).Value = (object?)model.LoaiDichVu ?? DBNull.Value;

                var pGiaTri = cmd.Parameters.Add("@GiaTri", SqlDbType.Decimal);
                pGiaTri.Precision = 15;
                pGiaTri.Scale = 2;
                pGiaTri.Value = (object?)model.GiaTri ?? DBNull.Value;

                cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value = (object?)model.TrangThai ?? DBNull.Value;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public bool Delete(int id)
        {
            const string sql = @"DELETE FROM HopDong WHERE HopDongID=@id";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }
    }
}
