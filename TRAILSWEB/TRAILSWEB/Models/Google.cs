using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using TRAILSWEB.Models;

namespace Google.Analytics.Ecommerce
{
    public class Transaction
    {
        private string _currencyValue = "USD";

        public string id { get; set; }          // Unique Transaction Identifier - Required
        public string affiliation { get; set; } // Affiliation or Store Name - Optional
        public decimal revenue { get; set; }    // Grand Total - Required
        public decimal shipping { get; set; }   // Shipping Cost - Optional
        public decimal tax { get; set; }        // Taxes Charged - Optional
        public string currency                  // Local Currency Code - Optional
        {
            get { return _currencyValue; }
            set { _currencyValue = value; }
        }
        public List<Item> items { get; set; }   // List of Items Purchased - Optional

        /// <summary>
        /// Create a new Google Analytics Transaction without Items
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="affiliation"></param>
        /// <param name="grandTotal"></param>
        /// <param name="shipping"></param>
        /// <param name="tax"></param>
        /// <param name="currencyCode"></param>
        public Transaction(string transactionId, string affiliation, decimal grandTotal, decimal shipping, decimal tax,
            string currencyCode)
        {
            this.id = transactionId;
            this.affiliation = affiliation;
            this.revenue = grandTotal;
            this.shipping = shipping;
            this.tax = tax;
            this.currency = currencyCode;
        }

        /// <summary>
        /// Create a new Google Analytics Transaction Google Cart Items
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="affiliation"></param>
        /// <param name="grandTotal"></param>
        /// <param name="shipping"></param>
        /// <param name="tax"></param>
        /// <param name="currencyCode"></param>
        /// <param name="cartItems"></param>
        public Transaction(string transactionId, string affiliation, decimal grandTotal, decimal shipping, decimal tax,
            string currencyCode, List<Item> cartItems)
        {
            this.id = transactionId;
            this.affiliation = affiliation;
            this.revenue = grandTotal;
            this.shipping = shipping;
            this.tax = tax;
            this.currency = currencyCode;
            this.items = cartItems;
        }

        /// <summary>
        /// Create a new Google Analytics Transaction built from an existing Shopping Cart
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="affiliation"></param>
        /// <param name="grandTotal"></param>
        /// <param name="shipping"></param>
        /// <param name="tax"></param>
        /// <param name="currencyCode"></param>
        /// <param name="cartItems"></param>
        public Transaction(string transactionId, string affiliation, decimal grandTotal, decimal shipping, decimal tax,
            string currencyCode = null, List<Cart> cartItems = null)
        {
            this.id = transactionId;
            this.affiliation = affiliation;
            this.revenue = grandTotal;
            this.shipping = shipping;
            this.tax = tax;
            this.currency = (!string.IsNullOrEmpty(currencyCode)) ? currencyCode : _currencyValue;

            foreach (var item in cartItems)
            {
                // Add Item from Shopping Cart
                this.AddItem(
                                transactionId,
                                item.TransponderDetail.ShortDescription,
                                item.TransponderDetail.TransponderType.ToString(),
                                null,
                                item.TransponderDetail.Price.ToDecimal(),
                                1
                            );
            }
        }

        public void AddItem(string transactionId, string productName, string productId, string category, decimal price, int quantity)
        {
            // Determine if an Item of the same SKU already exists in Cart
            bool itemExists = (!this.items.IsNullOrEmpty()) ? this.items.Any(i => i.sku == productId) : false;

            if (!itemExists)
            {
                if (this.items.IsNullOrEmpty())
                {
                    // Initialize List
                    this.items = new List<Item>();
                }

                // Add New Item
                this.items.Add(new Item(transactionId, productName, productId, category, price, quantity));
            }
            else
            {
                // Increase Quantity
                this.items.Find(i => i.sku == productId).quantity += 1;

                // Add Price
                this.items.Find(i => i.sku == productId).price += price;
            }
        }

        /// <summary>
        /// Returns JavaScript Block(s) to Add Ecommerce Transaction
        /// <remarks>Optionally includes Scripts to Add Items if provided.</remarks>
        /// </summary>
        /// <returns>Google Analytics Ecommerce JavaScript Code Blocks</returns>
        public string ToJavaScript()
        {
            StringBuilder builder = new StringBuilder();

            // Initialize Ecommerce Plugin
            builder.AppendLine("\tga('require', 'ecommerce', 'ecommerce.js');\n");

            // Retrieve JSON Version of Transaction
            string jsonTransaction = this.ToString();

            // Add Transaction
            builder.AppendLine(string.Format("\tga('ecommerce:addTransaction', {0});\n", jsonTransaction));

            // Check for Items to Add
            if (!this.items.IsNullOrEmpty())
            {
                string jsonItem = string.Empty;

                foreach (var item in this.items)
                {
                    // Retrieve JSON Version of Item
                    jsonItem = item.ToString();

                    // Add Item
                    builder.AppendLine(string.Format("\tga('ecommerce:addItem', {0});\n", jsonItem));
                }
            }

            builder.AppendLine("\tga('ecommerce:send');");

            return builder.ToString();
        }

        /// <summary>
        /// Overrides ToString to Return a JSON Formatted version of the Transaction Object
        /// </summary>
        /// <returns>JSON String</returns>
        public override string ToString()
        {
            // Create new Transaction without "Items"
            var transaction = new
            {
                id = this.id,
                affiliation = this.affiliation,
                revenue = this.revenue,
                shipping = this.shipping,
                tax = this.tax,
                currency = this.currency
            };

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // Convert Transaction Object to JSON for return to Page/View
            return JsonConvert.SerializeObject(transaction, Formatting.Indented, settings).Replace("\"", "'");
        }
    }

    public class Item
    {
        public string id { get; set; }          // Transaction Identifier - Foreign Key
        public string name { get; set; }        // Product Name - Required
        public string sku { get; set; }         // Product SKU - Required
        public string category { get; set; }    // Product Category - Optional
        public decimal price { get; set; }      // Uniq Price - Required
        public int quantity { get; set; }       // Quantity - Required

        public Item(string transactionId, string productName, string productId, string category, decimal price, int quantity)
        {
            this.id = transactionId;
            this.name = productName;
            this.sku = productId;
            this.category = category;
            this.price = price;
            this.quantity = quantity;
        }

        /// <summary>
        /// Overrides ToString to Return a JSON Formatted version of the Transaction Object
        /// </summary>
        /// <returns>JSON String</returns>
        public override string ToString()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // Convert Transaction Object to JSON for return to Page/View
            return JsonConvert.SerializeObject(this, Formatting.Indented, settings).Replace("\"", "'");
        }
    }
}