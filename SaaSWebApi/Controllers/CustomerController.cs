using System.Net;
using System.Web.Http;
using SaaSDTO;
using SaaSDAL;
using SaaSService;
using System.Web;
using System.Text;
using SaaSWebAPI;
using System.Collections.Generic;

namespace SaaSWebApi.Controllers
{
    [Authenticate]
    public class CustomerController : ApiController
    {
        // GET: Customer
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]       
        public SaaSResponse Customer(string uname, string pin)
        {
            SaaSResponse objResponseXML = new SaaSResponse();
            List<SaaSCustomer> oCustomers = new List<SaaSCustomer>();
            //oCustomers.GetCustomers();
            //oCustomers.Name = "Krutika";
            //oCustomers.Email = "krutika@gmail.com";
            //oCustomers.Mobile = "674983";
            //oCustomers.Address = "hskdjfhkj";
            //oCustomers.Notes = "adgfjdagfj";
            CustomerService customer = new CustomerService();
            var customerContext = customer.GetCustomers();
            foreach(var item in customerContext )
            {
                oCustomers.Add(new SaaSCustomer() { Name = item.Name, Email = item.Email, Mobile = item.Mobile, Address = item.Address, Notes = item.Notes });
            }
            objResponseXML._saasData = Common.GetXMLFromObject<List<SaaSCustomer>>(oCustomers);
            return objResponseXML;
        }

    }
}