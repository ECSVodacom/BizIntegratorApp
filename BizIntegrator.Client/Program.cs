using BizIntegrator.OrderManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BizIntegrator.Client
{
    public static class Program
    {
        static void Main(string[] args)
        {
            OrderHandler orderHandler = new OrderHandler();
            orderHandler.ProcessOrders();
        }
    }
}
