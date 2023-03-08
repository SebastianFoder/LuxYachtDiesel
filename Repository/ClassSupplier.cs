using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ClassSupplier : ClassNotify
    {
		private int _Id;
		private string _firmName;
		private string _contactName;
		private string _address;
		private string _city;
		private string _postalCode;
		private ClassCountry _country;
		private string _phone;
		private string _mailAdr;
        private bool _isEnabled;

        public ClassSupplier()
		{
			Id = 0;
			firmName = string.Empty;
			contactName = string.Empty;
			address = string.Empty;
			city = string.Empty;
			postalCode = string.Empty;
			country = new ClassCountry();
			phone = string.Empty;
			mailAdr = string.Empty;
			isEnabled = true;
		}
		public ClassSupplier(ClassSupplier inSupplier)
		{
			Id = inSupplier.Id;
			firmName = inSupplier.firmName;
			contactName = inSupplier.contactName;
			address = inSupplier.address;
			city = inSupplier.city;
			postalCode = inSupplier.postalCode;
			country = inSupplier.country;
			phone = inSupplier.phone;
			mailAdr = inSupplier.mailAdr;
			isEnabled = inSupplier.isEnabled;
		}

		

		public bool isEnabled
		{
			get { return _isEnabled; }
			set
			{
				if (_isEnabled != value)
				{
					_isEnabled = value;
				}
				Notify("isEnabled");
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


		public string contactName
		{
			get { return _contactName; }
			set
			{
				if (_contactName != value)
				{
					_contactName = value;
				}
				Notify("contactName");
			}
		}


		public string firmName
		{
			get { return _firmName; }
			set
			{
				if (_firmName != value)
				{
					_firmName = value;
				}
				Notify("firmName");
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
