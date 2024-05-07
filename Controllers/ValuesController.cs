using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;
using Newtonsoft.Json;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public readonly IConfiguration _config;

        public ValuesController(IConfiguration configuration)
        {
            _config = configuration;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public string Get()
        {
            SqlConnection con = new SqlConnection(_config.GetConnectionString("WebApplication1Context").ToString());
            SqlDataAdapter ad = new SqlDataAdapter("select * from agents", con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            List<Agent> agents = new List<Agent>(); 
            if(dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Agent agent = new Agent();
                    agent.code = Convert.ToString(dt.Rows[i]["AGENT_CODE"]);
                    agent.name = Convert.ToString(dt.Rows[i]["AGENT_NAME"]);
                    agent.area = Convert.ToString(dt.Rows[i]["WORKING_AREA"]);
                    agent.commision = Convert.ToString(dt.Rows[i]["COMMISSION"]);
                    agent.phone = Convert.ToString(dt.Rows[i]["PHONE_NO"]);
                    agent.country = Convert.ToString(dt.Rows[i]["COUNTRY"]);
                    agents.Add(agent);
                }
            }
            if(agents.Count > 0)
            {
                return JsonConvert.SerializeObject(agents);
            }
            else
            {
                return "no data found";
            }

        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            SqlConnectionManager.GetConnection();
            string query = "select * from agents where AGENT_CODE = " + "'" + id + "'";
            SqlDataAdapter ad = new SqlDataAdapter(query, SqlConnectionManager._connection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            Agent agent = new Agent();
            
            if(dt.Rows.Count>0)
            {
                agent.code = Convert.ToString(dt.Rows[0]["AGENT_CODE"]).Trim();
                agent.name = Convert.ToString(dt.Rows[0]["AGENT_NAME"]).Trim();
                agent.phone = Convert.ToString(dt.Rows[0]["PHONE_NO"]).Trim();
                agent.country = Convert.ToString(dt.Rows[0]["COUNTRY"]).Trim();
                agent.commision = Convert.ToString(dt.Rows[0]["COMMISSION"]).Trim();
                agent.area = Convert.ToString(dt.Rows[0]["WORKING_AREA"]).Trim();
                SqlConnectionManager.CloseConnection();
                return JsonConvert.SerializeObject(agent);
            }
            else
            {
                return "no such record found";
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] Agent value)
        {
            SqlConnectionManager.GetConnection();
            string query = "insert into agents(AGENT_CODE, AGENT_NAME, PHONE_NO, COUNTRY, COMMISSION, WORKING_AREA) values(@param1,@param2,@param3,@param4,@param5,@param6)";
            SqlCommand cmd = new SqlCommand(query, SqlConnectionManager._connection);
            cmd.Parameters.AddWithValue("@param1", value.code);
            cmd.Parameters.AddWithValue("@param2", value.name);
            cmd.Parameters.AddWithValue("@param3", value.phone);
            cmd.Parameters.AddWithValue("@param4", value.country);
            cmd.Parameters.AddWithValue("@param5", value.commision);
            cmd.Parameters.AddWithValue("@param6", value.area);
            cmd.ExecuteNonQuery();
            SqlConnectionManager.CloseConnection();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(string code, [FromBody] string NewName)
        {
            SqlConnectionManager.GetConnection();
            string query = "update agents set AGENT_NAME=@param1 where AGENT_CODE=@param2";
            SqlCommand cmd = new SqlCommand(query, SqlConnectionManager._connection);
            cmd.Parameters.AddWithValue("@param1", NewName);
            cmd.Parameters.AddWithValue("@param2", code);
            cmd.ExecuteNonQuery();
            SqlConnectionManager.CloseConnection();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(string code)
        {
            SqlConnectionManager.GetConnection();
            string query = "delete from agents where AGENT_CODE=" + "'" + code + "'";
            SqlCommand cmd = new SqlCommand(query, SqlConnectionManager._connection);
            cmd.ExecuteNonQuery();
            SqlConnectionManager.CloseConnection();
        }
    }
}
