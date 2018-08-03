using System.Collections.Generic;
using System.Linq;
using SaaSDTO;
using SaaSDAL;

namespace SaaSService
{
    public class CustomerService
    {
        public List<Customers> GetCustomers()
        {
            List<Customers> customers = null;
            using (HackSaaSEntities context = new HackSaaSEntities())
            {
                try
                {
                    customers = context.Customers.ToList();
                }
                catch (System.Exception)
                { }
            }

            return customers;
        }


    }
}
