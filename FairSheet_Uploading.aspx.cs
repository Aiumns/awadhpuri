using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
public partial class Cargo_FairSheet_Uploading : BasePage
{
    string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    SqlConnection con = null;
    SqlCommand com = null;
    int bit = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }
    protected void cmdExcel_Click(object sender, EventArgs e)
    {
         try
        {
            if (FileUpload1.HasFile)
            {
                string File = FileUpload1.FileName;
                char[] del = { '.' };
                string[] Extensions = File.Split(del);
                bool Isexcel = false;
                foreach (string s in Extensions)
                {
                    if (s == "xlsx" || s == "xls" || s == "XLSX" || s == "XLS" || s == "csv")
                    {
                        Isexcel = true;
                        break;
                    }
                }
                if (!Isexcel)
                {

                }              
              else if (DataUpload())
                    {
                        lblMessage.Text = "File uploaded Successfully!";
                        System.Data.DataTable dtNotInsert = (System.Data.DataTable)ViewState["NotInserted"];
                        if (dtNotInsert.Rows.Count > 0)
                        {
                            ViewState["GenrateReport"] = dtNotInsert;
                            pnlComplete.Visible = true;
                            lblMessage1.Text = "Some Airwaybill No are not inserted";
                            btnReport.Visible = true;
                        }
                    }
                    else
                    {
                        lblMessage.Text =(String)ViewState["error"];
                        ViewState["error"] = null;
                    }
            }
        }
        catch (Exception ex)
        {
            //File.Delete(Server.MapPath("../../CarSlabRate" + ViewState["filename"].ToString()));
           
        }
    }
    protected Boolean DataUpload() 
    {
        try
        {
            string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
            //IExcelDataReader excelReader;
            if (Extension == ".xls" || Extension == ".xlsx")
            {
                //for excel 2003
               // excelReader = ExcelReaderFactory.CreateBinaryReader(FileUpload1.PostedFile.InputStream);
            }
            else
            {
                // for Excel 2007
                //excelReader = ExcelReaderFactory.CreateOpenXmlReader(FileUpload1.PostedFile.InputStream);
            }
            //excelReader.IsFirstRowAsColumnNames = true;
            //DataSet result = excelReader.AsDataSet();

            //if (result.Tables.Count < 1)
            //{
            //    lblMessage1.Text = "Excel sheet can not be NULL";
            //    ViewState["error"] = lblMessage1.Text;
            //    return false;
            //}
           // excelReader.IsFirstRowAsColumnNames = true;
            //DataTable dt = result.Tables[0];

            //======================Updated By:Pradeep Sharma for distinct record on 29 jan 2015==============
           // DataView view = new DataView(dt);
           // DataTable distinctValues = new DataTable();
           // distinctValues = view.ToTable(true, "Agent_Name", "AirWayBill_No", "City_Code");
            //if (distinctValues.Rows.Count < 1 || dt == null)
            //{
            //    lblMessage1.Text = "Excel sheet can not be NULL";
            //    if (ViewState["error"] != "" || ViewState["error"] == null)
            //    {
            //        ViewState["error"] = lblMessage1.Text;
            //    }
            //    else
            //    {
            //        lblMessage1.Text = "Invalid Data, Please Check!";

            //    }
            //    return false;

            //}
           // ViewState["BulkStock"] = distinctValues;
            using (SqlConnection sqlCon = new SqlConnection(strCon))
            {
                sqlCon.Open();
                //com = new SqlCommand("SP_AKMYEAST_BulkStockAdd", sqlCon);
                com = new SqlCommand("SP_FairBulkSUpload", sqlCon);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@tbltype_FairBulkSUpload", SqlDbType.Structured).Value = null;
                //com.Parameters.Add("@bit", SqlDbType.Int).Value = bit;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = com;
                DataSet ds2 = new DataSet();
                adp.Fill(ds2);
                DataTable dtNotInserted = ds2.Tables[4];
                ViewState["NotInserted"] = dtNotInserted;
                return true;
            }
        }
        catch (SqlException ex)
        {
            int l = ex.Number;
            if (l == 515)
            {
                ViewState["error"] = "Please Enter correct data in city or Agent Name";
            }
            return false;
        }
    }
   

    //protected void btnReport_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        // DataTable dt = (DataTable)ViewState["GenrateReport"];
    //        //GenerateXLS("AirWayBill_No", dt);
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
    #region Function to generate excel on response via datatable
    //public void GenerateXLS(string pFileName, DataTable dtSource)
    //{
    //    HttpResponse response = HttpContext.Current.Response;
    //    response.Clear();
    //    response.Charset = "";
    //    // set the response mime type for excel 
    //    if ((pFileName).ToLower().Contains(".xlsx"))
    //    {
    //        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    //    }
    //    else
    //    {
    //        response.ContentType = "application/vnd.ms-excel";
    //    }
    //    response.AddHeader("Content-Disposition", "attachment;filename=\"" + pFileName + "\"");

    //    // create a string writer 
    //    using (StringWriter sw = new StringWriter())
    //    {
    //        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
    //        {
    //            // instantiate a datagrid 
    //            GridView gvExport = new GridView();
    //            gvExport.DataSource = dtSource;
    //            gvExport.DataBind();
    //            //(start): require for date format issue
    //            HtmlTextWriter hw = new HtmlTextWriter(sw);
    //            foreach (GridViewRow r in gvExport.Rows)
    //            {
    //                if (r.RowType == DataControlRowType.DataRow)
    //                {
    //                    for (int columnIndex = 0; columnIndex < r.Cells.Count; columnIndex++)
    //                    {
    //                        r.Cells[columnIndex].Attributes.Add("class", "text");
    //                    }
    //                }
    //            }
    //            //(end): require for date format issue
    //            gvExport.RenderControl(htw);
    //            //(start): require for date format issue
    //            System.Text.StringBuilder style = new System.Text.StringBuilder();
    //            style.Append("<style>");
    //            style.Append("." + "text" + " { mso-number-format:" + "\\@;" + " }");
    //            style.Append("</style>");
    //            response.Clear();
    //            Response.Buffer = true;
    //            //response.Charset = "";
    //            //response.Write(sw.ToString());
    //            Response.Write(style.ToString());
    //            Response.Output.Write(sw.ToString());
    //            Response.Flush();
    //            //(end): require for date format issue
    //            try
    //            {
    //                response.End();
    //            }
    //            catch (Exception er)
    //            {
    //                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + er.Message.ToString().Replace("'", "") + "');</script>");
    //            }

    //        }
    //    }
    //}
    #endregion
    protected void btnSample_Click(object sender, EventArgs e)
    {
        // if you have Excel 2007 uncomment this line of code
        //  string excelConnectionString =string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0",path);

        string ExcelContentType = "application/vnd.ms-excel";
        string Excel2010ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        // string strdt = "23/01/2019";
        // DateTime Updatedondt = DateTime.Parse(strdt.ToString());

        if (FileUpload1.HasFile)
        {
            //Check the Content Type of the file
            if (FileUpload1.PostedFile.ContentType == ExcelContentType || FileUpload1.PostedFile.ContentType == Excel2010ContentType)
            {
                try
                {
                    //Save file pathhttp://localhost:51585/Cfi.App.Pace.WebUI/Cargo/Excel/
                    string path = string.Concat(Server.MapPath("~/Cargo/Excel/"), FileUpload1.FileName);
                    //Save File as Temp then you can delete it if you want
                    FileUpload1.SaveAs(path);
                    //string path = @"C:\Users\Johnney\Desktop\ExcelData.xls";
                    //For Office Excel 2010  please take a look to the followng link  http://social.msdn.microsoft.com/Forums/en-US/exceldev/thread/0f03c2de-3ee2-475f-b6a2-f4efb97de302/#ae1e6748-297d-4c6e-8f1e-8108f438e62e
                    string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);

                    // Create Connection to Excel Workbook
                    using (OleDbConnection connection = new OleDbConnection(excelConnectionString))
                    {
                        connection.Open();
                        string sheet1 = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();

                        DataTable dtExcelData = new DataTable();
                        //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
                        dtExcelData.Columns.AddRange(new DataColumn[8]
                                {
                                    new DataColumn("Exname", typeof(string)),
                                    new DataColumn("Dest", typeof(string)),                                   
                                    new DataColumn("Country",typeof(string)),
                                    new DataColumn("Source",typeof(string)),
                                    new DataColumn("Date", typeof(DateTime)),                               
                                    new DataColumn("Compbrsno",typeof(int)),
                                    new DataColumn("UpdatedOn",typeof(DateTime)),
                                    new DataColumn("UpdatedBy",typeof(string))
                                });
                        dtExcelData.Columns["Compbrsno"].DefaultValue =Convert.ToInt32(Session["CompBrSNo"].ToString());
                        dtExcelData.Columns["UpdatedOn"].DefaultValue = DateTime.Now.ToString("yyyy-MM-dd");
                        dtExcelData.Columns["UpdatedBy"].DefaultValue = Convert.ToString(Session["EmailID"]);

                        //string oldDate = "2019/01/24";
                        //DateTime defaultdate = DateTime.ParseExact(oldDate, "yyyy-MM-dd", null);
                        //dtExcelData.Columns["Date2"].DefaultValue = oldDate;// DateTime.ParseExact(oldDate, "yyyy-MM-dd", null);
                        //OleDbCommand command = new OleDbCommand("Select  *  FROM [Sheet1$]", connection);
                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", connection))
                        {
                            oda.Fill(dtExcelData);
                        }
                        connection.Close();

                        string consString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(consString))
                        {
                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                            {
                                //Set the database table name
                                bulkCopy.DestinationTableName = "dbo.FairDetails_EX";
                                //[OPTIONAL]: Map the Excel columns with that of the database table                              
                                bulkCopy.ColumnMappings.Add("Exname", "Exname");
                                bulkCopy.ColumnMappings.Add("Dest", "Dest");
                                bulkCopy.ColumnMappings.Add("Country", "Country");
                                bulkCopy.ColumnMappings.Add("Source", "Source");
                                bulkCopy.ColumnMappings.Add("Date", "Date");
                                bulkCopy.ColumnMappings.Add("Compbrsno", "Compbrsno");
                                bulkCopy.ColumnMappings.Add("UpdatedOn", "UpdatedOn");
                                bulkCopy.ColumnMappings.Add("UpdatedBy", "UpdatedBy");                            
                                con.Open();
                                bulkCopy.WriteToServer(dtExcelData);
                                con.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Label1.Text = ex.Message;
                }
            }
        }
    }
}