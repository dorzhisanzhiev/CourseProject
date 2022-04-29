using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourseProject.Vigenere;
using System.Text;
using System.IO;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

namespace CourseProjectAsp.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
        private IWebHostEnvironment _environment;
        public IndexModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        [BindProperty]
        public IFormFile Upload { get; set; }
        [BindProperty]
        public string Input { get; set; }
        public string SaveResult { get; set; }
        public string Output { get; set; }
        [BindProperty]
        public string Key { get; set; }
        [BindProperty]
        public string FileDirectory { get; set; }
        [BindProperty]
        public string FileName { get; set; }
        [BindProperty]
        public string Extension { get; set; }
        public void OnPostUploadFile()
        {
            if (Path.GetExtension(Upload.FileName) == ".txt")
            {
                StringBuilder result = new StringBuilder();
                using (StreamReader reader = new StreamReader(Upload.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                        result.AppendLine(reader.ReadLine());
                }
                FileMessage.Input = result.ToString().TrimEnd('\r', '\n');
                Input = FileMessage.Input;
            }
            if (Path.GetExtension(Upload.FileName) == ".docx")
            {
                string docText = string.Empty;
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(Upload.OpenReadStream(), false))
                {
                    foreach (Paragraph p in wordDocument.MainDocumentPart.Document.Body.Descendants<Paragraph>())
                    {
                        docText += p.InnerText + "\n";
                    }
                }
                FileMessage.Input = docText.TrimEnd('\r', '\n');
                Input = FileMessage.Input;
            }
        }
        public void OnPostEnter()
        {
            FileMessage.Input = Input;
        }
        public void OnPostSave()
        {
            if (!Directory.Exists(FileDirectory))
            {
                SaveResult = "Такой директории нет!";
                Output = FileMessage.Output;
                Input = FileMessage.Input;
                return;
            }
            FileName += Extension;
            var fileUploadPath = Path.Combine(FileDirectory, FileName);
            if (Extension == ".txt")
            {
                try
                {
                    using (var fileStream = new FileStream(fileUploadPath, FileMode.Create))
                    {
                    }
                    System.IO.File.WriteAllText(fileUploadPath, FileMessage.Output);
                    SaveResult = "Успех!";
                }
                catch (Exception ex)
                {
                    SaveResult = ex.Message;
                }
            }
            if (Extension == ".docx")
            {
                try
                {
                    string[] outputLines = FileMessage.Output.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(fileUploadPath, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        Body body = mainPart.Document.AppendChild(new Body());
                        foreach (string line in outputLines)
                        {
                            Paragraph para = body.AppendChild(new Paragraph());
                            Run run = para.AppendChild(new Run());
                            run.AppendChild(new Text(line));
                        }
                    }
                    SaveResult = "Успех!";
                }
                catch (Exception ex)
                {
                    SaveResult = ex.Message;
                }
            }
            Output = FileMessage.Output;
            Input = FileMessage.Input;
        }
        public void OnPostEncrypt()
        {
            FileMessage.Output = Vigenere.Encrypt(FileMessage.Input, Key);
            Output = FileMessage.Output;
            Input = FileMessage.Input;
        }
        public void OnPostDecrypt()
        {
            FileMessage.Output = Vigenere.Decrypt(FileMessage.Input, Key);
            Output = FileMessage.Output;
            Input = FileMessage.Input;
        }
        public class FileMessage
        {
            public static string Input { get; set; }
            public static string Output { get; set; }
        }
    }
}