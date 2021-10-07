using System.Threading.Tasks;
using System.Net;
using DocumentManager.Models;
using System.IO;
using System;
using Microsoft.AspNetCore.Mvc;
using DocumentManager.Repositories;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using Firebase.Storage;
using Firebase.Auth;
using Microsoft.AspNetCore.Http;

namespace DocumentManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepo;
        private readonly string apikey = "AIzaSyARCSsAACIELOLR5MF-IDc0Hp_oMweMTvQ";
        private readonly string bucket = "documents-1772a.appspot.com";
        private readonly string authEmail = "admin.document@gmail.com";
        private readonly string authPassword = "P@ssword01";
        public DocumentController(IDocumentRepository repository)
        {
            _documentRepo = repository;
        }
        //Get Documents
        [HttpGet]
        public async Task<IEnumerable<Document>> Get()
        {
            return await _documentRepo.GetDocuments();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            var document = await _documentRepo.GetDocument(id);
            if (document == null)
            {
                return NotFound();
            }

            return document;

        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Add([FromForm] DocumentFile document)
        {
            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

            string fileName = Path.GetFileName(document.File.FileName);

            var auth = new FirebaseAuthProvider(new FirebaseConfig(apikey));

            var a = await auth.SignInWithEmailAndPasswordAsync(authEmail, authPassword);
            //var a = await auth.SignInAnonymouslyAsync();

            var cancellation = new CancellationTokenSource();

            if (!Directory.Exists(pathBuilt)) {
                Directory.CreateDirectory(pathBuilt);
            } 
            FileStream fs = null;
            var path = Path.Combine(pathBuilt, document.File.FileName);

            //fs = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            //await document.File.CopyToAsync(fs);
            using (fs = new FileStream(Path.Combine(path), FileMode.Create))
            {
                await document.File.CopyToAsync(fs);
            }

            fs = new FileStream(Path.Combine(path), FileMode.Open);

            var downloadUrl = await new FirebaseStorage
                (
                bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true,
                })
                .Child("assets")
                .Child($"{fileName}")
                .PutAsync(fs, cancellation.Token);


            var newDocument = new Document
            {
                ControlNumber = document.ControlNumber,
                Title = document.Title,
                Type = document.Type,
                FilePath = downloadUrl,
                Revision = document.Revision,
                UpdatedDate = DateTime.Now,
                Filename = fileName
            };
            var newDoc = await _documentRepo.AddDocument(newDocument);
            //return CreatedAtAction(nameof(Get), new { id = newDoc.Id }, newDoc);
            return Ok(new Response { Status = "Success", Message = "Document successfully added." });

        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Document>> Edit(int id, [FromForm] DocumentFile document)
        {
            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

            string fileName = Path.GetFileName(document.File.FileName);

            var auth = new FirebaseAuthProvider(new FirebaseConfig(apikey));

            var a = await auth.SignInWithEmailAndPasswordAsync(authEmail, authPassword);
            //var a = await auth.SignInAnonymouslyAsync();

            var cancellation = new CancellationTokenSource();

            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }
            FileStream fs = null;
            var path = Path.Combine(pathBuilt, document.File.FileName);

            //fs = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            //await document.File.CopyToAsync(fs);
            using (fs = new FileStream(Path.Combine(path), FileMode.Create))
            {
                await document.File.CopyToAsync(fs);
            }

            fs = new FileStream(Path.Combine(path), FileMode.Open);

            var downloadUrl = await new FirebaseStorage
                (
                bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true,
                })
                .Child("assets")
                .Child($"{fileName}")
                .PutAsync(fs, cancellation.Token);
            //if (id != document.Id)
            //{
            //    return BadRequest();
            //}
            var editdocument = new Document
            {
                Id = id,
                ControlNumber = document.ControlNumber,
                Title = document.Title,
                Type = document.Type,
                FilePath = downloadUrl,
                Revision = document.Revision,
                UpdatedDate = DateTime.Now,
                Filename = fileName
            };
            await _documentRepo.UpdateDocument(editdocument);

            return Ok(new Response { Status = "Success", Message = "Document successfully updated." });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Document>> Delete(int id)
        {
            var documentToDelete = await _documentRepo.GetDocument(id);                        
            if (documentToDelete == null)
            {
                return NotFound();
            }
            var result = await _documentRepo.DeleteDocument(documentToDelete.Id);
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Error in deleting  document" });
            }
            return Ok(new Response { Status = "Success", Message = "Successfully deleted." });

        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<ActionResult<Document>> DeleteMultiple(int[] ids)
        {
            var result = await _documentRepo.DeleteDocuments(ids);

            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Error in deleting multiple documents" });
            }
            return Ok(new Response { Status = "Success", Message = "Successfully deleted." });
        }
    }
}
