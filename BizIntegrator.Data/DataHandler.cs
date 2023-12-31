﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Numerics;

namespace BizIntegrator.Data
{
    public class DataHandler
    {
        public DataTable GetApiData(string transactionType)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                {
                    DataTable dataTable = new DataTable();

                    StringBuilder sb = new StringBuilder();
                    
                    connection.Open();
                    sb.Clear();
                    sb.AppendLine("select api.Id, api.AccountKey, api.[Name], ep.[EndPoint], api.PrivateKey ");
                    sb.AppendLine(",api.Username ");
                    sb.AppendLine(",api.[Password] ");
                    sb.AppendLine(", api.AuthenticationType, api.UseAPIKey ");
                    sb.AppendLine("from APIs api ");
                    sb.AppendLine("inner join APIEndpoints ep  ");
                    sb.AppendLine("on api.Id = ep.API_Id ");
                    sb.AppendLine("where ep.TransactionType = '"+ transactionType + "' and IsActive = 1 ");
                    using (SqlDataAdapter da = new SqlDataAdapter(sb.ToString(), connection))
                    {
                        da.Fill(dataTable);
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
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter dt = new SqlDataAdapter("SELECT isnull(Processed, 0) Processed From Orders Where OrdNo = '" + ordNo + "'", connection);
                    DataSet ds = new DataSet();
                    dt.Fill(ds);

                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["Processed"]) == true)
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
                WriteException(ex.InnerException.Message, "CheckOrders");
                throw;
            }
        }

        public void UpdateProcessedOrder(string ordNo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("Update Orders set Processed = 1 where ordNo = '" + ordNo + "'", connection))
                    {
                        cmd.CommandType = CommandType.Text;
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

        public DataTable GetEanCodes(string VendorNo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                {
                    StringBuilder sb = new StringBuilder();

                    connection.Open();
                    sb.Clear();
                    sb.AppendLine("SELECT api.EanCode SenderGln, sv.EanCode AS RecieverGln ");
                    sb.AppendLine("FROM APIs api ");
                    sb.AppendLine("inner join Suppliers s ");
                    sb.AppendLine("on api.ID = s.API_Id ");
                    sb.AppendLine("inner join SupplierVendors sv ");
                    sb.AppendLine("on s.Id = sv.SupplierId ");
                    sb.AppendLine("Where Vendor = '"+VendorNo+"'");
                    SqlDataAdapter dt = new SqlDataAdapter(sb.ToString(), connection);
                    DataSet ds = new DataSet();
                    dt.Fill(ds);

                    return ds.Tables[0];
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
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter dt = new SqlDataAdapter("SELECT Processed From Orders Where OrdNo = '" + orderNo + "'", connection);
                    DataSet ds = new DataSet();
                    dt.Fill(ds);

                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["Processed"]) == true)
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
                WriteException(ex.InnerException.Message, "ResendOrders");
                throw;
            }
        }

        public void WriteException(String exception, string method)
        {
            try
            {
                RemoveQuotes(exception);

                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO MessageException(ExceptionDate, Exception, Method) VALUES ('" + DateTime.Now + "', '" + RemoveQuotes(exception) + "', '" + method + "')", connection))
                    {
                        cmd.CommandType = CommandType.Text;

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

        public void CreateOrders(string OrdNo,string OrdDate,string OrdDesc,string OrdType,string OrdTerm
                                ,string OrdTermDesc, string OrdStat,string OrderStatus,string Origin, string PromDate
                                ,string CompName,string BranchNo,string BranchName,string BranchAddr1,string BranchAddr2
                                ,string BranchTel,string BranchFax,string BranchEmail,string BranchVat,string VendorRef
                                ,string VendorNo,string VendorName,string VendorAddr1,string VendorAddr2,string VendorSuburb
                                ,string VendorCity,string VendorContact, string TotLines, string TotQty, string TotExcl, string TotTax
                                , string TotVal,string DelivAddr1,string DelivAddr2,string DelivSuburb,string DelivCity
                                ,string BuyerNote,bool ConfirmInd,string CompID,bool ResendOrder,bool Processed)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_CREATE_ORDER", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@OrdNo", SqlDbType.VarChar).Value = OrdNo;
                        cmd.Parameters.Add("@OrdDate", SqlDbType.VarChar).Value = OrdDate;
                        cmd.Parameters.Add("@OrdDesc", SqlDbType.VarChar).Value = OrdDesc;
                        cmd.Parameters.Add("@OrdType", SqlDbType.VarChar).Value = OrdType;
                        cmd.Parameters.Add("@OrdTerm", SqlDbType.VarChar).Value = OrdTerm;
                        cmd.Parameters.Add("@OrdTermDesc", SqlDbType.VarChar).Value = OrdTermDesc;
                        cmd.Parameters.Add("@OrdStat", SqlDbType.Int).Value = OrdStat;
                        cmd.Parameters.Add("@OrderStatus", SqlDbType.VarChar).Value = OrderStatus;
                        cmd.Parameters.Add("@Origin", SqlDbType.VarChar).Value = Origin;
                        cmd.Parameters.Add("@PromDate", SqlDbType.DateTime).Value = PromDate;
                        cmd.Parameters.Add("@CompName", SqlDbType.VarChar).Value = CompName;
                        cmd.Parameters.Add("@BranchNo", SqlDbType.VarChar).Value = BranchNo;
                        cmd.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = BranchName;
                        cmd.Parameters.Add("@BranchAddr1", SqlDbType.VarChar).Value = BranchAddr1;
                        cmd.Parameters.Add("@BranchAddr2", SqlDbType.VarChar).Value = BranchAddr2;
                        cmd.Parameters.Add("@BranchTel", SqlDbType.VarChar).Value = BranchTel;
                        cmd.Parameters.Add("@BranchFax", SqlDbType.VarChar).Value = BranchFax;
                        cmd.Parameters.Add("@BranchEmail", SqlDbType.VarChar).Value = BranchEmail;
                        cmd.Parameters.Add("@BranchVat", SqlDbType.VarChar).Value = BranchVat;
                        cmd.Parameters.Add("@VendorRef", SqlDbType.VarChar).Value = VendorRef;
                        cmd.Parameters.Add("@VendorNo", SqlDbType.VarChar).Value = VendorNo;
                        cmd.Parameters.Add("@VendorName", SqlDbType.VarChar).Value = VendorName;
                        cmd.Parameters.Add("@VendorAddr1", SqlDbType.VarChar).Value = VendorAddr1;
                        cmd.Parameters.Add("@VendorAddr2", SqlDbType.VarChar).Value = VendorAddr2;
                        cmd.Parameters.Add("@VendorSuburb", SqlDbType.VarChar).Value = VendorSuburb;
                        cmd.Parameters.Add("@VendorCity", SqlDbType.VarChar).Value = VendorCity;
                        cmd.Parameters.Add("@VendorContact", SqlDbType.VarChar).Value = VendorContact;
                        cmd.Parameters.Add("@TotLines", SqlDbType.Int).Value = TotLines;
                        cmd.Parameters.Add("@TotQty", SqlDbType.Decimal).Value = TotQty;
                        cmd.Parameters.Add("@TotExcl", SqlDbType.Decimal).Value = TotExcl;
                        cmd.Parameters.Add("@TotTax", SqlDbType.Decimal).Value = TotTax;
                        cmd.Parameters.Add("@TotVal", SqlDbType.Decimal).Value = TotVal;
                        cmd.Parameters.Add("@DelivAddr1", SqlDbType.VarChar).Value = DelivAddr1;
                        cmd.Parameters.Add("@DelivAddr2", SqlDbType.VarChar).Value = DelivAddr2;
                        cmd.Parameters.Add("@DelivSuburb", SqlDbType.VarChar).Value = DelivSuburb;
                        cmd.Parameters.Add("@DelivCity", SqlDbType.VarChar).Value = DelivCity;
                        cmd.Parameters.Add("@ConfirmInd", SqlDbType.Bit).Value = ConfirmInd;
                        cmd.Parameters.Add("@CompID", SqlDbType.VarChar).Value = CompID;
                        cmd.Parameters.Add("@ResendOrder", SqlDbType.Bit).Value = ResendOrder != null ? Convert.ToBoolean(ResendOrder) : false;
                        cmd.Parameters.Add("@Processed", SqlDbType.Bit).Value = Processed != null ? Convert.ToBoolean(Processed) : false;
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

        public void CreateOrderLines(string OrdLn,string OrdNo,string ItemNo,string ItemDesc,string MfrItem, string QtyConv
                                    , string OrdQty,string PurcUom, string PurcUomConv,string TaxCde, string TaxRate
                                    , string UnitPrc, string LineTotExcl, string LineTotTax, string LineTotVal)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_CREATE_ORDER_LINES", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@OrdLn", SqlDbType.Int).Value = Convert.ToInt32(OrdLn);
                        cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = OrdNo;
                        cmd.Parameters.Add("@ItemNo", SqlDbType.VarChar).Value = ItemNo;
                        cmd.Parameters.Add("@ItemDesc", SqlDbType.VarChar).Value = ItemDesc;
                        cmd.Parameters.Add("@MfrItem", SqlDbType.VarChar).Value = MfrItem;
                        cmd.Parameters.Add("@QtyConv", SqlDbType.Decimal).Value = QtyConv;
                        cmd.Parameters.Add("@OrdQty", SqlDbType.Decimal).Value = OrdQty;
                        cmd.Parameters.Add("@PurcUom", SqlDbType.VarChar).Value = PurcUom;
                        cmd.Parameters.Add("@PurcUomConv", SqlDbType.Decimal).Value = PurcUomConv;
                        cmd.Parameters.Add("@TaxCde", SqlDbType.VarChar).Value = TaxCde;
                        cmd.Parameters.Add("@TaxRate", SqlDbType.Decimal).Value = TaxRate;
                        cmd.Parameters.Add("@UnitPrc", SqlDbType.Decimal).Value = UnitPrc;
                        cmd.Parameters.Add("@LineTotExcl", SqlDbType.Decimal).Value = LineTotExcl;
                        cmd.Parameters.Add("@LineTotTax", SqlDbType.Decimal).Value = LineTotTax;
                        cmd.Parameters.Add("@LineTotVal", SqlDbType.Decimal).Value = LineTotVal;
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

        public DataTable GetOrders()
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                {
                    DataTable dataTable = new DataTable();

                    StringBuilder sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine("SELECT [OrdNo], CONVERT(varchar(50), OrdDate, 126) AS [OrdDate],[OrdDesc],[OrdType],[OrdTerm],[OrdTermDesc],[OrdStat] ");
                    sb.AppendLine(",[OrderStatus],[Origin],CONVERT(varchar(8), PromDate, 112) AS [PromDate],[CompName],[BranchNo],[BranchName],[BranchAddr1] ");
                    sb.AppendLine(",[BranchAddr2],[BranchTel],[BranchFax],[BranchEmail],[BranchVat],[VendorRef],[VendorNo] ");
                    sb.AppendLine(",[VendorName],[VendorAddr1],[VendorAddr2],[VendorSuburb],[VendorCity],[VendorContact],[TotLines] ");
                    sb.AppendLine(",[TotQty],[TotExcl],[TotTax],[TotVal],[DelivAddr1],[DelivAddr2],[DelivSuburb],[DelivCity],[BuyerNote] ");
                    sb.AppendLine(",[ConfirmInd],[CompID],[ResendOrder],[Processed] ");
                    sb.AppendLine("FROM [dbo].[Orders] ");
                    sb.AppendLine("WHERE Processed = 0 ");

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
                WriteException(ex.InnerException.Message, "GetOrders");
                throw;
            }
        }

        public DataTable GetOrdersLines(string orderNo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                {
                    DataTable dataTable = new DataTable();

                    StringBuilder sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine("SELECT [OrdLn],[OrderNo],[ItemNo],[ItemDesc],[MfrItem] ");
                    sb.AppendLine(",[QtyConv],[OrdQty],[PurcUom],[PurcUomConv],[TaxCde] ");
                    sb.AppendLine(",[TaxRate],[UnitPrc],[LineTotExcl],[LineTotTax],[LineTotVal] ");
                    sb.AppendLine("FROM [dbo].[OrderLines] ");
                    sb.AppendLine("WHERE OrderNo = '" + orderNo + "' ");

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
    }
}
