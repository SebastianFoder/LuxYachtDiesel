using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ClassCustomer : ClassNotify
    {
		private int _Id;
		private string _name;
		private string _address;
		private string _city;
		private string _postalCode;
		private ClassCountry _country;
		private string _phone;
		private string _mailAdr;
        private bool _isActive;

        public ClassCustomer()
		{
			Id = 0;
			name = string.Empty;
			address = string.Empty;
			city = string.Empty;
			postalCode = string.Empty;
			country = new ClassCountry();
			phone = string.Empty;
			mailAdr = string.Empty;
			isActive = true;
		}
		public ClassCustomer(ClassCustomer inCustomer)
		{
			Id = inCustomer.Id;
			name = inCustomer.name;
			address = inCustomer.address;
			city = inCustomer.city;
			postalCode = inCustomer.postalCode;
			country = inCustomer.country;
			phone = inCustomer.phone;
			mailAdr = inCustomer.mailAdr;
			isActive = inCustomer.isActive;
		}

		

		public bool isActive
		{
			get { return _isActive; }
			set
			{
				if (_isActive != value)
				{
					_isActive = value;
				}
				Notify("isActive");
			}
		}


		public string mailAdr
		{
			get { return _mailAdr; }
			set
			{
				if (_mailAdr != value)
				{
					_mailAdr = value;
				}
				Notify("mailAdr");
			}
		}


		public string phone
		{
			get { return _phone; }
			set
			{
				if (_phone != value)
				{
					_phone = value;
				}
				Notify("phone");
			}
		}


		public ClassCountry country
        {
			get { return _country; }
			set
			{
				if (_country != value)
				{
					_country = value;
				}
				Notify("country");
			}
		}


		public string postalCode
		{
			get { return _postalCode; }
			set
			{
				if (_postalCode != value)
				{
					_postalCode = value;
				}
				Notify("postalCode");
			}
		}


		public string city
		{
			get { return _city; }
			set
			{
				if (_city != value)
				{
					_city = value;
				}
				Notify("city");
			}
		}


		public string address
		{
			get { return _address; }
			set
			{
				if (_address != value)
				{
					_address = value;
				}
				Notify("address");
			}
		}


		public string name
		{
			get { return _name; }
			set
			{
				if (_name != value)
				{
					_name = value;
				}
				Notify("name");
			}
		}


		public int Id
		{
			get { return _Id; }
			set
			{
				if (_Id != value)
				{
					_Id = value;
				}
				Notify("Id");
			}
		}

	}
}
