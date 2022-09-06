namespace EmployeePayRollServiceADO.NET
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to EmployeePayRollServiceADO.NET");

            EmployeeRepo repo = new EmployeeRepo();
            EmployeeModel employeeModel = new EmployeeModel();

            employeeModel.EmployeeName =  "Mandara";
            employeeModel.PhoneNumber = "9110264684";
            employeeModel.Address = "Banglore";
            employeeModel.Department = "Hr";
            employeeModel.Gender = 'F';
            employeeModel.BasicPay = 323400.00;
            employeeModel.Deductions = 2500.00;
            employeeModel.TaxablePay = 200.00;
            employeeModel.Tax = 300.00;
            employeeModel.NetPay = 25000.00;
            employeeModel.City = "Banglore";
            employeeModel.Country = "India";

            repo.AddEmployee(); //UC1
            repo.GetAllEmployees(); //UC2
            repo.UpdateBasicPay("Terisa", 3000000);//UC3 update BasicPay where name is Terisa table
            repo.UpdatedSalaryFromDatabase("Terisa"); //UC4
        }
    }
}