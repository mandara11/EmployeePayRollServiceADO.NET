using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayRollServiceADO.NET
{
    public class EmployeeRepo
    {
        public static string connectionstring = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=payroll_services;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionstring);

        //UC1:- Ability to create a payroll service database and have C# program connect to database.
        public bool AddEmployee()
        {
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("SpAddEmployeeDetails", this.connection);
                    command.CommandType=System.Data.CommandType.StoredProcedure; //execute this stored procedure
                    command.Parameters.AddWithValue("@EmployeeName", employeeModel.EmployeeName);
                    command.Parameters.AddWithValue("@PhoneNumber", employeeModel.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", employeeModel.Address);
                    command.Parameters.AddWithValue("@Department", employeeModel.Department);
                    command.Parameters.AddWithValue("@Gender", employeeModel.Gender);
                    command.Parameters.AddWithValue("@BasicPay", employeeModel.BasicPay);
                    command.Parameters.AddWithValue("@Deductions", employeeModel.Deductions);
                    command.Parameters.AddWithValue("@TaxablePay", employeeModel.TaxablePay);
                    command.Parameters.AddWithValue("@Tax", employeeModel.Tax);
                    command.Parameters.AddWithValue("@NetPay", employeeModel.NetPay);
                    command.Parameters.AddWithValue("@StartDate", DateTime.Now);
                    command.Parameters.AddWithValue("@City", employeeModel.City);
                    command.Parameters.AddWithValue("@Country", employeeModel.Country);

                    this.connection.Open();
                    var result = command.ExecuteNonQuery();
                    this.connection.Close();
                    if (result!=0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                this.connection.Close();
            }
        }
        // UC2:- Ability for Employee Payroll Service to retrieve the Employee Payroll from the Database.
        public void GetAllEmployees()
        {
            try
            {
                EmployeeModel employeeModel = new EmployeeModel(); //Creating Employeemodel class object
                using (this.connection)
                {
                    string query = @"select EmployeeID,EmployeeName,PhoneNumber,Address,Department,Gender,BasicPay,Deductions,TaxablePay,Tax,NetPay,StartDate,City,Country";
                    SqlCommand cmd = new SqlCommand(query, connection); //accept query and connection

                    this.connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader(); // Execute sqlDataReader to fetching all records
                    if (dr.HasRows) // Checking datareader has rows or not.
                    {
                        while (dr.Read()) //using while loop for read multiple rows.
                        {
                            employeeModel.EmployeeID = dr.GetInt32(0);
                            employeeModel.EmployeeName = dr.GetString(1);
                            employeeModel.PhoneNumber = dr.GetString(2);
                            employeeModel.Address = dr.GetString(3);
                            employeeModel.Department = dr.GetString(4);
                            employeeModel.Gender = dr.GetChar(5);
                            employeeModel.Deductions = dr.GetDouble(7);
                            employeeModel.TaxablePay = dr.GetDouble(8);
                            employeeModel.Tax = dr.GetDouble(9);
                            employeeModel.NetPay = dr.GetDouble(10);
                            employeeModel.StartDate = dr.GetDateTime(11);
                            employeeModel.City = dr.GetString(12);
                            employeeModel.Country = dr.GetString(13);

                            Console.WriteLine(employeeModel.EmployeeID+" "+ employeeModel.EmployeeName);
                            Console.WriteLine("-------------");
                        }

                    }
                    else
                    {
                        Console.WriteLine("No Data found");
                    }
                    dr.Close();

                    this.connection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                this.connection.Close(); //Always ensuring the closing of the connection
            }
        }

        //UC3:- Ability to update the salary i.e. the base pay for Employee 
        public bool UpdateBasicPay(string EmployeeName, double BasicPay)
        {
            try
            {
                using (connection)
                {
                    connection.Open();
                    string query = @"update dbo.employee_payroll set BasicPay=@inputBasicPay where EmployeeName=@inputEmployeeName";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@inputBasicPay", BasicPay); //parameters transact SQl stament or store procedure
                    command.Parameters.AddWithValue("@inputEmployeeName", EmployeeName);
                    var result = command.ExecuteNonQuery(); //ExecuteNonQuery and store result
                    Console.WriteLine("Record Update Successfully");
                    connection.Close();
                    GetAllEmployees(); // call method and show record
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        //UC4:- Ability to update the salary i.e. the base pay for Employee.
        public double UpdatedSalaryFromDatabase(string EmployeeName)
        {

            string cconnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=payroll_services;Integrated Security=True"; //Specifying the connection string from the sql server connection.

            SqlConnection connection = new SqlConnection(cconnectionString);
            try
            {
                using (connection)
                {
                    string query = @"select BasicPay from dbo.employee_payroll where EmployeeName=@inputEmployeeName";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@inputEmployeeName", EmployeeName);
                    return (double)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        /*UC5:- Ability to retrieve all employees who have joined in a particular data range from the
                 payroll service database*/
        public void EmployeesFromForDateRange(string Date)
        {
            EmployeeModel employeemodel = new EmployeeModel(); //Creating Employee model class object

            try
            {
                using (connection)
                {
                    connection.Open(); //open connection
                    string query = $@"select * from dbo.employee_payroll where StartDate between cast('{Date}' as date) and cast(getdate() as date)";
                    SqlCommand command = new SqlCommand(query, connection); //accept query and connection

                    SqlDataReader reader = command.ExecuteReader(); // Execute sqlDataReader to fetching all records

                    if (reader.HasRows)     // Checking datareader has rows or not.               
                    {
                        // Console.WriteLine("EmployeeId, EmployeeName, PhoneNumber, Address, Department, Gender, BasicPay, Deductions, TaxablePay, TaxablePay, Tax, NetPay, StartDate, City, Country");                                            
                        while (reader.Read()) //using while loop for read multiple rows.
                        {
                            employeemodel.EmployeeID = reader.GetInt32(0);
                            employeemodel.EmployeeName = reader.GetString(1);
                            employeemodel.PhoneNumber = reader.GetString(2);
                            employeemodel.Address = reader.GetString(3);
                            employeemodel.Department = reader.GetString(4);
                            employeemodel.Gender = reader.GetChar(5);
                            employeemodel.BasicPay = reader.GetDouble(6);
                            employeemodel.Deductions = reader.GetDouble(7);
                            employeemodel.TaxablePay = reader.GetDouble(8);
                            employeemodel.Tax = reader.GetDouble(9);
                            employeemodel.NetPay = reader.GetDouble(10);
                            employeemodel.StartDate = reader.GetDateTime(11);
                            employeemodel.City = reader.GetString(12);
                            employeemodel.Country = reader.GetString(13);
                            Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", employeemodel.EmployeeID,
                                employeemodel.EmployeeName, employeemodel.PhoneNumber, employeemodel.Address, employeemodel.Department,
                                employeemodel.Gender, employeemodel.BasicPay, employeemodel.Deductions, employeemodel.TaxablePay,
                                employeemodel.Tax, employeemodel.NetPay, employeemodel.StartDate, employeemodel.City, employeemodel.Country);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{Date} Record Not found on The Table "); //print 
                    }
                    reader.Close(); //close                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                this.connection.Close(); //Always ensuring the closing of the connection
            }
        }
    }
}
