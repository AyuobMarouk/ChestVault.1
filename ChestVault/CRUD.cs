using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using ChestVault.Schemas;
using System.Linq;
using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using MongoDB.Bson;
using System.Xml.Linq;
using Microsoft.VisualBasic.ApplicationServices;
using System.Drawing;
using static MongoDB.Driver.WriteConcern;

namespace ChestVault
{
    public class CRUD
    {

        //private const string ConnectionString = "mongodb://169.254.134.180:27017";
        private const string ConnectionString = "mongodb://127.0.0.1:27017";

        private const string DatabaseName = "vault";
        private const string UsersCollection = "users";
        private const string ItemsCollection = "items";
        private const string RecitsCollection = "recits";
        private const string PurchesCollection = "purches";
        private const string FastSellCollection = "fastsell";
        private const string DebtCollection = "debt";
        private const string ActvititesCollection = "activites";
        private const string WorkCollection = "workschedule";
        private const string setingsCollection = "setings";
        private IMongoCollection<Schema> ConnectToMongo<Schema>(in string collection)
        {
            var clinet = new MongoClient(ConnectionString);
            var db = clinet.GetDatabase(DatabaseName);
            return db.GetCollection<Schema>(collection);
        }

        #region User Function
        public async Task<List<UsersSchema>> GetAllUsers()
        {
            var users = ConnectToMongo<UsersSchema>(UsersCollection);
            var results = await users.FindAsync(_ => true);
            return results.ToList();
        }
        public async Task<List<UsersSchema>> GetUsers(string name)
        {
            var users = ConnectToMongo<UsersSchema>(UsersCollection);
            var results = await users.FindAsync(a => a.Name == name);
            return results.ToList();
        }
        public async Task AddUser(UsersSchema user)
        {
            var User = ConnectToMongo<UsersSchema>(UsersCollection);
            await User.InsertOneAsync(user);
            return;
        }
        public Task UpdateUser(UsersSchema user)
        {
            var User = ConnectToMongo<UsersSchema>(UsersCollection);
            var fitler = Builders<UsersSchema>.Filter.Eq("Id", user.Id);
            return User.ReplaceOneAsync(fitler, user, new ReplaceOptions { IsUpsert = true });
        }
        public Task DeleteUser(UsersSchema user)
        {
            var User = ConnectToMongo<UsersSchema>(UsersCollection);
            return User.DeleteOneAsync(i => i.Id == user.Id);
        }

        #endregion

        #region Items Functions
        public async Task<List<ItemsSchema>> GetItemby(string Type, string Name, string QRcode)
        {
            var Item = ConnectToMongo<ItemsSchema>(ItemsCollection);
            IAsyncCursor<ItemsSchema> results = null;
            if (Type != null && Name != null && QRcode != null)
                results = await Item.FindAsync(i => i.QRcode.Any(q => q == QRcode) && i.Name == Name && i.Type == Type);
            else if (Type != null && Name != null)
                results = await Item.FindAsync(i => i.Name == Name && i.Type == Type);
            else if (Type != null && QRcode != null)
                results = await Item.FindAsync(i => i.QRcode.Any(q => q == QRcode) && i.Type == Type);
            else if (Name != null && QRcode != null)
                results = await Item.FindAsync(i => i.QRcode.Any(q => q == QRcode) && i.Name == Name);
            else if (Type != null)
                results = await Item.FindAsync(i => i.Type == Type);
            else if (QRcode != null)
                results = await Item.FindAsync(i => i.QRcode.Any(q => q == QRcode));
            else if (Name != null)
                results = await Item.FindAsync(i => i.Name == Name);
            else
                results = await Item.FindAsync(_ => true);
            return results.ToList<ItemsSchema>();
        }

        public async Task<List<ItemsSchema>> GetAllItems()
        {
            var items = ConnectToMongo<ItemsSchema>(ItemsCollection);
            var results = await items.FindAsync(_ => true);
            return results.ToList();
        }

        public async Task<List<ItemsSchema>> GetItem(string itemName)
        {
            var Item = ConnectToMongo<ItemsSchema>(ItemsCollection);
            var results = await Item.FindAsync(i => i.Name == itemName);
            return results.ToList();
        }

        public async Task Additem(ItemsSchema item)
        {
            var Item = ConnectToMongo<ItemsSchema>(ItemsCollection);
            await Item.InsertOneAsync(item);
            return;
        }
        public Task UpdateItem(ItemsSchema item)
        {
            var Item = ConnectToMongo<ItemsSchema>(ItemsCollection);
            var fitler = Builders<ItemsSchema>.Filter.Eq("Id", item.Id);
            return Item.ReplaceOneAsync(fitler, item, new ReplaceOptions { IsUpsert = true });
        }
        public async Task<bool> DeleteItem(ItemsSchema item)
        {
            var Item = ConnectToMongo<ItemsSchema>(ItemsCollection);
            double total = 0;
            if (item.Info != null)
            {
                foreach (var item1 in item.Info)
                {
                    total += item1.Amount;
                }
            }
            if (total == 0)
            {
                await Item.DeleteOneAsync(i => i.Id == item.Id);
                await DeleteFastSellByName(item.Name);
                return (true);
            }
            return (false);
        }
        public async Task<List<ItemsSchema>> GetItemByQR(string QRCode)
        {
            var Item = ConnectToMongo<ItemsSchema>(ItemsCollection);
            var results = await Item.FindAsync(i => i.QRcode.Any(q => q == QRCode));
            return results.ToList();
        }
        #endregion

        #region fastsell
        public async Task<List<FastSellSchema>> GetAllFastSells()
        {
            var fastsell = ConnectToMongo<FastSellSchema>(FastSellCollection);
            var results = await fastsell.FindAsync(_ => true);
            return results.ToList();
        }

        public async Task<List<FastSellSchema>> GetFastSellsByName(string itemName)
        {
            var fastsell = ConnectToMongo<FastSellSchema>(FastSellCollection);
            var results = await fastsell.FindAsync(i => i.Name == itemName);
            return results.ToList();
        }

        public async Task<List<FastSellSchema>> GetFastSellByMenu(string menu)
        {
            var fastsell = ConnectToMongo<FastSellSchema>(FastSellCollection);
            var results = await fastsell.FindAsync(i => i.Menu == menu);
            return results.ToList();
        }

        public async Task AddFastSellItem(FastSellSchema item)
        {
            var fastsell = ConnectToMongo<FastSellSchema>(FastSellCollection);
            await fastsell.InsertOneAsync(item);
            return;
        }
        public Task UpdateFastSell(FastSellSchema item)
        {
            var Item = ConnectToMongo<FastSellSchema>(FastSellCollection);
            var fitler = Builders<FastSellSchema>.Filter.Eq("Id", item.Id);
            return Item.ReplaceOneAsync(fitler, item, new ReplaceOptions { IsUpsert = true });
        }
        public Task DeleteFastSell(FastSellSchema item)
        {
            var Item = ConnectToMongo<FastSellSchema>(FastSellCollection);
            return Item.DeleteOneAsync(i => i.Id == item.Id);
        }
        public Task DeleteFastSellByName(string name)
        {
            var Item = ConnectToMongo<FastSellSchema>(FastSellCollection);
            return Item.DeleteOneAsync(i => i.Name == name);
        }

        #endregion

        #region Purchase Recite
        public async Task<string[]> GetSuppliers()
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            var results = await Purches.FindAsync(_ => true);
            var result = results.ToList();
            if (result.Count == 0)
                return (new string[0]);
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].supplier == "شركة عامة" || result[i].supplier == "مسترجعات") result.RemoveAt(i--);
            }
            if (result.Count == 0)
                return (new string[0]);
            string list = result[0].supplier;
            bool a;
            foreach (var name in result)
            {
                a = true;
                foreach (var item in list.Split(','))
                {
                    if (name.supplier == item)
                    {
                        a = false;
                        break;
                    }
                }
                if (a)
                {
                    list = list + ',' + name.supplier;
                }
            }
            return (string[])list.Split(',');
        }


        public async Task<List<PurchaseSchema>> GetAllPurches()
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            var results = await Purches.FindAsync(_ => true);
            return results.ToList();
        }
        public async Task<List<PurchaseSchema>> GetPurches(int PurchesNumber)
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            var results = await Purches.FindAsync(r => r.Number == PurchesNumber);
            return results.ToList();
        }
        public async Task AddPurches(PurchaseSchema purches)
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            await Purches.InsertOneAsync(purches);
            return;
        }
        public async Task<List<PurchaseSchema>> GetPurchesByDept(bool dept)
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            if (dept)
            {
                var results = await Purches.FindAsync(r => r.Dept == 0);
                return results.ToList();
            }
            else
            {
                var results = await Purches.FindAsync(r => r.Dept > 0);
                return results.ToList();
            }
        }
        public async Task<List<PurchaseSchema>> GetPurchesBySupplier(string supplier)
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            var results = await Purches.FindAsync(r => r.supplier == supplier);
            return results.ToList();
        }
        public async Task<List<PurchaseSchema>> GetPurchaseByUser(string User)
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            var results = await Purches.FindAsync(r => r.User == User);
            return results.ToList();
        }
        public async Task<List<PurchaseSchema>> GetPurchesByDate(int date)
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            var results = await Purches.FindAsync(r => r.Date == date);
            return results.ToList();
        }
        public async Task<List<PurchaseSchema>> GetPurchesByName(string item)
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            var results = await Purches.FindAsync(i => i.Items.Any(q => q.Name == item));
            return results.ToList();
        }
        public async Task<bool> DeletePurches(PurchaseSchema purches)
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            var items = new List<ItemsSchema>();
            foreach (var item in purches.Items)
            {
                var t = await GetItem(item.Name);
                var tmp = t.First();
                if (tmp.Info.Count() == 0)
                    return false;
                for (int i = 0; i < tmp.Info.Count(); i++)
                {
                    bool yes = false;
                    if (tmp.Info[i].ExpDate == item.ExpDate && tmp.Info[i].BuyPrice == item.BuyPrice)
                    {
                        if (tmp.Info[i].Amount >= item.Amount)
                        {
                            tmp.Info[i].Amount -= item.Amount;
                            if (tmp.Info[i].Amount == 0)
                            {
                                tmp.Info.RemoveAt(i);
                            }
                            items.Add(tmp);
                            yes = true;
                            break;
                        }
                    }
                    if (!yes)
                    { return (false); }
                }
            }
            foreach (var item in items)
            {
                await UpdateItem(item);
            }
            await Purches.DeleteOneAsync(i => i.Id == purches.Id);
            return (true);
        }
        public async Task<bool> PayDept(PurchaseSchema purches, double dept)
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@/
            var tmp = await GetPurches(purches.Number);
            var oldpurches = tmp.First();
            oldpurches.Paid += dept;
            oldpurches.Dept = oldpurches.Total - oldpurches.Paid;
            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@/
            var fitler = Builders<PurchaseSchema>.Filter.Eq("Id", oldpurches.Id);
            await Purches.ReplaceOneAsync(fitler, oldpurches, new ReplaceOptions { IsUpsert = true });
            return (true);
        }
        public async Task<bool> UpdatePurches(PurchaseSchema purches)
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            var oldPurch = await GetPurches(purches.Number);
            var items = new List<ItemsSchema>();
            if (purches.Items.Count < oldPurch.First<PurchaseSchema>().Items.Count) return false;
            foreach (var item in purches.Items)
            {
                var newItem = await GetItem(item.Name);
                items.Add(newItem.First<ItemsSchema>());
                bool notFound = true;
                foreach (var item1 in oldPurch.First<PurchaseSchema>().Items)
                {
                    if (item.Id == item1.Id && item1.Amount != 0)
                    {
                        foreach (var info in items.Last<ItemsSchema>().Info)
                        {
                            if (item1.ExpDate == info.ExpDate && item1.BuyPrice == info.BuyPrice)
                            {
                                info.BuyPrice = item.BuyPrice;
                                info.ExpDate = item.ExpDate;
                                info.Amount += item.Amount - item1.Amount;
                                items.Last().SellPrice = item.SellPrice;
                                if (info.Amount < 0) return false;
                                if (info.Amount == 0) items.Last<ItemsSchema>().Info.Remove(info);
                                break;
                            }
                        }
                        notFound = false;
                    }
                }
                if (notFound)
                {
                    var newInfo = new ItemInfo();
                    newInfo.Amount = item.Amount;
                    newInfo.BuyPrice = item.BuyPrice;
                    newInfo.ExpDate = item.ExpDate;
                    items.Last<ItemsSchema>().SellPrice = item.SellPrice;
                    items.Last<ItemsSchema>().Info.Add(newInfo);
                }
            }
            foreach (var item in items)
            {
                await UpdateItem(item);
            }
            var fitler = Builders<PurchaseSchema>.Filter.Eq("Id", purches.Id);
            await Purches.ReplaceOneAsync(fitler, purches, new ReplaceOptions { IsUpsert = true });
            return (true);
        }
        public async Task<BoughtItemsSchema> GetSoldItem(string name)
        {
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            var results = await Purches.FindAsync(r => r.Items.Any(n => n.Name == name));
            var tmp = results.ToList<PurchaseSchema>();
            if (tmp.Count() == 0)
                return null;
            var result = tmp[tmp.Count() - 1];
            foreach (var item in result.Items)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }  


        public async Task<List<double>> GetItemHistory(string name, bool buy)
        {
            List<double> history = new List<double>();
            var Purches = ConnectToMongo<PurchaseSchema>(PurchesCollection);
            var results = await Purches.FindAsync(r => r.Items.Any(n => n.Name == name));
            var tmp = results.ToList<PurchaseSchema>();
        
            if (tmp.Count() == 0) 
                return history; 

            foreach (var Purchase in tmp)
            {
                if (Purchase.supplier == "مسترجعات")
                    continue;
                foreach (var item in Purchase.Items)
                {
                    bool a = true;
                    for (int i = 0; i < history.Count(); i++)
                    {
                        if (buy && item.BuyPrice == history[i])
                        {
                            a = false;
                            break;
                        } 
                        else if (!buy && item.SellPrice == history[i]) 
                        {
                            a = false;
                            break;
                        }
                    }
                    if (a)
                    {
                        if (buy)
                            history.Add(item.BuyPrice);
                        else
                            history.Add(item.SellPrice);
                    }
                } 
            }
            return (history);
        }
        #endregion

        #region Sold Recite
        public async Task<List<int>> GetRecitesMonthMed(DateTime day, bool a, string name)
        {
            int end = day.Year * 10000 + day.Month * 100 + day.Day;
            int start = day.Year * 10000 + (day.Month - 1) * 100 + day.Day;
            if (day.Month == 1)
                start = (day.Year - 1) * 10000 + 12 * 100 + day.Day;

            var results = await GetReciteInDateRange(start, end);
            if (results == null || results.ToList<RecitesSchema>().Count() == 0)
                return new List<int>();
            List<int> sum = new List<int>();
            int count = 1;
            for (int i = 0; i < results.Count(); i++)
            {
                if (i == results.Count() - 1)
                {
                    sum.Add(calDay(results.GetRange(i,1), a, name) + 1000000 * (results[i].Date % 1000));
                    break;
                }
                else
                {
                    for (int j = i + 1; j < results.Count() || j == results.Count() - 1; j++)
                    {
                        if (j == results.Count() - 1 && results[i].Date != results[j].Date)
                        {
                            sum.Add(calDay(results.GetRange(i, j - i), a, name) + 1000000 * (results[i].Date % 1000));
                            i += j - i - 1;
                            break;
                        }
                        else if(j == results.Count() - 1)
                        {
                            sum.Add(calDay(results.GetRange(i, j + 1 - i), a, name) + 1000000 * (results[i].Date % 1000));
                            i += j - i;
                            break;

                        }
                        else if (results[i].Date != results[j].Date)
                        {
                            sum.Add(calDay(results.GetRange(i, j - i), a, name) + 1000000 * (results[i].Date % 1000));
                            i += j - i - 1;
                            break;

                        }

                    }
                }
                count++;
            }
           // double med = ((int)((sum / 1.00) / count * 100)) / 100.00;
            return sum;
        }
        private int calDay(List<RecitesSchema> day, bool a, string name)
        {
            if (day == null || day.Count() == 0)
                return 0;
            var sum = 0;
            foreach (var recit in day)
            {
                if (recit.items == null || recit.items.Count == 0)
                    continue;
                if (a && recit.Consumer == "مسترجعات")
                    continue;
                if (!a && recit.Consumer != "مسترجعات")
                    continue;
                foreach (var item in recit.items)
                {
                    if (name == null || name == item.Name)
                        sum += item.Amount;
                }
            }
            return sum;
        }
        public async Task<int> GetRecitesDay(DateTime day, bool a, string name)
        {
            int date = day.Year * 10000 + day.Month * 100 + day.Day;
            var results = await GetReciteByDate(date);
            var sum = calDay(results.ToList<RecitesSchema>(), a, name);
            return (sum);
        }

        public async Task<List<RecitesSchema>> GetAllRecites()
        {
            var recites = ConnectToMongo<RecitesSchema>(RecitsCollection);
            var results = await recites.FindAsync(_ => true);
            return results.ToList();
        }
        public async Task<List<RecitesSchema>> GetRecite(int reciteNumber)
        {
            var Recite = ConnectToMongo<RecitesSchema>(RecitsCollection);
            var results = await Recite.FindAsync(r => r.Number == reciteNumber);
            return results.ToList();
        }
        public async Task AddRecit(RecitesSchema recit)
        {
            var Recit = ConnectToMongo<RecitesSchema>(RecitsCollection);
            await Recit.InsertOneAsync(recit);
            return;
        }
        public async Task<List<RecitesSchema>> GetReciteByUser(string user)
        {
            var Recite = ConnectToMongo<RecitesSchema>(RecitsCollection);
            var results = await Recite.FindAsync(r => r.User == user);
            return results.ToList();
        }
        public async Task<List<RecitesSchema>> GetReciteByConsumer(string consumer)
        {
            var Recite = ConnectToMongo<RecitesSchema>(RecitsCollection);
            var results = await Recite.FindAsync(r => r.Consumer == consumer);
            return results.ToList();
        }
        public async Task<List<RecitesSchema>> GetReciteByDate(int date)
        {
            var Recite = ConnectToMongo<RecitesSchema>(RecitsCollection);
            var results = await Recite.FindAsync(r => r.Date == date);
            return results.ToList();
        }
        public async Task<List<RecitesSchema>> GetReciteByState(bool state)
        {
            var Recite = ConnectToMongo<RecitesSchema>(RecitsCollection);
            IAsyncCursor<RecitesSchema> results;
            if (state)
            {
                results = await Recite.FindAsync(r => r.Total == r.Paid);
            }
            else
            {
                results = await Recite.FindAsync(r => r.Total != r.Paid);
            }
            return results.ToList();
        }
        public async Task<List<RecitesSchema>> GetReciteInDateRange(int start, int end)
        {
            var Recite = ConnectToMongo<RecitesSchema>(RecitsCollection);
            var results = await Recite.FindAsync(r => r.Date >= start && r.Date <= end);
            return results.ToList();
        }
        public async Task<List<RecitesSchema>> GetReciteRangeUser(int start, int end, string user)
        {
            var Recite = ConnectToMongo<RecitesSchema>(RecitsCollection);
            var results = await Recite.FindAsync(r => r.Date >= start && r.Date <= end && r.User == user);
            return results.ToList();
        }
        public async Task<List<RecitesSchema>> GetReciteRangeCustomer(int start, int end, string consumer)
        {
            var Recite = ConnectToMongo<RecitesSchema>(RecitsCollection);
            var results = await Recite.FindAsync(r => r.Date >= start && r.Date <= end && r.Consumer == consumer);
            return results.ToList();
        }
        public async Task<List<RecitesSchema>> GetReciteRangeState(int start, int end, bool state)
        {
            var Recite = ConnectToMongo<RecitesSchema>(RecitsCollection);
            IAsyncCursor<RecitesSchema> results;
            if (state)
            {
                results = await Recite.FindAsync(r => r.Date >= start && r.Date <= end && r.Total == r.Paid);
            }
            else
            {
                results = await Recite.FindAsync(r => r.Date >= start && r.Date <= end && r.Total != r.Paid);
            }
            return results.ToList();
        }
        #endregion

        #region DebtMenu

        public async Task AddCustomer(DeptSchema customer)
        {
            var Item = ConnectToMongo<DeptSchema>(DebtCollection);
            await Item.InsertOneAsync(customer);
            return;
        }
        public async Task<string[]> GetAllCustomers()
        {
            var Customers = ConnectToMongo<DeptSchema>(DebtCollection);
            var results = await Customers.FindAsync(_ => true);
            var result = results.ToList();
            if (result.Count() == 0)
            {
                return new string[0];
            }
            string list = result[0].Name;
            bool a;
            foreach (var name in result)
            {
                a = true;
                foreach (var item in list.Split(','))
                {
                    if (name.Name == item)
                    {
                        a = false;
                        break;
                    }
                }
                if (a)
                {
                    list = list + ',' + name.Name;
                }
            }
            return (string[])list.Split(',');
        }
        public Task UpdateCustomer(DeptSchema item)
        {
            var Item = ConnectToMongo<DeptSchema>(DebtCollection);
            var fitler = Builders<DeptSchema>.Filter.Eq("Id", item.Id);
            return Item.ReplaceOneAsync(fitler, item, new ReplaceOptions { IsUpsert = true });
        }
        public async Task<List<DeptSchema>> GetCustomer(string itemName)
        {
            var Item = ConnectToMongo<DeptSchema>(DebtCollection);
            var results = await Item.FindAsync(i => i.Name == itemName);
            return results.ToList();
        }
        #endregion

        #region Activities Functions
        public async Task<List<ActivitiesSchema>> GetAllActivites()
        {
            var users = ConnectToMongo<ActivitiesSchema>(ActvititesCollection);
            var results = await users.FindAsync(_ => true);
            return results.ToList();
        }
        public async Task AddActivity(ActivitiesSchema activity)
        {
            var User = ConnectToMongo<ActivitiesSchema>(ActvititesCollection);
            await User.InsertOneAsync(activity);
            return;
        }
        public async Task<List<ActivitiesSchema>> GetAcitivitesByUser(string username)
        {
            var Item = ConnectToMongo<ActivitiesSchema>(ActvititesCollection);
            var results = await Item.FindAsync(i => i.User == username);
            return results.ToList();
        }
        public async Task<List<ActivitiesSchema>> GetActivityByReason(string reason)
        {
            var Item = ConnectToMongo<ActivitiesSchema>(ActvititesCollection);
            var results = await Item.FindAsync(i => i.Reason == reason);
            return results.ToList();
        }
        public async Task<string[]> GetReasons()
        {
            var Purches = ConnectToMongo<ActivitiesSchema>(ActvititesCollection);
            var results = await Purches.FindAsync(_ => true);
            var result = results.ToList();
            string list = result[0].Reason;
            bool a;
            foreach (var name in result)
            {
                a = true;
                foreach (var item in list.Split(','))
                {
                    if (name.Reason == item)
                    {
                        a = false;
                        break;
                    }
                }
                if (a)
                {
                    list = list + ',' + name.Reason;
                }
            }
            return (string[])list.Split(',');
        }
        #endregion

        #region WorkSchedule Functions
        public async Task<List<WorkSchedule>> GetAllOpenSchedules()
        {
            var Item = ConnectToMongo<WorkSchedule>(WorkCollection);
            var results = await Item.FindAsync(i => i.Open == true);
            return results.ToList();
        }

        public async Task<List<WorkSchedule>> GetScheduleByName(string name)
        {
            var users = ConnectToMongo<WorkSchedule>(WorkCollection);
            var results = await users.FindAsync(a => a.User == name);
            return results.ToList();
        }
        public async Task<List<WorkSchedule>> GetWorkScheduleByUser(string user)
        {
            var Item = ConnectToMongo<WorkSchedule>(WorkCollection);
            var results = await Item.FindAsync(i => i.User == user);
            return results.ToList();
        }

        public Task UpdateWorkSchedule(WorkSchedule item)
        {
            var Item = ConnectToMongo<WorkSchedule>(WorkCollection);
            var fitler = Builders<WorkSchedule>.Filter.Eq("Id", item.Id);
            return Item.ReplaceOneAsync(fitler, item, new ReplaceOptions { IsUpsert = true });
        }

        public async Task AddSchedule(WorkSchedule item)
        {
            var Item = ConnectToMongo<WorkSchedule>(WorkCollection);
            await Item.InsertOneAsync(item);
            return;
        }
        #endregion

        #region Settings

        public async Task<List<SettingsSchema>> getSetting()
        {
            var Setting = ConnectToMongo<SettingsSchema>(setingsCollection);
            var result = await Setting.FindAsync(_ => true);
            return (result.ToList());
        }
        public async Task AddSetting(SettingsSchema setting)
        {
            var Setting = ConnectToMongo<SettingsSchema>(setingsCollection);
            await Setting.InsertOneAsync(setting);
            return;
        }
        public Task UpdateSetting(SettingsSchema setting)
        {
            var Setting = ConnectToMongo<SettingsSchema>(setingsCollection);
            var fitler = Builders<SettingsSchema>.Filter.Eq("Id", setting.Id);
            return Setting.ReplaceOneAsync(fitler, setting, new ReplaceOptions { IsUpsert = true });
        }
        public Task DeleteSetting(SettingsSchema setting)
        {
            var Setting = ConnectToMongo<SettingsSchema>(setingsCollection);
            return Setting.DeleteOneAsync(i => i.Id == setting.Id);
        }

        #endregion

        #region Accountant
        public async Task<GraphAcount> Accountant(DateTime start, int number, DateTime end)
        {
            number -= 1;
            GraphAcount acount = new GraphAcount();
            if (end != DateTime.MinValue)
            {
                if (start < end)
                {
                    DateTime tmp = end;
                    end = start;
                    start = tmp;
                }

            }
            else
            {
                int year = start.Year, month = start.Month, day = start.Day - number;
                while (true)
                {
                    int dayCount = (int) ((new DateTime(year, month, 1).Ticks - new DateTime(year, month - 1, 1).Ticks) / (60 * 60 * 24) / 10000000);
                    if (day > 0 && day <= dayCount) {
                        break;
                    } else {
                        day += dayCount;
                        month = --month > 1 ? month : 12;
                        year = month == 12 ? year - 1 : year;
                    }
                }
                end = new DateTime(year, month, day);
            }
            /////////////////////////////////////////////
            int startint = start.Year * 10000 + start.Month * 100 + start.Day;
            int endint = end.Year * 10000 + end.Month * 100 + end.Day;
            var results = await GetReciteInDateRange(endint, startint);
            if (results == null || results.ToList<RecitesSchema>().Count() == 0)
                return acount;
            for (int i = 0; i < results.Count(); i++)
            {
                if (i == results.Count() - 1)
                {
                    acount.Sold.Add(calDay(results.GetRange(i, 1), true, null) + 1000000 * (results[i].Date % 10000));
                    acount.Retuned.Add(calDay(results.GetRange(i, 1), false, null) + 1000000 * (results[i].Date % 10000));
                    break;
                }
                else
                {
                    for (int j = i + 1; j < results.Count() || j == results.Count() - 1; j++)
                    {
                        if (j == results.Count() - 1 && results[i].Date != results[j].Date)
                        {
                            acount.Sold.Add(calDay(results.GetRange(i, j - i), true, null) + 1000000 * (results[i].Date % 10000));
                            acount.Retuned.Add(calDay(results.GetRange(i, j - i), false, null) + 1000000 * (results[i].Date % 10000));
                            i += j - i - 1;
                            break;
                        }
                        else if (j == results.Count() - 1)
                        {
                            acount.Sold.Add(calDay(results.GetRange(i, j + 1 - i), true, null) + 1000000 * (results[i].Date % 10000));
                            acount.Retuned.Add(calDay(results.GetRange(i, j + 1 - i), false, null) + 1000000 * (results[i].Date % 10000));
                            i += j - i;
                            break;

                        }
                        else if (results[i].Date != results[j].Date)
                        {
                            acount.Sold.Add(calDay(results.GetRange(i, j - i), true, null) + 1000000 * (results[i].Date % 10000));
                            acount.Retuned.Add(calDay(results.GetRange(i, j - i), true, null) + 1000000 * (results[i].Date % 10000));
                            i += j - i - 1;
                            break;
                        }
                        int year = results[i].Date / 10000;
                        int month = results[i].Date % 10000 / 100;
                        int day = results[i].Date % 100;
                        if (acount.Retuned.Count() != 0 && acount.Retuned[acount.Retuned.Count() - 1] != 0)
                            acount.WeekMid.CountReturn[((int)new DateTime(year, month, day).DayOfWeek)]++;
                        if (acount.Sold.Count() != 0 && acount.Sold[acount.Sold.Count() - 1] != 0) 
                            acount.WeekMid.CountSell[((int)new DateTime(month, month, day).DayOfWeek)]++;
                    }

                }
            }
            /////////////////////////////////////////////
            bool a;
            foreach (var items in results)
            {
                int year = items.Date / 10000;
                int month = items.Date % 10000 / 100;
                int day = items.Date % 100;
                a = true;
                if (items.Consumer == "مسترجعات")
                {
                    acount.TotalReturn += items.Total;
                    acount.CountReturn++;
                    acount.WeekMid.Return[((int) new DateTime(year, month, day).DayOfWeek)] += items.Total;
                    foreach (var user in acount.usersLogs)
                    {
                        if (items.User == user.Name)
                        {
                            user.TotalReturn += items.Total;
                            a = false;
                            break;
                        }
                    }

                    if (a)
                    {
                        acount.usersLogs.Add(new UsersLog());
                        acount.usersLogs.Last().Name = items.User;
                        acount.usersLogs.Last().TotalReturn += items.Total;
                    }
                }
                else
                {
                    acount.TotalSell += items.Paid;
                    acount.CountSell++;
                    acount.WeekMid.Sell[((int)new DateTime(year, month, day).DayOfWeek)] += items.Paid;
                    foreach (var user in acount.usersLogs)
                    {
                        if (items.User == user.Name)
                        {
                            user.TotalSell += items.Paid;
                            a = false;
                            break;
                        }
                    }
                    if (a)
                    {
                        acount.usersLogs.Add(new UsersLog());
                        acount.usersLogs.Last().Name = items.User;
                        acount.usersLogs.Last().TotalSell += items.Paid;
                    }
                }
                foreach (var item in items.items)
                {
                    bool b = true;
                    ///////////// handle item log ///////
                    foreach (var itemlog in acount.itemslog)
                    {
                        if (itemlog.Name == item.Name)
                        {
                            if (items.Consumer == "مسترجعات")
                            {
                                itemlog.AmountReturn += item.Amount;
                                itemlog.TotalReturn += item.Total;
                            }
                            else
                            {
                                itemlog.AmountSell += item.Amount;
                                itemlog.TotalSell += item.Total;
                            }
                            b = false;
                            break;
                        }
                    }
                    if (b)
                    {
                        acount.itemslog.Add(new itemslog());
                        acount.itemslog.Last().Name = item.Name;
                        if (items.Consumer == "مسترجعات")
                        {
                            acount.itemslog.Last().AmountReturn += item.Amount;
                            acount.itemslog.Last().TotalReturn += item.Total;
                        }
                        else 
                        {
                            acount.itemslog.Last().AmountSell += item.Amount;
                            acount.itemslog.Last().TotalSell += item.Total;
                        }
                    }
                    ///////////// handle item log ///////

                    foreach (var item1 in item.info)
                    {
                        if (items.Consumer == "مسترجعات")
                        {
                            acount.buyReturn += item1.BuyPrice * item1.Amount;
                            foreach (var user in acount.usersLogs)
                            {
                                if (items.User == user.Name)
                                {
                                    user.buyReturn += item1.BuyPrice * item1.Amount;
                                    a = false;
                                    break;
                                }
                            }
                            if (a)
                            {
                                acount.usersLogs.Add(new UsersLog());
                                acount.usersLogs.Last().Name = items.User;
                                acount.usersLogs.Last().buyReturn += item1.BuyPrice * item1.Amount;
                            }
                        }
                        else
                        {
                            acount.buySell += item1.BuyPrice * item1.Amount;
                            foreach (var user in acount.usersLogs)
                            {
                                if (items.User == user.Name)
                                {
                                    user.buySell += item1.BuyPrice * item1.Amount;
                                    a = false;
                                    break;
                                }
                            }
                            if (a)
                            {
                                acount.usersLogs.Add(new UsersLog());
                                acount.usersLogs.Last().Name = items.User;
                                acount.usersLogs.Last().buySell += item1.BuyPrice * item1.Amount;
                            }
                        }
                    }
                }
            }
            /////////////////////////////////////////////////
            acount.NetReturn = acount.TotalReturn - acount.buyReturn;
            acount.NetSell = acount.TotalSell - acount.buySell;
            foreach (var item in acount.usersLogs)
            {
                item.NetReturn = item.TotalReturn - item.buyReturn;
                item.NetSell = item.TotalSell - item.buySell;
            }
            for (int i = 0; i < 7;i++)
            {
                acount.WeekMid.Return[i] /= acount.WeekMid.CountReturn[i] == 0 ? 1 : acount.WeekMid.CountReturn[i];
                acount.WeekMid.Sell[i] /= acount.WeekMid.CountSell[i] == 0 ? 1 : acount.WeekMid.CountSell[i];
            }
            acount.RemainedItems = await GetRemainedItemsInfo();
            acount = await GetNets(acount);
            return accountTrim(acount);
        }

        public async Task<GraphAcount> GetNets(GraphAcount acount)
        {
            var items = ConnectToMongo<ItemsSchema>(ItemsCollection);
            var result = await items.FindAsync(a => a.Info.Any(b => b.Amount > 0));
            List<ItemsSchema> results = result.ToList<ItemsSchema>();
            acount.CountNet_Items = results.Count;
            var amount = 0.0;
            foreach (var item in results)
            {
                foreach (var item1 in item.Info)
                {
                    amount += item1.Amount;
                }
            }
            acount.CountNet_ItemsAmount = ((int) amount * 100) / 100;
            acount.TotalNet = await GetTotalNet();
            return acount;
        }
        public async Task<double> GetTotalNet()
        {
            var items = ConnectToMongo<ItemsSchema>(ItemsCollection);
            var results = await items.FindAsync(_ => true);
            var total = 0.0;
            foreach (var item in results.ToList<ItemsSchema>())
            {
                foreach (var item1 in item.Info)
                {
                    total += item1.BuyPrice * item1.Amount;
                }
            }
            return ((int)total * 100) / 100;
        }

        public async Task<List<RemainedItemsInfo>> GetRemainedItemsInfo()
        {
            var ItemsInfo = new List<RemainedItemsInfo>();
            var results = await GetAllItems();
            bool a;
            foreach (var item in results)
            {
                a = true;
                foreach (var saved in ItemsInfo)
                {
                    if (item.Name == saved.Name)
                    {
                        if (item.Info != null)
                        {
                            foreach (var info in item.Info)
                            {
                                saved.count += info.Amount;
                            }
                        }
                        a = false;
                        break;
                    }
                }
                if (a)
                {
                    RemainedItemsInfo tmp = new RemainedItemsInfo();
                    tmp.Name = item.Name;
                    if (item.Info != null)
                    {
                        foreach (var info in item.Info)
                        {
                            tmp.count += info.Amount;
                        }
                    }
                    ItemsInfo.Add(tmp);
                }
            }
            return ItemsInfo;
        }

        public GraphAcount accountTrim(GraphAcount a)
        {
            a.buyReturn = Trim(a.buyReturn);
            a.TotalReturn = Trim(a.TotalReturn);
            a.NetReturn = Trim(a.NetReturn);
            a.NetSell = Trim(a.NetSell);
            a.TotalSell = Trim(a.TotalSell);
            a.buySell = Trim(a.buySell);

            for (int i = 0; i < a.RemainedItems.Count(); i++)
            {
                a.RemainedItems[i].count = Trim(a.RemainedItems[i].count);
            }

            for (int i = 0; i < a.itemslog.Count(); i++)
            {
                a.itemslog[i].TotalSell = Trim(a.itemslog[i].TotalSell);
                a.itemslog[i].TotalReturn = Trim(a.itemslog[i].TotalReturn);
                a.itemslog[i].AmountSell = Trim(a.itemslog[i].AmountSell);
                a.itemslog[i].AmountReturn = Trim(a.itemslog[i].AmountReturn);
            }

            for (int i = 0; i < a.usersLogs.Count(); i++)
            {
                a.usersLogs[i].TotalSell = Trim(a.usersLogs[i].TotalSell);
                a.usersLogs[i].NetSell = Trim(a.usersLogs[i].NetSell);
                a.usersLogs[i].buySell = Trim(a.usersLogs[i].buySell);
                a.usersLogs[i].TotalReturn = Trim(a.usersLogs[i].TotalReturn);
                a.usersLogs[i].NetReturn = Trim(a.usersLogs[i].NetReturn);
                a.usersLogs[i].buyReturn = Trim(a.usersLogs[i].buyReturn);
            }

            for (int i = 0; i < 7; i++)
            {
                a.WeekMid.Sell[i] = Trim(a.WeekMid.Sell[i]);
                a.WeekMid.CountSell[i] = Trim(a.WeekMid.CountSell[i]);
                a.WeekMid.Return[i] = Trim(a.WeekMid.Return[i]);
                a.WeekMid.CountReturn[i] = Trim(a.WeekMid.CountReturn[i]);
            }
            return a;
        }
        public double Trim(double number)
        {
            return ((int) (number * 100) / 100.0);
        }
        #endregion


        #region Cheat Codes
        public async void deleteDB()
        {
            var client = new MongoClient(ConnectionString);
            await client.DropDatabaseAsync(DatabaseName);
        }
        public async Task UpdateAllPurchase()
        {
            var purches = await GetAllPurches();
            var items = await GetAllItems();
            foreach (var purech in purches.ToList<PurchaseSchema>())
            {
                foreach (var pur in purech.Items)
                {
                    foreach (var item in items.ToList<ItemsSchema>())
                    {
                        if (pur.Name == item.Name)
                        {
                            if (item.SellPrice == 0)
                            {
                                item.SellPrice = pur.SellPrice;
                                await UpdateItem(item);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
