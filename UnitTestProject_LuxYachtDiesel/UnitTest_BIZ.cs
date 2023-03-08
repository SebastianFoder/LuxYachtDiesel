using Microsoft.VisualStudio.TestTools.UnitTesting;
using IO;
using BIZ;
using Repository;
using System;

namespace UnitTestProject_LuxYachtDiesel
{
    [TestClass]
    public class UnitTest_BIZ
    {
        [TestMethod]
        public void TestCalculateOrderPrices_CurrencyConversionAndProfitCalculation_BRL_MXN()
        {
            // Arrange the necessary objects and values for the test

            ClassOrder co = new ClassOrder();
            co.volume = 17500;
            ClassCustomer cc = new ClassCustomer();
            cc.country.currencyCode = "BRL";
            co.customer = cc;
            ClassSupplier cs = new ClassSupplier();
            cs.country.currencyCode = "MXN";
            co.supplier = cs;
            ClassCurrency ccu = new ClassCurrency();
            ccu.rates["DKK"] = 7.011081M;
            ccu.rates["USD"] = 1M;
            ccu.rates["BRL"] = 5.2011M;
            ccu.rates["MXN"] = 18.363982M;
            ClassDieselPrice cdp = new ClassDieselPrice();
            cdp.price = 1.03D;
            double price = 0D;
            double customerPrice = 0D;
            double supplierPrice = 0D;
            double profit = 0D;

            // Act: Calculate the order, customer price, supplier price, and profit based on the arranged data

            price = cdp.price * co.volume;
            customerPrice = price + (price * 0.00148D) * Convert.ToDouble(ccu.rates[co.customer.country.currencyCode]);
            supplierPrice = price + (price * 0.00148D) * Convert.ToDouble(ccu.rates[co.supplier.country.currencyCode]);
            profit = (price * 0.00148D) * Convert.ToDouble(ccu.rates["DKK"]);

            // Assert that the expected values are equal to the actual calculated values
            Assert.AreEqual(18025D, price, 0.00001);
            Assert.AreEqual(18163.7497447D, customerPrice, 0.00001);
            Assert.AreEqual(18514.895947814D, supplierPrice, 0.00001);
            Assert.AreEqual(187.034607837D, profit, 0.00001);
        }
        [TestMethod]
        public void TestCalculateOrderPrices_CurrencyConversionAndProfitCalculation_GBP_EUR()
        {
            // Arrange the necessary objects and values for the test

            ClassOrder co = new ClassOrder();
            co.volume = 21850;
            ClassCustomer cc = new ClassCustomer();
            cc.country.currencyCode = "GBP";
            co.customer = cc;
            ClassSupplier cs = new ClassSupplier();
            cs.country.currencyCode = "EUR";
            co.supplier = cs;
            ClassCurrency ccu = new ClassCurrency();
            ccu.rates["DKK"] = 7.011081M;
            ccu.rates["USD"] = 1M;
            ccu.rates["GBP"] = 0.827588M;
            ccu.rates["EUR"] = 0.941942M;
            ClassDieselPrice cdp = new ClassDieselPrice();
            cdp.price = 0.91D;
            double price = 0D;
            double customerPrice = 0D;
            double supplierPrice = 0D;
            double profit = 0D;

            // Act: Calculate the order, customer price, supplier price, and profit based on the arranged data

            price = cdp.price * co.volume;
            customerPrice = price + (price * 0.00148D) * Convert.ToDouble(ccu.rates[co.customer.country.currencyCode]);
            supplierPrice = price + (price * 0.00148D) * Convert.ToDouble(ccu.rates[co.supplier.country.currencyCode]);
            profit = (price * 0.00148D) * Convert.ToDouble(ccu.rates["DKK"]);

            // Assert that the expected values are equal to the actual calculated values
            Assert.AreEqual(19883.5D, price, 0.00001);
            Assert.AreEqual(19907.853912077D, customerPrice, 0.00001);
            Assert.AreEqual(19911.2190735604D, supplierPrice, 0.00001);
            Assert.AreEqual(206.31914701398D, profit, 0.00001);
        }
        [TestMethod]
        public void TestCalculateOrderPrices_CurrencyConversionAndProfitCalculation_NOK_RUB()
        {
            // Arrange the necessary objects and values for the test

            ClassOrder co = new ClassOrder();
            co.volume = 99500;
            ClassCustomer cc = new ClassCustomer();
            cc.country.currencyCode = "NOK";
            co.customer = cc;
            ClassSupplier cs = new ClassSupplier();
            cs.country.currencyCode = "RUB";
            co.supplier = cs;
            ClassCurrency ccu = new ClassCurrency();
            ccu.rates["DKK"] = 7.011081M;
            ccu.rates["USD"] = 1M;
            ccu.rates["NOK"] = 10.326784M;
            ccu.rates["RUB"] = 74.850299M;
            ClassDieselPrice cdp = new ClassDieselPrice();
            cdp.price = 1.23D;
            double price = 0D;
            double customerPrice = 0D;
            double supplierPrice = 0D;
            double profit = 0D;

            // Act: Calculate the order, customer price, supplier price, and profit based on the arranged data

            price = cdp.price * co.volume;
            customerPrice = price + (price * 0.00148D) * Convert.ToDouble(ccu.rates[co.customer.country.currencyCode]);
            supplierPrice = price + (price * 0.00148D) * Convert.ToDouble(ccu.rates[co.supplier.country.currencyCode]);
            profit = (price * 0.00148D) * Convert.ToDouble(ccu.rates["DKK"]);


            // Assert that the expected values are equal to the actual calculated values

            Assert.AreEqual(122385D, price, 0.00001);
            Assert.AreEqual(124255.488320563D, customerPrice, 0.00001);
            Assert.AreEqual(135942.61968781D, supplierPrice, 0.00001);
            Assert.AreEqual(1269.9156993138D, profit, 0.00001);
        }
    }
}
