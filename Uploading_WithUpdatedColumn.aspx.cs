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
using System.Text;
public partial class Cargo_Uploading_WithUpdatedColumn :BasePage
{
    string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    SqlConnection con = null;
    SqlCommand com = null;
    static int bit = 0; //every time changed 
    readonly int bit2 = 0; //once changed 
    const int bit3 = 0;   //never changed 
    //bit++;
    //bit2++;
    //bit3++;
    public Cargo_Uploading_WithUpdatedColumn()
    { 
          
    }
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    public int Uploadfile(object bit, int bit2)
    {
         return Convert.ToInt32(bit) + Convert.ToInt32(bit2) + Convert.ToInt32(bit3);         
         //int a= (Int32)(bit); // can change
         //a++;
         //Response.Write(bit); // access
         //// bit2++;// can not change
         //Response.Write(bit2);
         //// bit3++;// can not change
         //Response.Write(bit3); // access
         //return bit + bit2 + bit3; 
    }
    protected void btnSample_Click(object sender, EventArgs e)
    {
        StringBuilder sqlparam =new StringBuilder();
        int IntReturn = 0;
        string ExcelContentType = "application/vnd.ms-excel";
        string Excel2010ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";      
        // DateTime Updatedondt = DateTime.Parse(strdt.ToString());
        if (FileUpload1.HasFile)
        {
            //Check the Content Type of the file
            if (FileUpload1.PostedFile.ContentType == ExcelContentType || FileUpload1.PostedFile.ContentType == Excel2010ContentType)
            {
                try
                {                  
                    string path = string.Concat(Server.MapPath("~/Cargo/Excel/"), FileUpload1.FileName);                 
                    FileUpload1.SaveAs(path);                 
                    string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
                    // Create Connection to Excel Workbook
                    using (OleDbConnection connection = new OleDbConnection(excelConnectionString))
                    {
                        connection.Open();
                        string sheet1 = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                        DataTable dt2 = new DataTable();
                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", connection))
                         {
                            oda.Fill(dt2);
                         }
                        DataTable dtExcelData = new DataTable();                     
                        //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
                        if (dt2.Rows.Count > 0)
                         {
                            //Start string builder for dynamic table type
                            sqlparam.Append(@"CREATE TYPE [db_owner].[Dynamic_tbltype] AS TABLE(");
                            //sqlparam.Append("CREATE TYPE [dbo].[Dynamic_tbltype] AS TABLE(");
                            foreach (DataColumn col in dt2.Columns)
                            {
                                dtExcelData.Columns.Add(col.ColumnName, typeof(string));                               
                                sqlparam.Append(col.ColumnName + "  varchar(300) NULL,");                              
                            }
                            dtExcelData.Columns.Add("Compbrsno", typeof(int));
                            dtExcelData.Columns.Add("UpdatedOn", typeof(DateTime));
                            dtExcelData.Columns.Add("UpdatedBy", typeof(string));
                            dtExcelData.Columns.Add("remarks", typeof(string));
                            //Add default column to table type
                            sqlparam.Append("Compbrsno" + "  int NULL,");
                            sqlparam.Append("UpdatedOn" + "  date NULL,");
                            sqlparam.Append("UpdatedBy" + "  varchar(300) NULL,");
                            sqlparam.Append("remarks" + "  varchar(300) NULL)");
                            //End string builder for dynamic table type
                            //Set default Value To dynamic table
                            dtExcelData.Columns["Compbrsno"].DefaultValue = Convert.ToInt32(Session["CompBrSNo"].ToString());
                            dtExcelData.Columns["UpdatedOn"].DefaultValue = DateTime.Now.ToString("yyyy-MM-dd");
                            dtExcelData.Columns["UpdatedBy"].DefaultValue = Convert.ToString(Session["EmailID"]);       
                        }                                   
                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", connection))
                        {
                            oda.Fill(dtExcelData);
                        }  
                        connection.Close();
                        using (SqlConnection sqlCon = new SqlConnection(strCon))
                        {
                            sqlCon.Open();
                            com = new SqlCommand("CreateAnd_Drop_type", sqlCon);
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.Add("@sql", SqlDbType.NVarChar).Value = Convert.ToString(sqlparam);
                            //com.Parameters.Add("@tbltype_DynamicExcelSheet", SqlDbType.Structured).Value = dtExcelData;

                            SqlParameter RetParam = new SqlParameter("ReturnValue", DBNull.Value);
                            RetParam.Direction = ParameterDirection.ReturnValue;
                            com.Parameters.Add(RetParam);

                            com.ExecuteNonQuery();
                            IntReturn = Convert.ToInt32(com.Parameters["ReturnValue"].Value);
                        }
                        if (IntReturn == 0)
                        {
                            using (SqlConnection sqlCon = new SqlConnection(strCon))
                            {
                                sqlCon.Open();
                                //com = new SqlCommand("SP_AKMYEAST_BulkStockAdd", sqlCon);
                                com = new SqlCommand("SP_DynamicExcelSheet_Uploading", sqlCon);
                                com.CommandType = CommandType.StoredProcedure;
                                com.Parameters.Add("@tbltype_DynamicExcelSheet", SqlDbType.Structured).Value = dtExcelData;
                                com.ExecuteNonQuery();
                            }
                        } 

                        //////Using transaction  
                        //SqlConnection sqlCon = new SqlConnection(strCon);
                        //SqlTransaction trans;                        
                        //sqlCon.Open();
                        //trans = sqlCon.BeginTransaction();
                        //try
                        //{
                        //    SqlCommand cmd= new SqlCommand("CreateAnd_Drop_type", sqlCon, trans);
                        //    cmd.Parameters.AddWithValue("@sql", SqlDbType.NVarChar).Value = Convert.ToString(sqlparam);
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.ExecuteNonQuery();

                        //    SqlCommand cmd2 = new SqlCommand("SP_DynamicExcelSheet_Uploading", sqlCon, trans);
                        //    cmd2.Parameters.Add("@tbltype_DynamicExcelSheet", SqlDbType.Structured).Value = dtExcelData;
                        //    cmd2.CommandType = CommandType.StoredProcedure;
                        //    cmd2.ExecuteNonQuery();
                        //    sqlCon.Close();

                        //    trans.Commit();
                        //}
                        //catch (Exception)
                        //{
                        //    trans.Rollback();
                        //}
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