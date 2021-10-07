using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManager.Repositories
{
    public interface IDocumentRepository
    {
        Task<Document> AddDocument(Document document);
        Task<Document> UpdateDocument(Document document);
        Task<bool> DeleteDocument(int id);
        Task<bool> DeleteDocuments(int[] ids);
        Task<IEnumerable<Document>> GetDocuments();
        Task<Document> GetDocument(int id);
    }
}
