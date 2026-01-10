using Microsoft.Data.SqlClient;
using System.Data;
using QuanLyKH_TC_API.Model;

namespace QuanLyKH_TC_API.ADL
{
    public class ChiPhiDAL
    {
        private readonly string _connectionString;

        public ChiPhiDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ChiPhi> GetAll()
        {
            var list = new List<ChiPhi>();
            const string sql = @"SELECT ChiPhiID, HopDongID, ContainerID, LoaiChiPhi, SoTien, ThuKhachHang FROM ChiPhi";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ChiPhi
                {
                    ChiPhiID = Convert.ToInt32(reader["ChiPhiID"]),
                    HopDongID = reader["HopDongID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["HopDongID"]),
                    ContainerID = reader["ContainerID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ContainerID"]),
                    LoaiChiPhi = reader["LoaiChiPhi"] as string,
                    SoTien = reader["SoTien"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["SoTien"]),
                    ThuKhachHang = reader["ThuKhachHang"] as string
                });
            }
            return list;
        }

        public ChiPhi? GetById(int id)
        {
            const string sql = @"SELECT ChiPhiID, HopDongID, ContainerID, LoaiChiPhi, SoTien, ThuKhachHang
                                 FROM ChiPhi WHERE ChiPhiID = @id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new ChiPhi
            {
                ChiPhiID = Convert.ToInt32(reader["ChiPhiID"]),
                HopDongID = reader["HopDongID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["HopDongID"]),
                ContainerID = reader["ContainerID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ContainerID"]),
                LoaiChiPhi = reader["LoaiChiPhi"] as string,
                SoTien = reader["SoTien"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["SoTien"]),
                ThuKhachHang = reader["ThuKhachHang"] as string
            };
        }

        public bool Create(ChiPhi model)
        {
            const string sql = @"INSERT INTO ChiPhi(HopDongID, ContainerID, LoaiChiPhi, SoTien, ThuKhachHang)
                                 VALUES (@HopDongID, @ContainerID, @LoaiChiPhi, @SoTien, @ThuKhachHang)";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@HopDongID", SqlDbType.Int).Value = (object?)model.HopDongID ?? DBNull.Value;
                cmd.Parameters.Add("@ContainerID", SqlDbType.Int).Value = (object?)model.ContainerID ?? DBNull.Value;

                cmd.Parameters.Add("@LoaiChiPhi", SqlDbType.NVarChar, 100).Value = (object?)model.LoaiChiPhi ?? DBNull.Value;

                var pSoTien = cmd.Parameters.Add("@SoTien", SqlDbType.Decimal);
                pSoTien.Precision = 15;
                pSoTien.Scale = 2;
                pSoTien.Value = (object?)model.SoTien ?? DBNull.Value;

                cmd.Parameters.Add("@ThuKhachHang", SqlDbType.NVarChar, 10).Value = (object?)model.ThuKhachHang ?? DBNull.Value;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public bool Update(ChiPhi model)
        {
            const string sql = @"UPDATE ChiPhi
                                 SET HopDongID=@HopDongID, ContainerID=@ContainerID, LoaiChiPhi=@LoaiChiPhi, SoTien=@SoTien, ThuKhachHang=@ThuKhachHang
                                 WHERE ChiPhiID=@ChiPhiID";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@ChiPhiID", SqlDbType.Int).Value = model.ChiPhiID;
                cmd.Parameters.Add("@HopDongID", SqlDbType.Int).Value = (object?)model.HopDongID ?? DBNull.Value;
                cmd.Parameters.Add("@ContainerID", SqlDbType.Int).Value = (object?)model.ContainerID ?? DBNull.Value;

                cmd.Parameters.Add("@LoaiChiPhi", SqlDbType.NVarChar, 100).Value = (object?)model.LoaiChiPhi ?? DBNull.Value;

                var pSoTien = cmd.Parameters.Add("@SoTien", SqlDbType.Decimal);
                pSoTien.Precision = 15;
                pSoTien.Scale = 2;
                pSoTien.Value = (object?)model.SoTien ?? DBNull.Value;

                cmd.Parameters.Add("@ThuKhachHang", SqlDbType.NVarChar, 10).Value = (object?)model.ThuKhachHang ?? DBNull.Value;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public bool Delete(int id)
        {
            const string sql = @"DELETE FROM ChiPhi WHERE ChiPhiID=@id";
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
