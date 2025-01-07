using CSharpEgitimKampi601.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEgitimKampi601.Services
{
    public class CustomerOperations
    {
        public void AddCustomer(Customer customer) 
        {
            var connection = new MongoDbConnection();//Bağlantı isteğinde bulundum
            var customerCollection = connection.GetCustomersCollection();//Tabloma bağlandım

            var document = new BsonDocument
            {
                {"CustomerName",customer.CustomerName },
                {"CustomerSurname",customer.CustomerSurname },
                {"CustomerCity", customer.CustomerCity},
                {"CustomerBalance",customer.CustomerBalance },
                {"CustomerShoppingCount",customer.CustomerShoppingCount }
            };//Parametrelerimi gönderdim

            customerCollection.InsertOne(document);//Ekleme işlemimi yaptım, Bu işlem MongoDb de bizim ekleme işlemlerimizi gerçekleştirecektir        
        } 

        public List<Customer> GetAllCustomer() //Customer türünde bütün müşterileri getir adında bir metot yazıyorum 
        { 
            var connection = new MongoDbConnection();//Veri tabanıma Bağlantımı oluşturuyorum
            var customerCollection= connection.GetCustomersCollection();//customer sınıfından değerlerimi getiriyorum(kollleksiyona yani tabloya bağlantı hazırlamış oldum)
            var customers = customerCollection.Find(new BsonDocument()).ToList();
            List<Customer> customerList=new List<Customer>();
            foreach (var c in customers) 
            {
                customerList.Add(new Customer
                {
                    CustomerId = c["_id"].ToString(),
                    CustomerBalance = decimal.Parse(c["CustomerBalance"].ToString()),
                    CustomerCity = c["CustomerCity"].ToString(),
                    CustomerName = c["CustomerName"].ToString(),
                    CustomerShoppingCount = int.Parse(c["CustomerShoppingCount"].ToString()),
                    CustomerSurname = c["CustomerSurname"].ToString()
                });
            }
            return customerList;

        }
        
        public void DeleteCustomer(string id)
        {
            var connection = new MongoDbConnection();
            var customerCollection = connection.GetCustomersCollection();
            var filter=Builders<BsonDocument>.Filter.Eq("_id",ObjectId.Parse(id));
            customerCollection.DeleteOne(filter);
        }

        public void UpdateCustomer(Customer customer)
        {
            var connection = new MongoDbConnection();
            var customerCollection = connection.GetCustomersCollection();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(customer.CustomerId));
            var updatedValue = Builders<BsonDocument>.Update
                .Set("CustomerName", customer.CustomerName)
                .Set("CustomerSurname", customer.CustomerSurname)
                .Set("CustomerCity", customer.CustomerCity)
                .Set("CustomerBalance", customer.CustomerBalance)
                .Set("CustomerSoppingCount", customer.CustomerShoppingCount);
            customerCollection.UpdateOne(filter,updatedValue);
        }
    
    }
}
