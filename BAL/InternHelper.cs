using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Npgsql;

namespace Core_MVC.BAL
{
    public class InternHelper
    {
        private readonly NpgsqlConnection _con;
        private readonly string _imagePath;
        public InternHelper(NpgsqlConnection con)
        {
            _con = con;
            _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        }


        public List<InterClass> FetchAllIntern()
        {
            try
            {
                var intern = new List<InterClass>();
                _con.Open();
                using NpgsqlCommand cmd = new NpgsqlCommand("SELECT *FROM t_internsdemo ", _con);

                using NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var i = new InterClass
                    {
                        c_internid1 = Convert.ToInt32(dr["c_internid"].ToString()),
                        c_InternName = dr["c_internname"].ToString(),
                        c_Gender = Convert.ToChar(dr["c_gender"]),
                        c_TopicId = Convert.ToInt32(dr["c_topicid"].ToString()),
                        c_Date_Of_Presentation = Convert.ToDateTime(dr["c_date_of_presentation"]),
                        c_Status = Convert.ToBoolean(dr["c_status"]),
                        c_Topic_Image = dr["c_topic_image"].ToString()
                    };
                    intern.Add(i);
                }
                return intern;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _con.Close();
            }
        }

        public InterClass FetchInternDetails(int id)
        {
            var intern = new InterClass();
            var topuc = new TopicClass();
            _con.Open();

            using var command = new NpgsqlCommand("SELECT * FROM t_internsdemo i JOIN t_assignedtask s ON i.c_topicid = s.c_topicid WHERE c_internid = @id", _con);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                intern.c_internid1 = Convert.ToInt32(reader["c_internid"]);
                intern.c_InternName = reader["c_internname"].ToString();
                intern.c_Gender = Convert.ToChar(reader["c_gender"]);
                intern.c_TopicId = Convert.ToInt32(reader["c_topicid"]);
                topuc.c_TopicName = reader["c_topicname"].ToString();
                intern.c_Date_Of_Presentation = Convert.ToDateTime(reader["c_date_of_presentation"]);
                intern.c_Status = Convert.ToBoolean(reader["c_status"]);
                intern.c_Topic_Image = reader["c_topic_image"] as string;
            }

            _con.Close();
            return intern;
        }

        public List<AssignTaskClass> GetTopics() {
            List<AssignTaskClass> topics = new();
            _con.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT c_topicid, c_topicname FROM t_assignedtask", _con))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            topics.Add(new AssignTaskClass
                            {
                                c_topicid = (int)reader["c_topicid"],
                                c_topicname = (string)reader["c_topicname"]
                            });
                        }
                    }
                }
                _con.Close();
                return topics;
        }

        public void AddNewIntern(InterClass intern)
        {
            using var command = new NpgsqlCommand("INSERT INTO t_internsdemo (c_internname, c_gender, c_topicid, c_date_of_presentation, c_status, c_topic_image) VALUES (@internname, @gender, @topicid, @date, @status, @image)", _con);
            command.Parameters.AddWithValue("@internname", intern.c_InternName);
            command.Parameters.AddWithValue("@gender", intern.c_Gender);
            command.Parameters.AddWithValue("@topicid", intern.c_TopicId);
            command.Parameters.AddWithValue("@date", intern.c_Date_Of_Presentation);
            command.Parameters.AddWithValue("@status", intern.c_Status);
            command.Parameters.AddWithValue("@image", intern.c_Topic_Image ?? (object)DBNull.Value);
            _con.Open();
            command.ExecuteNonQuery();
            _con.Close();
        }




        public void UpdateExistingIntern(InterClass intern)
        {
            _con.Open();

            using var command = new NpgsqlCommand(@"
    UPDATE t_InternsDemo
    SET
        c_InternName = @InternName,
        c_Gender = @Gender,
        c_TopicId = @TopicId,
        c_Date_Of_Presentation = @DateOfPresentation,
        c_Status = @Status,
        c_Topic_Image = @TopicImage
    WHERE c_internid = @InternId", _con);


            command.Parameters.AddWithValue("@InternId", intern.c_internid1);
            command.Parameters.AddWithValue("@InternName", intern.c_InternName);
            command.Parameters.AddWithValue("@Gender", intern.c_Gender);
            command.Parameters.AddWithValue("@TopicId", intern.c_TopicId);
            command.Parameters.AddWithValue("@DateOfPresentation", intern.c_Date_Of_Presentation);
            command.Parameters.AddWithValue("@Status", intern.c_Status);
            command.Parameters.AddWithValue("@TopicImage", (object)intern.c_Topic_Image ?? DBNull.Value);

            command.ExecuteNonQuery();
            _con.Close();
        }

        public void DeleteExistingIntern(int id)
        {
            var intern = FetchInternDetails(id);
            if (intern?.c_Topic_Image != null)
            {
                var imagePath = Path.Combine(_imagePath, Path.GetFileName(intern.c_Topic_Image));
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }


            using var command = new NpgsqlCommand("DELETE FROM t_internsdemo WHERE c_internid = @id", _con);
            command.Parameters.AddWithValue("@id", id);
            _con.Open();
            command.ExecuteNonQuery();
            _con.Close();
        }

    }
}