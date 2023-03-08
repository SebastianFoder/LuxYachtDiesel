using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public class ClassLuxYachtDieselDB : ClassDbCon
    {
        static string sqlConStr = @"Server=(localdb)\MSSQLLocalDB;DataBase=LuxYachtDieselDB;Trusted_Connection=True";

        public ClassLuxYachtDieselDB()
        {
            SetCon(sqlConStr);
        }

        /// <summary>
        /// Retrieves a list of all active customers from the database with their corresponding 
        /// country and currency information, sorted by name and then country.
        /// </summary>
        /// <returns>A list of ClassCustomer objects representing active customers.</returns>
        public List<ClassCustomer> GetAllCustomersFromDB()
        {
            List<ClassCustomer> res = new List<ClassCustomer>();

            try
            {
                string sqlQuery = "SELECT CustomersEnabled.isActive, Customers.Id, Customers.name, Customers.address, Customers.city, " +
                    "Customers.postalCode, Customers.phone, Customers.mailAdr, CountryCurrency.Id AS countryId, CountryCurrency.countryCode, " +
                    "CountryCurrency.currency, CountryCurrency.currencyCode, CountryCurrency.country " +
                    "FROM Customers LEFT OUTER JOIN CountryCurrency ON Customers.country = CountryCurrency.Id " +
                    "LEFT OUTER JOIN CustomersEnabled ON Customers.Id = CustomersEnabled.customerId " +
                    "WHERE (CustomersEnabled.isActive = 1)";
                using (DataTable dataTable = DbReturnDataTable(sqlQuery))
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        ClassCustomer cc = new ClassCustomer();
                        cc.Id = Convert.ToInt32(row["Id"]);
                        cc.name = row["name"].ToString();
                        cc.address = row["address"].ToString();
                        cc.city = row["city"].ToString();
                        cc.postalCode = row["postalCode"].ToString();
                        ClassCountry cco = new ClassCountry();
                        cco.Id = Convert.ToInt32(row["countryId"]);
                        cco.country = row["country"].ToString();
                        cco.countryCode = row["countryCode"].ToString();
                        cco.currency = row["currency"].ToString();
                        cco.currencyCode = row["currencyCode"].ToString();
                        cc.country = cco;
                        cc.phone = row["phone"].ToString();
                        cc.mailAdr = row["mailAdr"].ToString();
                        cc.isActive = Convert.ToBoolean(row["isActive"]);
                        res.Add(cc);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res.OrderBy(c => c.name).ThenBy(c => c.country).ToList();
        }

        /// <summary>
        /// Saves a new customer record to the database, along with a corresponding record 
        /// in the CustomersEnabled table to indicate whether the customer is active or not.
        /// </summary>
        /// <param name="inCustomer">The ClassCustomer representing the customer to be saved.</param>
        /// <returns>The integer ID of the new customer record in the Customers table.</returns>
        public int SaveCustomerinDB(ClassCustomer inCustomer)
        {
            int res = 0;
            string sqlQuery = "INSERT INTO Customers " +
                "(name, address, city, postalCode, country, phone, mailAdr) " +
                "VALUES (@name, @address, @city, @postalCode, @country, @phone, @mailAdr) " +
                "SELECT SCOPE_IDENTITY()";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = inCustomer.name;
                    cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = inCustomer.address;
                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = inCustomer.city;
                    cmd.Parameters.Add("@postalCode", SqlDbType.NVarChar).Value = inCustomer.postalCode;
                    cmd.Parameters.Add("@country", SqlDbType.Int).Value = inCustomer.country.Id;
                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = inCustomer.phone;
                    cmd.Parameters.Add("@mailAdr", SqlDbType.NVarChar).Value = inCustomer.mailAdr;

                    res = ExecuteScalarInDB(cmd);
                }
                // Makes a new query to use for the 2nd table
                // using the SELECT SCOPE_IDENTITY() we get the new id
                // which we can use in the new table to show if a user is active
                sqlQuery = "INSERT INTO CustomersEnabled " +
                "(customerId, isActive) " +
                "VALUES (@customerId, @isActive)";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@customerId", SqlDbType.Int).Value = res;
                    cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = inCustomer.isActive;

                    ExecuteNonQuery(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }

        /// <summary>
        /// Updates an existing customer record in the database and also updates the corresponding record
        /// in the CustomersEnabled table to reflect the customer's active status.
        /// </summary>
        /// <param name="inCustomer">The ClassCustomer representing the customer to be updated.</param>
        /// <returns>An integer representing the number of rows affected by the update operation.</returns>
        public int UpdateCustomerInDB(ClassCustomer inCustomer)
        {
            int res = 0;
            string sqlQuery = "UPDATE Customers " +
                "SET " +
                "name = @name, " +
                "address = @address, " +
                "city = @city, " +
                "postalCode = @postalCode, " +
                "country = @country, " +
                "phone = @phone, " +
                "mailAdr = @mailAdr " +
                "WHERE id = @id " +
                "UPDATE CustomersEnabled " +
                "SET " +
                "isActive = @isActive " +
                "WHERE customerId = @id";
            try
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = inCustomer.name;
                    cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = inCustomer.address;
                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = inCustomer.city;
                    cmd.Parameters.Add("@postalCode", SqlDbType.NVarChar).Value = inCustomer.postalCode;
                    cmd.Parameters.Add("@country", SqlDbType.Int).Value = inCustomer.country.Id;
                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = inCustomer.phone;
                    cmd.Parameters.Add("@mailAdr", SqlDbType.NVarChar).Value = inCustomer.mailAdr;
                    cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = inCustomer.isActive;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = inCustomer.Id;

                    res = ExecuteNonQuery(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }

        /// <summary>
        /// Retrieves a list of all active suppliers from the database, including their company information
        /// and corresponding country information. The list is ordered by the supplier's company name.
        /// </summary>
        /// <returns>A List of ClassSuppliers representing the active suppliers in the database.</returns>
        public List<ClassSupplier> GetAllSuppliersFromDB()
        {
            List<ClassSupplier> res = new List<ClassSupplier>();

            try
            {
                string sqlQuery = "SELECT Supplier.Id, Supplier.companyName, Supplier.contactName, Supplier.address, Supplier.city, " +
                    "Supplier.postalCode, Supplier.phone, Supplier.mailAdr, SupplierEnabled.isActive, " +
                    "CountryCurrency.Id AS countryId, CountryCurrency.country, CountryCurrency.countryCode, CountryCurrency.currency, " +
                    "CountryCurrency.currencyCode " +
                    "FROM CountryCurrency INNER JOIN Supplier ON CountryCurrency.Id = Supplier.country " +
                    "INNER JOIN SupplierEnabled ON Supplier.Id = SupplierEnabled.supplierId " +
                    "WHERE (SupplierEnabled.isActive = 1)";
                using (DataTable dataTable = DbReturnDataTable(sqlQuery))
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        ClassSupplier cs = new ClassSupplier();
                        cs.Id = Convert.ToInt32(row["Id"]);
                        cs.firmName = row["companyName"].ToString();
                        cs.contactName = row["contactName"].ToString();
                        cs.address = row["address"].ToString();
                        cs.city = row["city"].ToString();
                        cs.postalCode = row["postalCode"].ToString();
                        ClassCountry cco = new ClassCountry();
                        cco.Id = Convert.ToInt32(row["countryId"]);
                        cco.country = row["country"].ToString();
                        cco.countryCode = row["countryCode"].ToString();
                        cco.currency = row["currency"].ToString();
                        cco.currencyCode = row["currencyCode"].ToString();
                        cs.country = cco;
                        cs.phone = row["phone"].ToString();
                        cs.mailAdr = row["mailAdr"].ToString();
                        cs.isEnabled = Convert.ToBoolean(row["isActive"]);
                        res.Add(cs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res.OrderBy(s => s.firmName).ToList();
        }

        /// <summary>
        /// Saves a new supplier record to the database, along with a corresponding record 
        /// in the SupplierEnabled table to indicate whether the supplier is active or not.
        /// </summary>
        /// <param name="inSupplier">The ClassSupplier representing the supplier to be saved.</param>
        /// <returns>The integer ID of the new supplier record in the Supplier table.</returns>
        public int SaveSupplierInDB(ClassSupplier inSupplier)
        {
            int res = 0;
            string sqlQuery = "INSERT INTO Supplier " +
                "(companyName, contactName, address, city, postalCode, country, phone, mailAdr) " +
                "VALUES (@companyName, @contactName, @address, @city, @postalCode, @country, @phone, @mailAdr) " +
                "SELECT SCOPE_IDENTITY()";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@companyName", SqlDbType.NVarChar).Value = inSupplier.firmName;
                    cmd.Parameters.Add("@contactName", SqlDbType.NVarChar).Value = inSupplier.contactName;
                    cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = inSupplier.address;
                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = inSupplier.city;
                    cmd.Parameters.Add("@postalCode", SqlDbType.NVarChar).Value = inSupplier.postalCode;
                    cmd.Parameters.Add("@country", SqlDbType.Int).Value = inSupplier.country.Id;
                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = inSupplier.phone;
                    cmd.Parameters.Add("@mailAdr", SqlDbType.NVarChar).Value = inSupplier.mailAdr;

                    res = ExecuteScalarInDB(cmd);
                }
                // Makes a new query to use for the 2nd table
                // using the SELECT SCOPE_IDENTITY() we get the new id
                // which we can use in the new table to show if a user is active
                sqlQuery = "INSERT INTO SupplierEnabled " +
                "(supplierId, isActive) " +
                "VALUES (@supplierId, @isActive)";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@supplierId", SqlDbType.Int).Value = res;
                    cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = inSupplier.isEnabled;

                    ExecuteNonQuery(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }

        /// <summary>
        /// Updates an supplier customer record in the database and also updates the corresponding record
        /// in the SupplierEnabled table to reflect the supplier's active status.
        /// </summary>
        /// <param name="inSupplier">The ClassSupplier to use for updating the record.</param>
        /// <returns>The number of rows affected in the database.</returns>
        public int UpdateSupplierInDB(ClassSupplier inSupplier)
        {
            int res = 0;
            string sqlQuery = "UPDATE Supplier " +
                "SET " +
                "companyName = @companyName, " +
                "contactName = @contactName, " +
                "address = @address, " +
                "city = @city, " +
                "postalCode = @postalCode, " +
                "country = @country, " +
                "phone = @phone, " +
                "mailAdr = @mailAdr " +
                "WHERE id = @id " +
                "UPDATE SupplierEnabled " +
                "SET " +
                "isActive = @isActive " +
                "WHERE supplierId = @id";
            try
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@companyName", SqlDbType.NVarChar).Value = inSupplier.firmName;
                    cmd.Parameters.Add("@contactName", SqlDbType.NVarChar).Value = inSupplier.contactName;
                    cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = inSupplier.address;
                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = inSupplier.city;
                    cmd.Parameters.Add("@postalCode", SqlDbType.NVarChar).Value = inSupplier.postalCode;
                    cmd.Parameters.Add("@country", SqlDbType.Int).Value = inSupplier.country.Id;
                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = inSupplier.phone;
                    cmd.Parameters.Add("@mailAdr", SqlDbType.NVarChar).Value = inSupplier.mailAdr;
                    cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = inSupplier.isEnabled;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = inSupplier.Id;

                    res = ExecuteNonQuery(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }

        /// <summary>
        /// Saves an order to the database.
        /// </summary>
        /// <param name="inOrder">The order to save.</param>
        /// <returns>The ID of the saved order.</returns>
        public int SaveOrderToDB(ClassOrder inOrder)
        {
            int res = 0;
            string sqlQuery = "INSERT INTO [Order] " +
                "(customerId, supplierId, dieselVolume, orderDate, dieselPrice, customerRate, supplierRate, ownProfit) " +
                "VALUES (@customerId, @supplierId, @dieselVolume, @orderDate, @dieselPrice, @customerRate, @supplierRate, @ownProfit) " +
                "SELECT SCOPE_IDENTITY()";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@customerId", SqlDbType.Int).Value = inOrder.customer.Id;
                    cmd.Parameters.Add("@supplierId", SqlDbType.Int).Value = inOrder.supplier.Id;
                    cmd.Parameters.Add("@dieselVolume", SqlDbType.Int).Value = inOrder.volume;
                    cmd.Parameters.Add("@orderDate", SqlDbType.Date).Value = inOrder.date;
                    cmd.Parameters.Add("@dieselPrice", SqlDbType.Decimal).Value = inOrder.price;
                    cmd.Parameters.Add("@customerRate", SqlDbType.Decimal).Value = inOrder.customerRate;
                    cmd.Parameters.Add("@supplierRate", SqlDbType.Decimal).Value = inOrder.supplierRate;
                    cmd.Parameters.Add("@ownProfit", SqlDbType.Decimal).Value = inOrder.ownProfit;

                    res = ExecuteScalarInDB(cmd);
                }
                sqlQuery = "INSERT INTO OrderOwnRate " +
                "(orderId, ownRate) " +
                "VALUES (@orderId, @ownRate)";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = res;
                    cmd.Parameters.Add("@ownRate", SqlDbType.Decimal).Value = inOrder.ownRate;

                    res = ExecuteNonQuery(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }

        /// <summary>
        /// Retrieves a list of all diesel prices from the database and orders them by date and ID in descending order.
        /// </summary>
        /// <returns>A list of ClassDieselPrices representing the diesel prices.</returns>
        public List<ClassDieselPrice> GetAllOilPriceFromDB()
        {
            List<ClassDieselPrice> res = new List<ClassDieselPrice>();

            try
            {
                string sqlQuery = "SELECT Id, date, price FROM DieselPrice";
                using (DataTable dataTable = DbReturnDataTable(sqlQuery))
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        ClassDieselPrice cdp = new ClassDieselPrice();
                        cdp.Id = Convert.ToInt32(row["Id"]);
                        cdp.date = Convert.ToDateTime(row["date"]);
                        cdp.price = Convert.ToDouble(row["price"]);
                        res.Add(cdp);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res.OrderByDescending(cdp => cdp.date).ThenByDescending(cdp => cdp.Id).ToList();
        }

        /// <summary>
        /// Saves the diesel price to the database with the current date.
        /// </summary>
        /// <param name="inPrice">The price to be saved to the database.</param>
        /// <returns>The ID of the saved diesel price.</returns>
        public int SaveDieselPrice(double inPrice)
        {
            int res = 0;
            string sqlQuery = "INSERT INTO DieselPrice " +
                "(date, price) " +
                "VALUES (@date, @price) " +
                "SELECT SCOPE_IDENTITY()";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@date", SqlDbType.Date).Value = DateTime.Today;
                    cmd.Parameters.Add("@price", SqlDbType.Money).Value = inPrice;

                    res = ExecuteScalarInDB(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }

        /// <summary>
        /// Retrieves all countries from the database.
        /// </summary>
        /// <returns>A list of ClassCountry objects representing countries.</returns>
        public List<ClassCountry> GetAllCountriesFromDB()
        {
            List<ClassCountry> res = new List<ClassCountry>();

            try
            {
                string sqlQuery = "SELECT Id, country, countryCode, currency, currencyCode FROM CountryCurrency";
                using (DataTable dataTable = DbReturnDataTable(sqlQuery))
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        ClassCountry cc = new ClassCountry();
                        cc.Id = Convert.ToInt32(row["Id"]);
                        cc.country = row["country"].ToString();
                        cc.countryCode = row["countryCode"].ToString();
                        cc.currency = row["currency"].ToString();
                        cc.currencyCode = row["currencyCode"].ToString();
                        res.Add(cc);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res.OrderBy(c => c.country).ToList();
        }
    }
}
