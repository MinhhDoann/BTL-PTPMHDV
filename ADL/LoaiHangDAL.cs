using QuanLyContainer_API.Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyContainer_API.ADL
{
    public class LoaiHangDAL
    {
        private readonly string _connectionString;

        public LoaiHangDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<LoaiHang> GetAll()
        {
            var list = new List<LoaiHang>();
            const string sql = "SELECT LoaiHangID, TenLoai, MoTa FROM LoaiHang";

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(sql, conn);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new LoaiHang
                {
                    LoaiHangID = reader["LoaiHangID"].ToString(),
                    TenLoai = reader["TenLoai"].ToString(),
                    MoTa = reader["MoTa"].ToString()
                });
            }
            return list;
        }

        public LoaiHang GetById(string id)
        {
            const string sql = "SELECT LoaiHangID, TenLoai, MoTa FROM LoaiHang WHERE LoaiHangID = @id";

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@id", SqlDbType.NVarChar, 50).Value = id;

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new LoaiHang
                {
                    LoaiHangID = reader["LoaiHangID"].ToString(),
                    TenLoai = reader["TenLoai"].ToString(),
                    MoTa = reader["MoTa"].ToString()
                };
            }
            return null;
        }
        public bool Create(LoaiHang model)
        {
            const string sql = @"INSERT INTO LoaiHang (TenLoai, MoTa)
                                 VALUES (@TenLoai, @MoTa)";
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@TenLoai", SqlDbType.NVarChar, 100).Value =
                    string.IsNullOrWhiteSpace(model.TenLoai) ? DBNull.Value : model.TenLoai;

                cmd.Parameters.Add("@MoTa", SqlDbType.NVarChar, 255).Value =
                    string.IsNullOrWhiteSpace(model.MoTa) ? DBNull.Value : model.MoTa;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }


        public bool UpdatePartial(LoaiHang model)
        {
            var sql = "UPDATE LoaiHang SET ";
            var parameters = new List<SqlParameter>();
            bool hasChanges = false;

            if (model.TenLoai != null)
            {
                if (!string.IsNullOrWhiteSpace(model.TenLoai) && model.TenLoai != "string")
                {
                    sql += "TenLoai = @TenLoai, ";
                    parameters.Add(new SqlParameter("@TenLoai", SqlDbType.NVarChar, 100)
                    {
                        Value = model.TenLoai
                    });
                    hasChanges = true;
                }
            }

            if (model.MoTa != null)
            {
                sql += "MoTa = @MoTa, ";
                parameters.Add(new SqlParameter("@MoTa", SqlDbType.NVarChar, 255)
                {
                    Value = model.MoTa
                });
                hasChanges = true;
            }

            if (!hasChanges)
            {
                return false;
            }

            sql = sql.TrimEnd(',', ' ');
            sql += " WHERE LoaiHangID = @LoaiHangID";

            parameters.Add(new SqlParameter("@LoaiHangID", SqlDbType.NVarChar, 50)
            {
                Value = model.LoaiHangID
            });

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"SQL Error: {ex.Message}");
                        return false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return false;
                    }
                }
            }
        }


        public bool Delete(string id)
        {
            const string sql = "DELETE FROM LoaiHang WHERE LoaiHangID = @id";
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@id", SqlDbType.NVarChar, 50).Value = id;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
