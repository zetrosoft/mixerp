﻿/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using MixERP.Net.Common.Extensions;
using MixERP.Net.Core.Modules.Inventory.Data.Helpers;
using MixERP.Net.Entities.Core;
using MixERP.Net.Entities.Office;
using MixERP.Net.FrontEnd.Cache;

namespace MixERP.Net.Core.Modules.Inventory.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class ItemData : WebService
    {
        [WebMethod]
        public decimal CountItemInStock(string itemCode, int unitId, int storeId)
        {
            return Items.CountItemInStock(AppUsers.GetDatabase(), itemCode, unitId, storeId);
        }

        [WebMethod]
        public Collection<ListItem> GetAgents()
        {
            Collection<ListItem> values = new Collection<ListItem>();


            foreach (Salesperson salesperson in Salespersons.GetSalespersons(AppUsers.GetDatabase()))
            {
                values.Add(new ListItem(salesperson.SalespersonName, salesperson.SalespersonId.ToString(DateTimeFormatInfo.InvariantInfo)));
            }

            return values;
        }

        [WebMethod]
        public string GetItemCodeByItemId(int itemId)
        {
            return Items.GetItemCodeByItemId(AppUsers.GetDatabase(), itemId);
        }

        [WebMethod]
        public Collection<ListItem> GetItems(string tranBook)
        {
            if (string.IsNullOrWhiteSpace(tranBook))
            {
                return new Collection<ListItem>();
            }

            if (tranBook.ToUpperInvariant().Equals("SALES"))
            {
                return GetItems();
            }

            return GetStockItems();
        }

        [WebMethod]
        public Collection<ListItem> GetPaymentTerms()
        {
            Collection<ListItem> values = new Collection<ListItem>();

            foreach (PaymentTerm paymentTerm in PaymentTerms.GetPaymentTerms(AppUsers.GetDatabase()))
            {
                values.Add(new ListItem(paymentTerm.PaymentTermName, paymentTerm.PaymentTermId.ToString(CultureInfo.InvariantCulture)));
            }

            return values;
        }

        [WebMethod]
        public decimal GetPrice(string tranBook, string itemCode, string partyCode, int priceTypeId, int unitId)
        {
            decimal price = 0;

            if (string.IsNullOrWhiteSpace(itemCode))
            {
                return 0;
            }

            switch (tranBook)
            {
                case "Sales":
                    price = Items.GetItemSellingPrice(AppUsers.GetDatabase(), itemCode, partyCode, priceTypeId, unitId);
                    break;

                case "Purchase":
                    price = Items.GetItemCostPrice(AppUsers.GetDatabase(), itemCode, partyCode, unitId);
                    break;
            }

            return price;
        }

        [WebMethod]
        public Collection<ListItem> GetPriceTypes()
        {
            Collection<ListItem> values = new Collection<ListItem>();


            foreach (PriceType priceType in PriceTypes.GetPriceTypes(AppUsers.GetDatabase()))
            {
                values.Add(new ListItem(priceType.PriceTypeName, priceType.PriceTypeId.ToString(CultureInfo.InvariantCulture)));
            }

            return values;
        }

        [WebMethod]
        public Collection<ListItem> GetShippers()
        {
            Collection<ListItem> values = new Collection<ListItem>();

            foreach (Shipper shipper in Shippers.GetShippers(AppUsers.GetDatabase()))
            {
                values.Add(new ListItem(shipper.CompanyName, shipper.ShipperId.ToString(CultureInfo.InvariantCulture)));
            }

            return values;
        }

        [WebMethod]
        public Collection<ListItem> GetStores()
        {
            int officeId = AppUsers.GetCurrentLogin().View.OfficeId.ToInt();
            Collection<ListItem> values = new Collection<ListItem>();

            IEnumerable<Store> stores = Stores.GetStores(AppUsers.GetDatabase(), officeId);

            foreach (Store store in stores)
            {
                values.Add(new ListItem(store.StoreName, store.StoreId.ToString(CultureInfo.InvariantCulture)));
            }

            return values;
        }

        [WebMethod]
        public decimal GetTaxRate(string itemCode)
        {
            return Items.GetTaxRate(AppUsers.GetDatabase(), itemCode);
        }

        [WebMethod]
        public Collection<ListItem> GetUnits(string itemCode)
        {
            Collection<ListItem> values = new Collection<ListItem>();

            using (DataTable table = Units.GetUnitViewByItemCode(AppUsers.GetDatabase(), itemCode))
            {
                foreach (DataRow dr in table.Rows)
                {
                    values.Add(new ListItem(dr["unit_name"].ToString(), dr["unit_id"].ToString()));
                }

                return values;
            }
        }

        [WebMethod]
        public bool IsStockItem(string itemCode)
        {
            return Items.IsStockItem(AppUsers.GetDatabase(), itemCode);
        }

        [WebMethod]
        public bool ItemCodeExists(string itemCode)
        {
            return Items.ItemExistsByCode(AppUsers.GetDatabase(), itemCode);
        }

        [WebMethod]
        public bool UnitNameExists(string unitName)
        {
            return Units.UnitExistsByName(AppUsers.GetDatabase(), unitName);
        }

        private static Collection<ListItem> GetItems()
        {
            Collection<ListItem> values = new Collection<ListItem>();

            foreach (Item item in Items.GetItems(AppUsers.GetDatabase()))
            {
                values.Add(new ListItem(item.ItemName, item.ItemCode));
            }

            return values;
        }

        private static Collection<ListItem> GetStockItems()
        {
            Collection<ListItem> values = new Collection<ListItem>();

            foreach (Item item in Items.GetStockItems(AppUsers.GetDatabase()))
            {
                values.Add(new ListItem(item.ItemName, item.ItemCode));
            }

            return values;
        }
    }
}