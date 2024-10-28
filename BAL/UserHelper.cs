using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Core_MVC.Models;
using Npgsql;

namespace Core_MVC.BAL
{
    public class UserHelper
    {
        private readonly string _connectionString;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserHelper(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _httpContextAccessor = httpContextAccessor;
        }

        public void addUser(User user)
        {
            try
            {
                NpgsqlConnection con = new NpgsqlConnection(_connectionString);
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO t_user (c_username, c_email, c_gender, c_password) VALUES(@p1,@p2,@p3,@p4)", con);
                cmd.Parameters.AddWithValue("@p1", user.c_username);
                cmd.Parameters.AddWithValue("@p2", user.c_email);
                Console.WriteLine(user.c_gender);
                cmd.Parameters.AddWithValue("@p3", user.c_gender);
                cmd.Parameters.AddWithValue("@p4", user.c_password);

                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
            }
        }

        public bool getExistEmail(string email)
        {
            try
            {
                NpgsqlConnection con = new NpgsqlConnection(_connectionString);
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(c_email) FROM t_user WHERE c_email = @email", con);
                cmd.Parameters.AddWithValue("@email", email);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int count = Convert.ToInt32(dr[0]);
                    return count > 0;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
            }
            return true;
        }

        public void sendMail(string email, string token)
        {
            string my_email = "pumbhadiyasahil@gmail.com";
            string receiverEmail = email;
            string url = $"http://localhost:5043/MVCUser/varifyToken?email={email}&token={token}";
            string body = $@"
                                <h2>Verification Email from Case</h2>
                                <p>Please verify via this link</p>
                                <a href ='{url}'>Verify your email</a>";


            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("pumbhadiyasahil@gmail.com", "tdkz zgfx rtde rygp"),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage(my_email, receiverEmail)
            {
                Body = body,
                IsBodyHtml = false,
            };

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void StoreTokenInDatabase(string email, string token)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                string query = "INSERT INTO t_token (email, token) VALUES (@Email, @Token)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("Email", email);
                    cmd.Parameters.AddWithValue("Token", token);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Email and token stored in the database.");
                }
            }
        }


        public bool CheckToken(string email, string token)
        {
            string query = @"
            SELECT COUNT(*) 
            FROM public.t_token
            WHERE email = @Email 
            AND token = @Token 
            AND created_at >= @ValidTimeLimit";

            DateTime validTimeLimit = DateTime.Now.AddMinutes(-2);

            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Token", token);
                        cmd.Parameters.AddWithValue("@ValidTimeLimit", validTimeLimit);

                        int result = Convert.ToInt32(cmd.ExecuteScalar());

                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public bool checkLogin(string username, string password)
        {
            try
            {
                NpgsqlConnection con = new NpgsqlConnection(_connectionString);
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(c_username) FROM t_user WHERE c_username = @username AND c_password = @password", con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int count = Convert.ToInt32(dr[0]);
                    Console.WriteLine("vvv" + count);

                    return count > 0;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
            }
            return false;
        }

        public void changePassword(string newPassword, string email)
        {
            try
            {
                NpgsqlConnection con = new NpgsqlConnection(_connectionString);
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE t_user SET c_password = @newPassword WHERE c_email=@email", con);
                cmd.Parameters.AddWithValue("@newPassword", newPassword);
                cmd.Parameters.AddWithValue("@email", email);

                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
            }
        }
    }
}