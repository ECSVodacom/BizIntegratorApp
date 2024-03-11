using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Numerics;
using System.Diagnostics;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace BizIntegrator.Data
{
    public class DataHandler
    {
        public string SetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            return connectionString;
        }

        public DataTable GetApiData(string transactionType)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    DataTable dataTable = new DataTable();

                    connection.Open();

                    string queryString = "select api.Id, api.AccountKey, api.[Name], ep.[EndPoint], api.PrivateKey " +
                                            ",api.Username " +
                                            ",api.[Password] " +
                                            ", api.AuthenticationType, api.UseAPIKey, ep.TransactionType " +
                                            "from APIs api " +
                                            "inner join APIEndpoints ep  " +
                                            "on api.Id = ep.API_Id " +
                                            "WHERE ep.TransactionType = @TransactionType AND IsActive = 1 ";

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@TransactionType", transactionType);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {
                            da.Fill(dataTable);
                        }
                    }

                    return dataTable;
                }

            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "GetApiData");

                throw;
            }
        }

        public bool CheckOrders(string ordNo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    connection.Open();

                    string query = "SELECT ISNULL(Processed, 0) AS Processed FROM Orders WHERE OrdNo = @OrdNo";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.VarChar) { Value = ordNo });

                        using (SqlDataAdapter dt = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            dt.Fill(dataTable);

                            if (Convert.ToBoolean(dataTable.Rows[0]["Processed"]) == true)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "CheckOrders");
                throw;
            }
        }

        public void UpdateProcessedOrder(string ordNo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE Orders SET Processed = 1 WHERE ordNo = @ordNo", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ordNo", ordNo);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "UpdateProcessedOrder");
                throw;
            }
        }

        public DataTable GetEanCodes(string vendorNo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    connection.Open();
                    string queryString = "SELECT api.EanCode AS SenderGln, sv.EanCode AS RecieverGln " +
                                        "FROM APIs api " +
                                        "INNER JOIN Suppliers s ON api.ID = s.API_Id " +
                                        "INNER JOIN SupplierVendors sv ON s.Id = sv.SupplierId " +
                                        "WHERE Vendor = @VendorNo";
                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@VendorNo", vendorNo);

                        SqlDataAdapter dt = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        dt.Fill(ds);

                        return ds.Tables[0];
                    }
                }

            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "GetEanCodes");
                throw;
            }
        }

        public bool ResendOrders(string orderNo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    connection.Open();

                    string query = "SELECT Processed FROM Orders WHERE OrdNo = @OrderNo";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderNo", orderNo);

                        SqlDataAdapter dt = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        dt.Fill(ds);

                        if (ds.Tables[0].Rows.Count > 0 && Convert.ToBoolean(ds.Tables[0].Rows[0]["Processed"]))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "ResendOrders");
                throw;
            }
        }

        public void WriteException(String exception, string method)
        {
            try
            {
                RemoveQuotes(exception);

                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    connection.Open();

                    string query = "INSERT INTO MessageException(ExceptionDate, Exception, Method) VALUES (@ExceptionDate, @Exception, @Method)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ExceptionDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Exception", RemoveQuotes(exception));
                        cmd.Parameters.AddWithValue("@Method", method);

                        cmd.ExecuteNonQuery();
                    }
                }


            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "WriteException");
                throw;
            }


        }

        static string RemoveQuotes(string input)
        {
            return input.Replace("'", "").Replace("\"", "").Replace("`", "");
        }

        public void CreateCustomerList(string customerCode, string customerName, string physicalAddress, string email
                            , string branchCode, string ucARBrnchNo, string dateTimeStamp, string groupCode, string groupDescription
                            , string area, string areaDescription, string ulARWHLinked, string id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_CREATE_CustomerList", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@CustomerCode", SqlDbType.VarChar).Value = customerCode;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = customerName;
                        cmd.Parameters.Add("@PhysicalAddress", SqlDbType.VarChar).Value = physicalAddress;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
                        cmd.Parameters.Add("@UcARBrnchNo", SqlDbType.VarChar).Value = ucARBrnchNo;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = branchCode;
                        cmd.Parameters.Add("@DateTimeStamp", SqlDbType.VarChar).Value = dateTimeStamp;
                        cmd.Parameters.Add("@GroupCode", SqlDbType.VarChar).Value = groupCode;
                        cmd.Parameters.Add("@GroupDescription", SqlDbType.VarChar).Value = groupDescription;
                        cmd.Parameters.Add("@Area", SqlDbType.VarChar).Value = area;
                        cmd.Parameters.Add("@AreaDescription", SqlDbType.VarChar).Value = areaDescription;
                        cmd.Parameters.Add("@UlARWHLinked", SqlDbType.VarChar).Value = ulARWHLinked;
                        cmd.Parameters.Add("@API_Id", SqlDbType.VarChar).Value = id;
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "CreateOrders");
                throw;
            }
        }

        public void CreateOrders(string ordNo,string ordDate,string ordDesc,string ordType,string ordTerm
                                ,string ordTermDesc, string ordStat,string orderStatus,string origin, string promDate
                                ,string compName,string branchNo,string branchName,string branchAddr1,string branchAddr2
                                ,string branchTel,string branchFax,string branchEmail,string branchVat,string vendorRef
                                ,string vendorNo,string vendorName,string vendorAddr1,string vendorAddr2,string vendorSuburb
                                ,string vendorCity,string vendorContact, string totLines, string totQty, string totExcl
                                , string totTax, string totVal,string delivAddr1,string delivAddr2,string delivSuburb
                                ,string delivCity,string buyerNote,bool confirmInd,string compID,bool resendOrder
                                ,bool processed, string id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_CREATE_ORDER", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@OrdNo", SqlDbType.VarChar).Value = ordNo;
                        cmd.Parameters.Add("@OrdDate", SqlDbType.VarChar).Value = ordDate;
                        cmd.Parameters.Add("@OrdDesc", SqlDbType.VarChar).Value = ordDesc;
                        cmd.Parameters.Add("@OrdType", SqlDbType.VarChar).Value = ordType;
                        cmd.Parameters.Add("@OrdTerm", SqlDbType.VarChar).Value = ordTerm;
                        cmd.Parameters.Add("@OrdTermDesc", SqlDbType.VarChar).Value = ordTermDesc;
                        cmd.Parameters.Add("@OrdStat", SqlDbType.Int).Value = ordStat;
                        cmd.Parameters.Add("@OrderStatus", SqlDbType.VarChar).Value = orderStatus;
                        cmd.Parameters.Add("@Origin", SqlDbType.VarChar).Value = origin;
                        cmd.Parameters.Add("@CompName", SqlDbType.VarChar).Value = compName;
                        cmd.Parameters.Add("@BranchNo", SqlDbType.VarChar).Value = branchNo;
                        cmd.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = branchName;
                        cmd.Parameters.Add("@BranchAddr1", SqlDbType.VarChar).Value = branchAddr1;
                        cmd.Parameters.Add("@BranchAddr2", SqlDbType.VarChar).Value = branchAddr2;
                        cmd.Parameters.Add("@BranchTel", SqlDbType.VarChar).Value = branchTel;
                        cmd.Parameters.Add("@BranchFax", SqlDbType.VarChar).Value = branchFax;
                        cmd.Parameters.Add("@BranchEmail", SqlDbType.VarChar).Value = branchEmail;
                        cmd.Parameters.Add("@BranchVat", SqlDbType.VarChar).Value = branchVat;
                        cmd.Parameters.Add("@VendorRef", SqlDbType.VarChar).Value = vendorRef;
                        cmd.Parameters.Add("@VendorNo", SqlDbType.VarChar).Value = vendorNo;
                        cmd.Parameters.Add("@VendorName", SqlDbType.VarChar).Value = vendorName;
                        cmd.Parameters.Add("@VendorAddr1", SqlDbType.VarChar).Value = vendorAddr1;
                        cmd.Parameters.Add("@VendorAddr2", SqlDbType.VarChar).Value = vendorAddr2;
                        cmd.Parameters.Add("@VendorSuburb", SqlDbType.VarChar).Value = vendorSuburb;
                        cmd.Parameters.Add("@VendorCity", SqlDbType.VarChar).Value = vendorCity;
                        cmd.Parameters.Add("@VendorContact", SqlDbType.VarChar).Value = vendorContact;
                        cmd.Parameters.Add("@TotLines", SqlDbType.Int).Value = totLines;
                        cmd.Parameters.Add("@TotQty", SqlDbType.Decimal).Value = decimal.Parse(totQty, CultureInfo.InvariantCulture); 
                        cmd.Parameters.Add("@TotExcl", SqlDbType.Decimal).Value = decimal.Parse(totExcl, CultureInfo.InvariantCulture); 
                        cmd.Parameters.Add("@TotTax", SqlDbType.Decimal).Value = decimal.Parse(totTax, CultureInfo.InvariantCulture); 
                        cmd.Parameters.Add("@TotVal", SqlDbType.Decimal).Value = decimal.Parse(totVal, CultureInfo.InvariantCulture); 
                        cmd.Parameters.Add("@DelivAddr1", SqlDbType.VarChar).Value = delivAddr1;
                        cmd.Parameters.Add("@DelivAddr2", SqlDbType.VarChar).Value = delivAddr2;
                        cmd.Parameters.Add("@DelivSuburb", SqlDbType.VarChar).Value = delivSuburb;
                        cmd.Parameters.Add("@DelivCity", SqlDbType.VarChar).Value = delivCity;
                        cmd.Parameters.Add("@ConfirmInd", SqlDbType.Bit).Value = confirmInd;
                        cmd.Parameters.Add("@CompID", SqlDbType.VarChar).Value = compID;
                        cmd.Parameters.Add("@ResendOrder", SqlDbType.Bit).Value = resendOrder != null ? Convert.ToBoolean(resendOrder) : false;
                        cmd.Parameters.Add("@Processed", SqlDbType.Bit).Value = processed != null ? Convert.ToBoolean(processed) : false;
                        cmd.Parameters.Add("@API_Id", SqlDbType.VarChar).Value = id;
                        cmd.ExecuteNonQuery();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "CreateOrders");
                throw;
            }
        }

        public void CreateOrderLines(string ordLn,string ordNo,string itemNo,string itemDesc,string mfrItem, string qtyConv
                                    , string ordQty,string purcUom, string purcUomConv,string taxCde, string taxRate
                                    , string unitPrc, string lineTotExcl, string lineTotTax, string lineTotVal)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_CREATE_ORDER_LINES", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@OrdLn", SqlDbType.Int).Value = Convert.ToInt32(ordLn);
                        cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = ordNo;
                        cmd.Parameters.Add("@ItemNo", SqlDbType.VarChar).Value = itemNo;
                        cmd.Parameters.Add("@ItemDesc", SqlDbType.VarChar).Value = itemDesc;
                        cmd.Parameters.Add("@QtyConv", SqlDbType.Decimal).Value = decimal.Parse(qtyConv, CultureInfo.InvariantCulture);
                        cmd.Parameters.Add("@OrdQty", SqlDbType.Decimal).Value = decimal.Parse(ordQty, CultureInfo.InvariantCulture);
                        cmd.Parameters.Add("@PurcUom", SqlDbType.VarChar).Value = purcUom;
                        cmd.Parameters.Add("@PurcUomConv", SqlDbType.Decimal).Value = decimal.Parse(purcUomConv, CultureInfo.InvariantCulture);
                        cmd.Parameters.Add("@TaxCde", SqlDbType.VarChar).Value = taxCde;
                        cmd.Parameters.Add("@TaxRate", SqlDbType.Decimal).Value = decimal.Parse(taxRate, CultureInfo.InvariantCulture);
                        cmd.Parameters.Add("@UnitPrc", SqlDbType.Decimal).Value = decimal.Parse(unitPrc, CultureInfo.InvariantCulture);
                        cmd.Parameters.Add("@LineTotExcl", SqlDbType.Decimal).Value = decimal.Parse(lineTotExcl, CultureInfo.InvariantCulture);
                        cmd.Parameters.Add("@LineTotTax", SqlDbType.Decimal).Value = decimal.Parse(lineTotTax, CultureInfo.InvariantCulture);
                        cmd.Parameters.Add("@LineTotVal", SqlDbType.Decimal).Value = decimal.Parse(lineTotVal, CultureInfo.InvariantCulture);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "CreateOrderLines");
                throw;
            }
        }

        public DataTable GetOrdersLines(string orderNo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    DataTable dataTable = new DataTable();

                    sb.Clear();
                    sb.AppendLine("SELECT [OrdLn],[OrderNo],[ItemNo],[ItemDesc],[MfrItem] ");
                    sb.AppendLine(",[QtyConv],[OrdQty],[PurcUom],[PurcUomConv],[TaxCde] ");
                    sb.AppendLine(",[TaxRate],[UnitPrc],[LineTotExcl],[LineTotTax],[LineTotVal] ");
                    sb.AppendLine("FROM [dbo].[OrderLines] ");
                    sb.AppendLine("WHERE OrderNo = @OrderNo");

                    connection.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(sb.ToString(), connection))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@OrderNo", orderNo);
                        da.Fill(dataTable);
                    }

                    return dataTable;
                }

            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "GetOrderLines");
                throw;
            }

        }

        public void CreateInvoice(string invoiceNumber, int invoiceId, string invoiceDate, int documentState
            , string orderNumber, string externalOrderNumber, string customerCode, double grossTotalInVat
            , double grossTotalExVat, double grossTaxTotal, double discountAmountInVat, double discountAmountExVat
            , double netTotalExVat, double netTaxTotal, int totalInvoiceRounding, double netTotalInVat, bool processed, string id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_CREATE_Invoice", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@InvoiceNumber", SqlDbType.VarChar).Value = invoiceNumber;
                        cmd.Parameters.Add("@InvoiceId", SqlDbType.VarChar).Value = invoiceId;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.VarChar).Value = invoiceDate;
                        cmd.Parameters.Add("@DocumentState", SqlDbType.Int).Value = documentState;
                        cmd.Parameters.Add("@OrderNumber", SqlDbType.VarChar).Value = orderNumber;
                        cmd.Parameters.Add("@ExternalOrderNumber", SqlDbType.VarChar).Value = externalOrderNumber;
                        cmd.Parameters.Add("@CustomerCode", SqlDbType.VarChar).Value = customerCode;
                        cmd.Parameters.Add("@GrossTotalInVat", SqlDbType.Decimal).Value = grossTotalInVat;
                        cmd.Parameters.Add("@GrossTotalExVat", SqlDbType.Decimal).Value = grossTotalExVat;
                        cmd.Parameters.Add("@GrossTaxTotal", SqlDbType.Decimal).Value = grossTaxTotal;
                        cmd.Parameters.Add("@DiscountAmountInVat", SqlDbType.Decimal).Value = discountAmountInVat;
                        cmd.Parameters.Add("@DiscountAmountExVat", SqlDbType.Decimal).Value = discountAmountExVat;
                        cmd.Parameters.Add("@NetTotalExVat", SqlDbType.Decimal).Value = netTotalExVat;
                        cmd.Parameters.Add("@NetTaxTotal", SqlDbType.Decimal).Value = netTaxTotal;
                        cmd.Parameters.Add("@TotalInvoiceRounding", SqlDbType.Int).Value = totalInvoiceRounding;
                        cmd.Parameters.Add("@NetTotalInVat", SqlDbType.Decimal).Value = netTotalInVat;
                        cmd.Parameters.Add("@Processed", SqlDbType.Bit).Value = processed != null ? Convert.ToBoolean(processed) : false;
                        cmd.Parameters.Add("@API_Id", SqlDbType.VarChar).Value = id;
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "CreateOrders");
                throw;
            }
        }

        public void CreateInvoiceLines(string invoiceNumber, int invoiceId, string warehouseCode, string itemCode, string moduleCode
            , string lineDescription, double unitPriceExcl, double unitPriceIncl, int quantity, string unitOfMeasure
            , double lineNetTotalOrderedInVat, double lineNetTotalOrderedExVat, double lineNetTotalProcessedInVat
            , double lineNetTotalProcessedExVat, string lineNotes)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_CREATE_InvoiceLine", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@InvoiceNumber", SqlDbType.VarChar).Value = invoiceNumber;
                        cmd.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = invoiceId;
                        cmd.Parameters.Add("@WarehouseCode", SqlDbType.VarChar).Value = warehouseCode;
                        cmd.Parameters.Add("@ItemCode", SqlDbType.VarChar).Value = itemCode;
                        cmd.Parameters.Add("@ModuleCode", SqlDbType.VarChar).Value = moduleCode;
                        cmd.Parameters.Add("@LineDescription", SqlDbType.VarChar).Value = lineDescription;
                        cmd.Parameters.Add("@UnitPriceExcl", SqlDbType.Decimal).Value = unitPriceExcl;
                        cmd.Parameters.Add("@UnitPriceIncl", SqlDbType.Decimal).Value = unitPriceIncl;
                        cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity;
                        cmd.Parameters.Add("@UnitOfMeasure", SqlDbType.VarChar).Value = unitOfMeasure;
                        cmd.Parameters.Add("@LineNetTotalOrderedInVat", SqlDbType.Decimal).Value = lineNetTotalOrderedInVat;
                        cmd.Parameters.Add("@LineNetTotalOrderedExVat", SqlDbType.Decimal).Value = lineNetTotalOrderedExVat;
                        cmd.Parameters.Add("@LineNetTotalProcessedInVat", SqlDbType.Decimal).Value = lineNetTotalProcessedInVat;
                        cmd.Parameters.Add("@LineNetTotalProcessedExVat", SqlDbType.Decimal).Value = lineNetTotalProcessedExVat;
                        cmd.Parameters.Add("@LineNotes", SqlDbType.VarChar).Value = lineNotes;

                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "CreateOrders");
                throw;
            }
        }

        public bool CheckStockBarcode(string productCode)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    DataTable dataTable = new DataTable();

                    connection.Open();

                    string query = "SELECT * FROM StockBarcodes WHERE ProductCode = @ProductCode";

                    using (SqlDataAdapter da = new SqlDataAdapter(query, connection))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@ProductCode", productCode);

                        da.Fill(dataTable);
                    }

                    int rowCount = dataTable.Rows.Count;

                    return rowCount > 0;
                }


            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "CheckStockList");

                throw;
            }
        }

        public bool CheckCustomerList(string customerCode)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    DataTable dataTable = new DataTable();

                    connection.Open();

                    string query = "SELECT * FROM CustomerList WHERE CustomerCode = @CustomerCode";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerCode", customerCode);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {
                            da.Fill(dataTable);
                        }
                    }

                    int rowCount = dataTable.Rows.Count;

                    if (rowCount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "CheckCustomerList");

                throw;
            }
        }

        public string GetCustomerCode(string branchCode)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    DataTable dataTable = new DataTable();

                    using (SqlCommand cmd = new SqlCommand("SELECT CustomerCode FROM CustomerList WHERE BranchCode = @BranchCode", connection))
                    {
                        cmd.Parameters.AddWithValue("@BranchCode", branchCode);

                        connection.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dataTable);
                        }
                    }

                    string customerCode = string.Empty;

                    if (dataTable.Rows.Count > 0)
                    {
                        customerCode = dataTable.Rows[0]["CustomerCode"].ToString();
                    }

                    return customerCode;


                }

            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "GetCustomerCode");

                throw;
            }
        }
        public bool CheckInvoice(string invoiceId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    DataTable dataTable = new DataTable();

                        connection.Open();

                        string query = "SELECT Processed FROM Invoices WHERE InvoiceId = @InvoiceId";

                        using (SqlDataAdapter da = new SqlDataAdapter(query, connection))
                        {
                            da.SelectCommand.Parameters.AddWithValue("@InvoiceId", invoiceId);
                            da.Fill(dataTable);
                        }
                    

                    int rowCount = dataTable.Rows.Count;

                    if (rowCount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "CheckInvoice");

                throw;
            }
        }
        public bool CheckProcessedInvoice(string invoiceId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    DataTable dataTable = new DataTable();

                    bool result = false;

                    connection.Open();
                    string query = "SELECT Processed FROM Invoices WHERE InvoiceId = @InvoiceId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InvoiceId", invoiceId);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {
                            da.Fill(dataTable);
                        }
                    }

                    int rowCount = dataTable.Rows.Count;
                    if (rowCount > 0)
                    {
                        if (dataTable.Rows[0]["Processed"].ToString() == "True")
                        {
                            result = true;
                        }
                        else if (dataTable.Rows[0]["Processed"].ToString() == "False")
                        {
                            result = false;
                        }
                    }

                    return result;
                }

            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "CheckInvoice");

                throw;
            }
        }
        public void UpdateProcessedInvoice(string invoiceId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    connection.Open();

                    string updateQuery = "UPDATE Invoices SET Processed = 1 WHERE InvoiceId = @InvoiceId";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "UpdateProcessedInvoice");
                throw;
            }
        }
        public void CreateStocBarcode(string productCode, string productDescription, string bottleBarcode, string caseBarcode, string bottleUom, string caseUom, int unitsPerCase, string id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_CREATE_StockBarcode", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = productCode;
                        cmd.Parameters.Add("@ProductDescription", SqlDbType.VarChar).Value = productDescription;
                        cmd.Parameters.Add("@BottleBarcode", SqlDbType.VarChar).Value = bottleBarcode;
                        cmd.Parameters.Add("@CaseBarcode", SqlDbType.VarChar).Value = caseBarcode;
                        cmd.Parameters.Add("@UomBottle", SqlDbType.VarChar).Value = bottleUom;
                        cmd.Parameters.Add("@UomCase", SqlDbType.VarChar).Value = caseUom;
                        cmd.Parameters.Add("@UnitsPerCase", SqlDbType.Int).Value = unitsPerCase;
                        cmd.Parameters.Add("@API_Id", SqlDbType.VarChar).Value = id;
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "CreateStockBarcode");
                throw;
            }
        }

        public DataTable GetCustomerData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    DataTable dataTable = new DataTable();

                    StringBuilder sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine("SELECT * FROM [dbo].[CustomerList] ");

                    connection.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(sb.ToString(), connection))
                    {
                        da.Fill(dataTable);
                    }
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "GetOrderLines");
                throw;
            }

        }

        public DataTable GetApiDataPerName(string headerValue, string transactionType)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SetConnectionString()))
                {
                    DataTable dataTable = new DataTable();
                    StringBuilder sb = new StringBuilder();
                    connection.Open();

                    string queryString = "SELECT api.Id, api.AccountKey, api.[Name], ep.[EndPoint], api.PrivateKey, api.Username, api.[Password], api.AuthenticationType" +
                                        ", api.UseAPIKey, ep.TransactionType " +
                                        "FROM APIs api " +
                                        "INNER JOIN APIEndpoints ep ON api.Id = ep.API_Id " +
                                        "WHERE api.Name = @HeaderValue AND ep.TransactionType = @TransactionType AND IsActive = 1";

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@HeaderValue", headerValue);
                        command.Parameters.AddWithValue("@TransactionType", transactionType);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {
                            da.Fill(dataTable);
                        }
                    }

                    return dataTable;
                }


            }
            catch (Exception ex)
            {
                WriteException(ex.InnerException.Message, "GetApiData");

                throw;
            }
        }
    }
}
