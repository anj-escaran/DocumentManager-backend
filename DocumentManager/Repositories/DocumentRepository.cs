using DocumentManager.Repositories;
using DocumentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocumentDBContext _db;

        public DocumentRepository(DocumentDBContext context) => _db = context;

        public async Task<Document> AddDocument(Document document)
        {
            try
            {
                _db.Document.Add(document);
                await _db.SaveChangesAsync();
              
                return document;
            }
            catch (Exception)
            {
               
                throw;
            }
        }

        public async Task<bool> DeleteDocument(int id)
        {
            try
            {
                var document = _db.Document.Find(id);
                if (document != null) _db.Document.Remove(document);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteDocuments(int[] ids)
        {
            try
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    var document = _db.Document.Find(ids[i]);
                    if (document != null) _db.Document.Remove(document);
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Document> GetDocument(int id) => await _db.Document.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Document>> GetDocuments() => await _db.Document.ToListAsync();

        public async Task<Document> UpdateDocument(Document document)
        {
            try
            {
                _db.Entry(document).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return document;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}