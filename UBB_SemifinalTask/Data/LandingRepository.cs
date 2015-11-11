using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using HtmlAgilityPack;
using UBB_SemifinalTask.Models;

namespace UBB_SemifinalTask.Data
{
    public class LandingRepository : ILandingRepository
    {
        private bool _disposed;
        private readonly ApplicationDbContext _applicationDbContext;
        public LandingRepository()
        {
            _applicationDbContext = new ApplicationDbContext();
        }
        public async Task<string> GetStylesAsync(string authorId)
        {
            var item = await _applicationDbContext.LandingResources.FirstOrDefaultAsync(r => r.AuthorId == authorId);
            return item?.Styles;
        }
        public async Task<string> GetContentAsync(string authorId)
        {
            var item = await _applicationDbContext.LandingResources.FirstOrDefaultAsync(r => r.AuthorId == authorId);
            return item?.Content;
        }
        public async Task<LandingResources> GetPageAsync(int id)
        {
            var item = await _applicationDbContext.LandingResources.FirstOrDefaultAsync(r => r.Id == id);
            return item;
        }

        public async Task<LandingResources> AddStylesAsync(LandingResources item)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(r => r.Id == item.AuthorId);
            if (user == null) return null;
            item.AuthorId = user.Id;

            var existingItem = await _applicationDbContext.LandingResources.FirstOrDefaultAsync(r => r.AuthorId == item.AuthorId);
            if (existingItem != null)
            {
                existingItem.Styles = item.Styles;

                _applicationDbContext.LandingResources.Attach(existingItem);
                _applicationDbContext.Entry(existingItem).Property(x => x.Styles).IsModified = true;
                await _applicationDbContext.SaveChangesAsync();
                return existingItem;
            }

            _applicationDbContext.LandingResources.Add(item);
            await _applicationDbContext.SaveChangesAsync();
            return item;
        }
        public async Task<LandingResources> AddContentAsync(LandingResources item)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(r => r.Id == item.AuthorId);
            if (user == null) return null;
            item.AuthorId = user.Id;
            //var existingItem = await _applicationDbContext.LandingResources.FirstOrDefaultAsync(r => r.AuthorId == item.AuthorId);
            //if (existingItem != null)
            //{
            //    existingItem.Content = item.Content;
            //    existingItem.LinkOnUbb = item.LinkOnUbb;

            //    _applicationDbContext.LandingResources.Attach(existingItem);
            //    _applicationDbContext.Entry(existingItem).Property(x => x.Content).IsModified = true;
            //    _applicationDbContext.Entry(existingItem).Property(x => x.LinkOnUbb).IsModified = true;
            //    await _applicationDbContext.SaveChangesAsync();
            //    return existingItem;
            //}

            _applicationDbContext.LandingResources.Add(item);
            await _applicationDbContext.SaveChangesAsync();
            return item;
        }
        public async Task<MoneyReturnModel> GetMoney(int id)
        {
            var item = await _applicationDbContext.LandingResources.FirstOrDefaultAsync(r => r.Id == id);
            if (item != null)
            {
                bool isWellFormedUriString = Uri.IsWellFormedUriString(item.LinkOnUbb, UriKind.Absolute);
                if (isWellFormedUriString)
                {
                    HtmlWeb htmlWeb = new HtmlWeb();
                    HtmlDocument document = htmlWeb.Load(item.LinkOnUbb);
                    HtmlNode sumOfMoney = document.GetElementbyId("vote");
                    if (sumOfMoney != null)
                    {
                        var sum = sumOfMoney.ChildNodes[1];
                        HtmlNode nodeRemain = document.GetElementbyId("need_pay");
                        var stillNeed = nodeRemain.InnerHtml;
                        return new MoneyReturnModel() { StillNeeded = stillNeed, Sum = sum.InnerHtml };
                    }
                }
            }
            return null;
        }
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _applicationDbContext.Dispose();
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}