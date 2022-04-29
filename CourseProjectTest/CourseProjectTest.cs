using CourseProject.Vigenere;
using CourseProjectAsp;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace CourseProjectTest
{
    [TestClass]
    public class CourseProjectTest
    {
        [TestMethod]
        public void VigenereTest_Encrypt()
        {
            string expected = "бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!!";
            string actual = Vigenere.Encrypt("поздравляю, ты получил исходный текст!!!", "скорпион");
            Assert.AreEqual(expected, actual);
            expected = "б9щцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!!";
            actual = Vigenere.Encrypt("п9оздравляю, ты получил исходный текст!!!", "с9корпион");
            Assert.AreEqual(expected, actual);
            expected = "б9щцфаирщри, бл ячъбиуъ щбюэсяёш \nгфуаа!!!";
            actual = Vigenere.Encrypt("п9оздравляю, ты получил исходный \nтекст!!!", "с9корпион");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void VigenereTest_Decrypt()
        {
            string expected = "поздравляю, ты получил исходный текст!!!";
            string actual = Vigenere.Decrypt("бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!!", "скорпион");
            Assert.AreEqual(expected, actual);
            expected = "п9оздравляю, ты получил исходный текст!!!";
            actual = Vigenere.Decrypt("б9щцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!!", "с9корпион");
            Assert.AreEqual(expected, actual);
            expected = "п9оздравляю, ты получил исходный \nтекст!!!";
            actual = Vigenere.Decrypt("б9щцфаирщри, бл ячъбиуъ щбюэсяёш \nгфуаа!!!", "с9корпион");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TxTFileTest_Decrypt()
        {
            string fileName = @".\testFiles\Result_v5.txt"; //это в папке дебага, если потеряли
            StringBuilder result = new StringBuilder();
            using (StreamReader reader = new StreamReader(fileName))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            string actualInput = result.ToString();
            result = new StringBuilder();
            fileName = @".\testFiles\Result_v5_DecryptionResult.txt";
            using (StreamReader reader = new StreamReader(fileName))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            string actualOutput = result.ToString();

            string actual = Vigenere.Decrypt(actualInput, "скорпион");
            string expected = actualOutput;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void DocxFileTest_Decrypt()
        {
            string fileName = @".\testFiles\Result_v5.docx"; //это в папке дебага, если потеряли
            string docText = string.Empty;
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(fileName, false))
            {
                foreach (Paragraph p in wordDocument.MainDocumentPart.Document.Body.Descendants<Paragraph>())
                {
                    docText += p.InnerText + "\n";
                }
            }
            string actualInput = docText;

            fileName = @".\testFiles\Result_v5_DecryptionResult.docx";
            docText = string.Empty;
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(fileName, false))
            {
                foreach (Paragraph p in wordDocument.MainDocumentPart.Document.Body.Descendants<Paragraph>())
                {
                    docText += p.InnerText + "\n";
                }
            }
            string actualOutput = docText;

            string actual = Vigenere.Decrypt(actualInput, "скорпион");
            string expected = actualOutput;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TxTFileTest_Save()
        {
            string fileDirectory = @".\testFiles\";
            string fileName = "SaveResult";
            string extension = ".txt";
            fileName += extension;
            var fileUploadPath = Path.Combine(fileDirectory, fileName);
            string output = "бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!! ";
            try
            {
                using (var fileStream = new FileStream(fileUploadPath, FileMode.Create))
                {
                }
                File.WriteAllText(fileUploadPath, output);
            }
            catch
            {
            }
            StringBuilder result = new StringBuilder();
            using (StreamReader reader = new StreamReader(fileUploadPath))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            string actual = result.ToString().TrimEnd('\r', '\n');
            string expected = output;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void DocxFileTest_Save()
        {
            string fileDirectory = @".\testFiles\";
            string fileName = "SaveResult";
            string extension = ".docx";
            fileName += extension;
            var fileUploadPath = Path.Combine(fileDirectory, fileName);
            string output = "бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!! ";
            try
            {
                string[] outputLines = output.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
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
            }
            catch
            {
            }
            string docText = string.Empty;
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(fileUploadPath, false))
            {
                foreach (Paragraph p in wordDocument.MainDocumentPart.Document.Body.Descendants<Paragraph>())
                {
                    docText += p.InnerText + "\n";
                }
            }
            string actual = docText.TrimEnd('\r', '\n');
            string expected = output;
            Assert.AreEqual(expected, actual);
        }
    }
}