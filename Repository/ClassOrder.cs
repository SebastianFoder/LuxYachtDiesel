using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ClassOrder : ClassNotify
    {
		private DateTime _date;
		private double _price;
		private double _customerRate;
		private double _supplierRate;
        private double _ownRate;
        private double _ownProfit;
		private int _volume;
		private ClassCustomer _customer;
		private ClassSupplier _supplier;

		public ClassOrder()
		{
			date = DateTime.Now;
			price = 0D;
			customerRate = 0D;
            supplierRate = 0D;
			ownProfit = 0D;
			volume = 0;
			customer = new ClassCustomer();
			supplier = new ClassSupplier();
		}


		public ClassSupplier supplier
		{
			get { return _supplier; }
			set
			{
				if (_supplier != value)
				{
					_supplier = value;
				}
				Notify("supplier");
			}
		}



		public ClassCustomer customer
		{
			get { return _customer; }
			set
			{
				if (_customer != value)
				{
					_customer = value;
				}
				Notify("customer");
			}
		}


		public int volume
		{
			get { return _volume; }
			set
			{
				if (_volume != value)
				{
					_volume = value;
				}
				Notify("volume");
			}
		}


		public double ownProfit
		{
			get { return _ownProfit; }
			set
			{
				if (_ownProfit != value)
				{
					_ownProfit = value;
				}
				Notify("ownProfit");
			}
		}


		

		public double ownRate
		{
			get { return _ownRate; }
			set
			{
				if (_ownRate != value)
				{
					_ownRate = value;
				}
				Notify("ownRate");
			}
		}




		public double supplierRate
        {
			get { return _supplierRate; }
			set
			{
				if (_supplierRate != value)
				{
					_supplierRate = value;
				}
				Notify("supplierRate");
			}
		}


		public double customerRate
		{
			get { return _customerRate; }
			set
			{
				if (_customerRate != value)
				{
					_customerRate = value;
				}
				Notify("customerRate");
			}
		}


		public double price
		{
			get { return _price; }
			set
			{
				if (_price != value)
				{
					_price = value;
				}
				Notify("price");
			}
		}


		public DateTime date
		{
			get { return _date; }
			set
			{
				if (_date != value)
				{
					_date = value;
				}
				Notify("date");
			}
		}

	}
}
