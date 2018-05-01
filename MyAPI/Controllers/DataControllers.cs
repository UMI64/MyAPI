using MyAPI.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Description;
using MySql.Data.MySqlClient;

namespace MyAPI.Controllers
{
    public class Stm32f10xController : ApiController
    {
        PinDataClass[] Pins = new PinDataClass[]
        {
            new PinDataClass {Name = "PA0", Function = new string[] { "f1","f2"} },
            new PinDataClass {Name = "PA1", Function = new string[] {"Toys" }},
            new PinDataClass {Name = "PA2", Function = new string[] {"MISO","GPIO" }},
            new PinDataClass {Name = "PA3", Function = new string[] {"MOSI","GPIO" }}
        };

        public IEnumerable<PinDataClass> GetAllProducts()
        {
            return Pins;
        }
        public IHttpActionResult GetProduct(string id)
        {
            List<PinDataClass> pins = new List<PinDataClass>();
            foreach (PinDataClass pin in Pins)
            {
                if (pin.Name.Contains(id))
                {
                    pins.Add(pin);
                }
            }
            if (pins == null)
            {
                return NotFound();
            }
            return Ok(pins);
        }
    }
    public class UserController : ApiController
    {
        int reasult;
        string constructorString = "server=127.0.0.1;uid=root;password=wym971117;Database=APPData;persistsecurityinfo=True;SslMode=none";
        [HttpPost]
        public IHttpActionResult CheckUser(UserClass user)
        {
            var myConnnect = new MySqlConnection(constructorString);
            try
            {
                myConnnect.Open();
                MySqlCommand myCmd = new MySqlCommand("", myConnnect);

                myCmd.CommandText = "select * from users where name='" + user.UserName + "'";
                MySqlDataReader msqlReader = myCmd.ExecuteReader();
                if (msqlReader.Read() == true)
                {
                    var ThePassWord = msqlReader["Password"];
                    msqlReader.Close();
                    if (ThePassWord.Equals(user.PassWord))
                    {
                        myConnnect.Close();
                        return Ok(true);
                    }
                }
                myConnnect.Close();
                return Ok(false);
            }
            catch (MySqlException ex)
            {
                user.UserName = ex.ToString();
                myConnnect.Close();
                return Ok(user);
            }
        }
        [HttpPost]
        public IHttpActionResult PostUser(UserClass user)
        {
            var myConnnect = new MySqlConnection(constructorString);
            try
            {
                myConnnect.Open();
                MySqlCommand myCmd = new MySqlCommand("", myConnnect);
                try
                {
                    myCmd.CommandText = "create table users(PassWord char(20),Name char(20) not null)";
                    reasult = myCmd.ExecuteNonQuery();
                }
                catch
                {

                }
                myCmd.CommandText = "select * from users where name='" + user.UserName + "'";
                MySqlDataReader msqlReader = myCmd.ExecuteReader();
                if (msqlReader.Read() == false)
                {
                    msqlReader.Close();
                    myCmd.CommandText = "insert into users(Name ,PassWord) values('" + user.UserName + "','" + user.PassWord + "')";
                    reasult = myCmd.ExecuteNonQuery();
                    myConnnect.Close();
                    return Ok(true);
                }
                myConnnect.Close();
                return Ok(false);
            }
            catch (MySqlException ex)
            {
                user.UserName = ex.ToString();
                myConnnect.Close();
                return Ok(user);
            }
        }
    }
}

