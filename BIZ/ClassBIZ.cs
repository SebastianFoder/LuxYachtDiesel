using IO;
using Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class ClassBIZ : ClassNotify
    {
        ClassLuxYachtDieselDB _classDB;
        ClassCallWebApi _classAPI;

		private ClassCurrency _currency;
		private ClassCustomer _selectedCustomer;
		private ClassSupplier _selectedSupplier;
		private ClassCustomer _fallbackCustomer;
		private ClassSupplier _fallbackSupplier;
		private List<ClassCountry> _listCountry;
		private List<ClassCustomer> _listCustomers;
		private List<ClassSupplier> _listSuppliers;
		private ClassDieselPrice _dieselPrice;
		private List<ClassDieselPrice> _listDieselPrice;
		private ClassOrder _order;
        private string _currentTime;
        private string _diselPriceInput;
        private string _inputLiters;
        private double _customerPrice;
        private double _supplierPrice;
        private double _profit;
        private bool _UCCIsReadOnly;
        private bool _UCCIsReadOnlyOpp;
        private bool _UCSIsReadOnly;
        private bool _UCSIsReadOnlyOpp;
        private bool _tabControlEnabled;


        /// <summary>
        /// Initializes a new instance of the ClassBIZ class and sets up the data context and properties for 
		/// interacting with customer, supplier, diesel price, and order data. Also retrieves data from the database and 
		/// initializes default values for some properties.
        /// </summary>
        public ClassBIZ()
		{
			_classDB = new ClassLuxYachtDieselDB();
			_classAPI = new ClassCallWebApi();
            tabControlEnabled = false;
            currency = new ClassCurrency();
			selectedCustomer = new ClassCustomer();
			selectedSupplier = new ClassSupplier();
			fallbackCustomer = new ClassCustomer();
			fallbackSupplier = new ClassSupplier();
			listCountry = _classDB.GetAllCountriesFromDB();
			listCustomers = new List<ClassCustomer>();
			listSuppliers = new List<ClassSupplier>();
			listDieselPrice = new List<ClassDieselPrice>();
			order = new ClassOrder();
			currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").ToUpperInvariant();
			GetAllCustomersForListFromDB();
			GetAllSuppliersForListFromDB();
			GetAllDieselPricesForListFromDB();
			dieselPrice = listDieselPrice.Where(cdp => cdp.date <= DateTime.Now).OrderByDescending(cdp => cdp.date).ThenByDescending(cdp => cdp.Id).FirstOrDefault();
            diselPriceInput = dieselPrice.price.ToString();
			inputLiters = string.Empty;
			customerPrice = 0D;
			supplierPrice = 0D;
			profit = 0D;
            UCCIsReadOnlyOpp = true;
			UCCIsReadOnly = true;
            UCSIsReadOnlyOpp = true;
            UCSIsReadOnly = true;
        }		

		public bool tabControlEnabled
        {
			get { return _tabControlEnabled; }
			set
			{
				if (_tabControlEnabled != value)
				{
					_tabControlEnabled = value;
				}
				Notify("tabControlEnabled");
			}
		}
		public bool UCSIsReadOnlyOpp
        {
            get { return _UCSIsReadOnlyOpp; }
            set
            {
                if (_UCSIsReadOnlyOpp != value)
                {
                    _UCSIsReadOnlyOpp = value;
                }
                Notify("UCSIsReadOnlyOpp");
            }
        }
        public bool UCSIsReadOnly
        {
            get { return _UCSIsReadOnly; }
            set
            {
                if (_UCSIsReadOnly != value)
                {
                    _UCSIsReadOnly = value;
                    UCSIsReadOnlyOpp = !value;
                }
                Notify("UCSIsReadOnly");
            }
        }
        public bool UCCIsReadOnlyOpp
        {
            get { return _UCCIsReadOnlyOpp; }
            set
            {
                if (_UCCIsReadOnlyOpp != value)
                {
                    _UCCIsReadOnlyOpp = value;
                }
                Notify("UCCIsReadOnlyOpp");
            }
        }
        public bool UCCIsReadOnly
		{
			get { return _UCCIsReadOnly; }
			set
			{
				if (_UCCIsReadOnly != value)
				{
					_UCCIsReadOnly = value;
					UCCIsReadOnlyOpp = !value;
				}
				Notify("UCCIsReadOnly");
			}
		}
		public string currentTime
		{
			get { return _currentTime; }
			set
			{
				if (_currentTime != value)
				{
					_currentTime = value;
				}
				Notify("currentTime");
			}
		}
		public double profit
		{
			get { return _profit; }
			set
			{
				if (_profit != value)
				{
					_profit = value;
				}
				Notify("profit");
			}
		}
		public double supplierPrice
		{
			get { return _supplierPrice; }
			set
			{
				if (_supplierPrice != value)
				{
					_supplierPrice = value;
				}
				Notify("supplierPrice");
			}
		}
		public double customerPrice
		{
			get { return _customerPrice; }
			set
			{
				if (_customerPrice != value)
				{
					_customerPrice = value;
				}
				Notify("customerPrice");
			}
		}
		public string inputLiters
		{
			get { return _inputLiters; }
			set
			{
				if (_inputLiters != value && int.TryParse(value, out int x))
				{
					_inputLiters = x.ToString();
                    CalculateAllValuesForOrder();
                }
				Notify("inputLiters");
			}
		}
		public string diselPriceInput
		{
			get { return _diselPriceInput; }
			set
			{
				if (_diselPriceInput != value)
				{
					if (double.TryParse(value, out double x))
					{
                        _diselPriceInput = Math.Round(x,12).ToString();
                    }
				}
				Notify("diselPriceInput");
			}
		}
		public ClassOrder order
		{
			get { return _order; }
			set
			{
				if (_order != value)
				{
					_order = value;
				}
				Notify("order");
			}
		}
        public List<ClassDieselPrice> listDieselPrice
        {
            get { return _listDieselPrice; }
            set
            {
                if (_listDieselPrice != value)
                {
                    _listDieselPrice = value;
                }
                Notify("listDieselPrice");
            }
        }
        public ClassDieselPrice dieselPrice
		{
			get { return _dieselPrice; }
			set
			{
				if (_dieselPrice != value)
				{
					_dieselPrice = value;
					if (!tabControlEnabled && value.date.Date == DateTime.Today)
					{
                        tabControlEnabled = true;
                        CalculateAllValuesForOrder();
                    }
				}
				Notify("dieselPrice");
			}
		}
		public List<ClassSupplier> listSuppliers
		{
			get { return _listSuppliers; }
			set
			{
				if (_listSuppliers != value)
				{
					_listSuppliers = value;
				}
				Notify("listSuppliers");
			}
		}
		public List<ClassCustomer> listCustomers
		{
			get { return _listCustomers; }
			set
			{
				if (_listCustomers != value)
				{
					_listCustomers = value;
				}
				Notify("listCustomers");
			}
		}
		public List<ClassCountry> listCountry
		{
			get { return _listCountry; }
			set
			{
				if (_listCountry != value)
				{
					_listCountry = value;
				}
				Notify("listCountry");
			}
		}
		public ClassSupplier fallbackSupplier
		{
			get { return _fallbackSupplier; }
			set
			{
				if (_fallbackSupplier != value)
				{
					_fallbackSupplier = value;
				}
				Notify("fallbackSupplier");
			}
		}
		public ClassCustomer fallbackCustomer
		{
			get { return _fallbackCustomer; }
			set
			{
				if (_fallbackCustomer != value)
				{
					_fallbackCustomer = value;
				}
				Notify("fallbackCustomer");
			}
		}
		public ClassSupplier selectedSupplier
		{
			get { return _selectedSupplier; }
			set
			{
				if (_selectedSupplier != value)
				{
					_selectedSupplier = value;
                    if (selectedSupplier != null)
                    {
                        fallbackSupplier = new ClassSupplier(selectedSupplier);
                        CalculateAllValuesForOrder();
                    }
                }
				Notify("selectedSupplier");
			}
		}
		public ClassCustomer selectedCustomer
		{
			get { return _selectedCustomer; }
			set
			{
				if (_selectedCustomer != value)
				{
					_selectedCustomer = value;
					if (selectedCustomer != null)
					{
						fallbackCustomer = new ClassCustomer(selectedCustomer);
						CalculateAllValuesForOrder();
					}
				}
				Notify("selectedCustomer");
			}
		}
		public ClassCurrency currency
		{
			get { return _currency; }
			set
			{
				if (_currency != value)
				{
					_currency = value;
				}
				Notify("currency");
			}
		}

        /// <summary>
        /// Bruger vores ClassCallWebAPI
		/// til at få en ClassCurrency med alle kurser osv.
        /// </summary>
        /// <returns>ClassCurrency</returns>
        public async Task GetAllCurrencysWebAPI()
		{
			currency = await _classAPI.GetURLContentsAsync();
		}

        /// <summary>
        /// Bruger vores listDiselPrice til at få den seneste
		/// tilføjet pris
        /// </summary>
        /// <returns>ClassDieselPrice</returns>
        public ClassDieselPrice GetDieselPriceFromDB()
        {
			return listDieselPrice.Where(cdp => cdp.date <= DateTime.Now).OrderByDescending(cdp => cdp.date).ThenByDescending(cdp => cdp.Id).FirstOrDefault();
        }

		/// <summary>
		/// Bruger vores database til at få alle diesel priserne
		/// </summary>
		public void GetAllDieselPricesForListFromDB()
		{
			listDieselPrice = _classDB.GetAllOilPriceFromDB();
		}

        /// <summary>
        /// Updatere vores listCustomers med
        /// vores ClassLuxYachtDieselDB methode
        /// GetAllCustomersFromDB
        /// </summary>
        public void GetAllCustomersForListFromDB()
		{
			listCustomers = _classDB.GetAllCustomersFromDB();
		}

        /// <summary>
        /// Updatere vores listSuppliers med
        /// vores ClassLuxYachtDieselDB methode
        /// GetAllSuppliersFromDB
        /// </summary>
        public void GetAllSuppliersForListFromDB()
		{
			listSuppliers = _classDB.GetAllSuppliersFromDB();
		}

        /// <summary>
        /// Updates or inserts the customer data into the database. 
		/// If the customer already has an ID, it updates the existing customer data in the database. 
		/// Otherwise, it saves a new customer in the database and updates the customer with the newly generated ID. 
		/// If the operation is successful, it updates the selected customer with the corresponding data from the database, 
		/// sets UCCIsReadOnly to true.
        /// </summary>
		/// <returns>The number of rows affected in the database.</returns>
        public int UpdateORInserCustomerInDB()
		{
			try
			{
                if (fallbackCustomer.Id > 0)
                {
                    int res = _classDB.UpdateCustomerInDB(fallbackCustomer);

                    if (res > 0)
                    {
                        GetAllCustomersForListFromDB();
                        if (fallbackCustomer.isActive)
                        {
                            selectedCustomer = listCustomers.Where(c => c.Id == fallbackCustomer.Id).FirstOrDefault();
                        }
                        else
                        {
                            selectedCustomer = new ClassCustomer();
                        }
                        UCCIsReadOnly = true;
                        tabControlEnabled = true;
                    }
                    return res;
                }
                else
                {
                    int newID = _classDB.SaveCustomerinDB(fallbackCustomer);
                    if (newID > 0)
                    {
                        GetAllCustomersForListFromDB();
                        if (fallbackCustomer.isActive)
                        {
                            selectedCustomer = listCustomers.Where(c => c.Id == newID).FirstOrDefault();
                        }
                        else
                        {
                            selectedCustomer = new ClassCustomer();
                        }
                        UCCIsReadOnly = true;
                        tabControlEnabled = true;
                    }
                    return newID;
                }
            }
			catch (Exception ex)
			{
				throw ex;
			}
        }

        /// <summary>
        /// Updates or inserts the supplier data into the database. 
		/// If the supplier already has an ID, it updates the existing supplier data in the database. 
		/// Otherwise, it saves a new supplier in the database and updates the supplier with the newly generated ID. 
		/// If the operation is successful, it updates the selected supplier with the corresponding data from the database, 
		/// sets UCSIsReadOnly to true.
        /// </summary>
		/// <returns>The number of rows affected in the database.</returns>
        public int UpdateOrInsertSupplierInDB()
		{
			try
			{
				if (fallbackSupplier.Id > 0)
				{
					int res = _classDB.UpdateSupplierInDB(fallbackSupplier);
					if (res > 0)
					{
						GetAllSuppliersForListFromDB();
						if (fallbackSupplier.isEnabled)
						{
							selectedSupplier = listSuppliers.Where(c => c.Id == fallbackSupplier.Id).FirstOrDefault();
						}
						else
						{
							selectedSupplier = new ClassSupplier();
						}
						UCSIsReadOnly = true;
						tabControlEnabled = true;
					}
					return res;
				}
				else
				{
					int newID = _classDB.SaveSupplierInDB(fallbackSupplier);
					if (newID > 0)
					{
						GetAllSuppliersForListFromDB();
						if (fallbackSupplier.isEnabled)
						{
							selectedSupplier = listSuppliers.Where(c => c.Id == newID).FirstOrDefault();
						}
						else
						{
							selectedSupplier = new ClassSupplier();
						}
						UCSIsReadOnly = true;
						tabControlEnabled = true;
					}
					return newID;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

        /// <summary>
        /// Inserts an order into the database, calculating various values based on selected currencies, rates, and input liters.
        /// The method saves the order to the database and updates relevant lists, resets input values.
        /// </summary>
        /// <returns>The number of rows affected in the database.</returns>
        public int InsertOrderInDB()
		{
			try
			{
				order.date = DateTime.Now;
				order.price = dieselPrice.price;
				order.customerRate = Convert.ToDouble(currency.rates[selectedCustomer.country.currencyCode]);
				order.supplierRate = Convert.ToDouble(currency.rates[selectedSupplier.country.currencyCode]);
				order.ownRate = Convert.ToDouble(currency.rates["DKK"]);
				order.ownProfit = (order.price * 0.148D) * Convert.ToDouble(currency.rates["DKK"]);
				order.volume = Convert.ToInt32(inputLiters);
				order.customer = selectedCustomer;
				order.supplier = selectedSupplier;
				int res = _classDB.SaveOrderToDB(order);
				if (res > 0)
				{
					RegretNewOrderForDB();
					order = new ClassOrder();
				}
				return res;
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

		/// <summary>
		/// Resets the values in the UI for the UserControlCustomer
		/// </summary>
		public void RegretUpdateOrNewCustomerForDB()
		{
			UCCIsReadOnly = true;
			if (selectedCustomer.Id > 0)
			{
                fallbackCustomer = new ClassCustomer(selectedCustomer);
			}
			else
			{
				fallbackCustomer = new ClassCustomer();
			}
			tabControlEnabled = true;
        }

        /// <summary>
        /// Resets the values in the UI for the UserControlSupplier
        /// </summary>
        public void RegretUpdateOrNewSupplierForDB()
		{
            UCSIsReadOnly = true;
            if (selectedSupplier.Id > 0)
            {
                fallbackSupplier = new ClassSupplier(selectedSupplier);
            }
            else
            {
                fallbackSupplier = new ClassSupplier();
            }
            tabControlEnabled = true;
        }

        /// <summary>
        /// Saves a new diesel price to the database using the value in the diselPriceInput field.
        /// If the save operation was successful, the method updates the list of diesel prices and 
		/// retrieves the new diesel price from the database.
        /// </summary>
        /// <returns>The number of rows affected in the database.</returns>
        public int NewDieselPriceForDB()
		{
			try
			{
				int res = _classDB.SaveDieselPrice(Convert.ToDouble(diselPriceInput));
				if (res > 0)
				{
					GetAllDieselPricesForListFromDB();
					dieselPrice = GetDieselPriceFromDB();
					diselPriceInput = dieselPrice.price.ToString();
				}
				return res;
			}
			catch (Exception ex) 
			{ 
				throw ex; 
			}
		}

        /// <summary>
        /// Resets the values in the UI for the UserControlDiesel
        /// </summary>
        public void RegretNewOrderForDB()
		{
			GetAllCustomersForListFromDB();
			GetAllSuppliersForListFromDB();
			ClearAllValuesForOrder();
        }

        /// <summary>
        /// Resets the values in the UI for the UserControlDiesel
        /// </summary>
        public void ClearAllValuesForOrder()
		{
            customerPrice = 0;
            supplierPrice = 0;
            profit = 0;
            inputLiters = "0";
        }

        /// <summary>
        /// Calculates various values for an order based on the selected customer, supplier, diesel price, and currency.
        /// The method uses the inputLiters field to calculate the total price of diesel and then 
		/// converts it to customer and supplier prices using their respective currency rates.
        /// The method also calculates the profit in DKK based on a fixed percentage.
        /// </summary>
        public void CalculateAllValuesForOrder()
		{
			if (selectedCustomer != null && selectedSupplier != null && dieselPrice != null && currency != null && currency.rates.ContainsKey(selectedCustomer.country.currencyCode) && currency.rates.ContainsKey(selectedSupplier.country.currencyCode) && currency.rates.ContainsKey("DKK"))
			{
                double price = dieselPrice.price * Convert.ToDouble(inputLiters);
                customerPrice = Math.Round(price + (price * 0.00148D) * Convert.ToDouble(currency.rates[selectedCustomer.country.currencyCode]), 3);
                supplierPrice = Math.Round(price + (price * 0.00148D) * Convert.ToDouble(currency.rates[selectedSupplier.country.currencyCode]), 3);
                profit = Math.Round((price * 0.00148D) * Convert.ToDouble(currency.rates["DKK"]), 3);
            }
		}
	}
}
