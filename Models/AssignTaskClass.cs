using System.Text.Json.Serialization;

namespace Core_MVC.Models
{
    public class AssignTaskClass
    {
        [JsonPropertyName("c_topicid_2")]
        public int c_topicid { get; set; }
        public string? c_topicname { get; set; }
    }
}
