using Microsoft.Data.SqlClient;
using System.Data;
using QuanLyKH_TC_API.Model;

namespace QuanLyKH_TC_API.ADL
{
    public class ThanhToanDAL
    {
        private readonly string _connectionString;

        public ThanhToanDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ThanhToan> GetAll()
        {
            var list = new List<ThanhToan>();
            const string sql = @"SELECT ThanhToanID, HoaDonID, SoTien, PhuongThuc, ThoiGian FROM ThanhToan";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ThanhToan
                {
                    ThanhToanID = Convert.ToInt32(reader["ThanhToanID"]),
                    HoaDonID = Convert.ToInt32(reader["HoaDonID"]),
                    SoTien = reader["SoTien"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["SoTien"]),
                    PhuongThuc = reader["PhuongThuc"] as string,
                    ThoiGian = reader["ThoiGian"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ThoiGian"])
                });
            }
            return list;
        }

        public ThanhToan? GetById(int id)
        {
            const string sql = @"SELECT ThanhToanID, HoaDonID, SoTien, PhuongThuc, ThoiGian
                                 FROM ThanhToan WHERE ThanhToanID = @id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new ThanhToan
            {
                ThanhToanID = Convert.ToInt32(reader["ThanhToanID"]),
                HoaDonID = Convert.ToInt32(reader["HoaDonID"]),
                SoTien = reader["SoTien"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["SoTien"]),
                PhuongThuc = reader["PhuongThuc"] as string,
                ThoiGian = reader["ThoiGian"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ThoiGian"])
            };
        }

        public bool Create(ThanhToan model)
        {
            const string sql = @"INSERT INTO ThanhToan(HoaDonID, SoTien, PhuongThuc, ThoiGian)
                                 VALUES (@HoaDonID, @SoTien, @PhuongThuc, @ThoiGian)";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@HoaDonID", SqlDbType.Int).Value = model.HoaDonID;

                var pSoTien = cmd.Parameters.Add("@SoTien", SqlDbType.Decimal);
                pSoTien.Precision = 15;
                pSoTien.Scale = 2;
                pSoTien.Value = (object?)model.SoTien ?? DBNull.Value;

                cmd.Parameters.Add("@PhuongThuc", SqlDbType.NVarChar, 50).Value = (object?)model.PhuongThuc ?? DBNull.Value;
                cmd.Parameters.Add("@ThoiGian", SqlDbType.DateTime).Value = (object?)model.ThoiGian ?? DBNull.Value;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public bool Update(ThanhToan model)
        {
            const string sql = @"UPDATE ThanhToan
                                 SET HoaDonID=@HoaDonID, SoTien=@SoTien, PhuongThuc=@PhuongThuc, ThoiGian=@ThoiGian
                                 WHERE ThanhToanID=@ThanhToanID";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@ThanhToanID", SqlDbType.Int).Value = model.ThanhToanID;
                cmd.Parameters.Add("@HoaDonID", SqlDbType.Int).Value = model.HoaDonID;

                var pSoTien = cmd.Parameters.Add("@SoTien", SqlDbType.Decimal);
                pSoTien.Precision = 15;
                pSoTien.Scale = 2;
                pSoTien.Value = (object?)model.SoTien ?? DBNull.Value;

                cmd.Parameters.Add("@PhuongThuc", SqlDbType.NVarChar, 50).Value = (object?)model.PhuongThuc ?? DBNull.Value;
                cmd.Parameters.Add("@ThoiGian", SqlDbType.DateTime).Value = (object?)model.ThoiGian ?? DBNull.Value;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public bool Delete(int id)
        {
            const string sql = @"DELETE FROM ThanhToan WHERE ThanhToanID=@id";
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
